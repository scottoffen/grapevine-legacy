using System;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using Grapevine.Server.Exceptions;
using Grapevine.Util;
using HttpStatusCode = Grapevine.Util.HttpStatusCode;
using ExtendedProtectionSelector = System.Net.HttpListener.ExtendedProtectionSelector;

namespace Grapevine.Server
{
    /// <summary>
    /// Provides a programmatically controlled REST implementation for a single Prefix using HttpListener
    /// </summary>
    public interface IRestServer : IServerProperties, IDynamicProperties, IDisposable
    {
        /// <summary>
        /// Gets a value that indicates whether HttpListener has been started
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Gets the prefix created by combining the Protocol, Host and Port properties into a scheme and authority
        /// </summary>
        string Origin { get; }

        /// <summary>
        /// Starts the server: executes OnBeforeStart, starts the HttpListener, then executes OnAfterStart if the HttpListener is listening
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server; executes OnBeforeStop, stops the HttpListener, then executes OnAfterStop is the HttpListener is not listening
        /// </summary>
        void Stop();
    }

    public class RestServer : DynamicProperties, IRestServer
    {
        private string _host;
        private string _port;
        private string _protocol;
        private int _connections;
        protected bool IsStopping;
        protected readonly HttpListener Listener;
        protected readonly Thread Listening;
        protected readonly ConcurrentQueue<HttpListenerContext> Queue;
        protected readonly ManualResetEvent ReadyEvent, StopEvent;
        protected Thread[] Workers;

        public bool EnableThrowingExceptions { get; set; }
        public IGrapevineLogger Logger { get; set; }
        public Action OnBeforeStart { get; set; }
        public Action OnAfterStart { get; set; }
        public Action OnBeforeStop { get; set; }
        public Action OnAfterStop { get; set; }
        public IRouter Router { get; set; }

        public RestServer() : this(new ServerOptions()) { }

        public RestServer(ServerOptions options)
        {
            Listener = new HttpListener();
            Listening = new Thread(HandleRequests);
            Queue = new ConcurrentQueue<HttpListenerContext>();
            ReadyEvent = new ManualResetEvent(false);
            StopEvent = new ManualResetEvent(false);

            Connections = options.Connections;
            Host = options.Host;
            Logger = options.Logger;
            OnBeforeStart = options.OnBeforeStart;
            OnAfterStart = options.OnAfterStart;
            OnBeforeStop = options.OnBeforeStop;
            OnAfterStop = options.OnAfterStop;
            Port = options.Port;
            Protocol = options.Protocol;
            PublicFolder = options.PublicFolder;
            Router = options.Router;

            Advanced = new AdvancedRestServer(Listener);
            Listener.IgnoreWriteExceptions = true;
        }

        public static RestServer For(Action<ServerOptions> configure)
        {
            var options = new ServerOptions();
            configure(options);
            return new RestServer(options);
        }

        public static RestServer For<T>() where T : ServerOptions, new()
        {
            return new RestServer(new T());
        }

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpListener instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedRestServer Advanced { get; }

        public int Connections
        {
            get { return _connections; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _connections = value;
            }
        }

        public string Host
        {
            get { return _host; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _host = value == "0.0.0.0" ? "+" : value.ToLower();
            }
        }

        public bool IsListening => Listener?.IsListening ?? false;

        public Action OnStart
        {
            get { return OnAfterStart; }
            set { OnAfterStart = value; }
        }

        public Action OnStop
        {
            get { return OnAfterStop; }
            set { OnAfterStop = value; }
        }

        public string Origin => $"{Protocol}://{Host}:{Port}/";

        public string Port
        {
            get { return _port; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _port = value;
            }
        }

        public string Protocol
        {
            get { return _protocol; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _protocol = value.ToLower();
            }
        }

        public PublicFolder PublicFolder { get; }

        public void Start()
        {
            if (IsListening) return;
            try
            {
                if (Router.RoutingTable.Count == 0) Router.RegisterAssembly();

                OnBeforeStart?.Invoke();

                IsStopping = false;
                Listener.Prefixes.Add(Origin);
                Listener.Start();
                Listening.Start();

                Workers = new Thread[_connections * Environment.ProcessorCount];
                for (var i = 0; i < Workers.Length; i++)
                {
                    Workers[i] = new Thread(Worker);
                    Workers[i].Start();
                }

                if (IsListening) OnAfterStart?.Invoke();
            }
            catch (Exception e)
            {
                var cshe = new UnableToStartHostException($"An error occured when trying to start the {GetType().FullName}", e);
                Logger.Error(cshe);
                if (EnableThrowingExceptions) throw cshe;
            }
        }

        public void Stop()
        {
            if (!IsListening) return;
            try
            {
                OnBeforeStop?.Invoke();

                IsStopping = true;
                StopEvent.Set();
                Listening.Join();
                foreach (var worker in Workers) worker.Join();
                Listener.Stop();

                if (!IsListening) OnAfterStop?.Invoke();
            }
            catch (Exception e)
            {
                var cshe = new UnableToStopHostException($"An error occured while trying to stop {GetType().FullName}", e);
                Logger.Error(cshe);
                if (EnableThrowingExceptions) throw cshe;
            }
        }

        public void Dispose()
        {
            Stop();
            Listener.Close();
        }

        public IRestServer LogToConsole()
        {
            Logger = new ConsoleLogger();
            return this;
        }

        /// <summary>
        /// For use in routes that want to stop the server; starts a new thread and then calls Stop on the server
        /// </summary>
        public void ThreadSafeStop()
        {
            new Thread(Stop).Start();
        }

        private void HandleRequests()
        {
            while (Listener.IsListening)
            {
                var context = Listener.BeginGetContext(ContextReady, null);
                if (0 == WaitHandle.WaitAny(new[] { StopEvent, context.AsyncWaitHandle })) return;
            }
        }

        private void ContextReady(IAsyncResult result)
        {
            try
            {
                lock (Queue)
                {
                    Queue.Enqueue(Listener.EndGetContext(result));
                    ReadyEvent.Set();
                }
            }
            catch (ObjectDisposedException) { /* Intentionally not doing anything with this */ }
            catch (Exception e)
            {
                /* Ignore exceptions thrown by incomplete async methods listening for incoming requests */
                if (IsStopping && e is HttpListenerException && ((HttpListenerException)e).NativeErrorCode == 995) return;
                Logger.Debug(e);
            }
        }

        private void Worker()
        {
            WaitHandle[] wait = { ReadyEvent, StopEvent };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpContext context;

                lock (Queue)
                {
                    if (Queue.Count > 0)
                    {
                        HttpListenerContext ctx;
                        Queue.TryDequeue(out ctx);
                        if (ctx == null) continue;
                        context = new HttpContext(ctx, this);
                    }
                    else { ReadyEvent.Reset(); continue; }
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(PublicFolder.Prefix) && context.Request.PathInfo.StartsWith(PublicFolder.Prefix))
                    {
                        PublicFolder.SendPublicFile(context);
                        if (!context.WasRespondedTo) context.Response.SendResponse(HttpStatusCode.NotFound);
                        return;
                    }

                    if (!Router.Route(PublicFolder.SendPublicFile(context))) throw new RouteNotFound(context);
                }
                catch (RouteNotFound)
                {
                    context.Response.SendResponse(HttpStatusCode.NotFound);
                }
                catch (NotImplementedException)
                {
                    context.Response.SendResponse(HttpStatusCode.NotImplemented);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    context.Response.SendResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpListener instance in use. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedRestServer
    {
        private readonly HttpListener _listener;

        internal AdvancedRestServer(HttpListener listener)
        {
            _listener = listener;
        }

        /// <summary>
        /// Gets or sets the delegate called to determine the protocol used to authenticate clients
        /// </summary>
        public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
        {
            get { return _listener.AuthenticationSchemeSelectorDelegate; }
            set { _listener.AuthenticationSchemeSelectorDelegate = value; }
        }

        /// <summary>
        /// Gets or sets the scheme used to authenticate clients
        /// </summary>
        public AuthenticationSchemes AuthenticationSchemes
        {
            get { return _listener.AuthenticationSchemes; }
            set { _listener.AuthenticationSchemes = value; }
        }

        /// <summary>
        /// Get or set the ExtendedProtectionPolicy to use for extended protection for a session
        /// </summary>
        public ExtendedProtectionPolicy ExtendedProtectionPolicy
        {
            get { return _listener.ExtendedProtectionPolicy; }
            set { _listener.ExtendedProtectionPolicy = value; }
        }

        /// <summary>
        /// Get or set the delegate called to determine the ExtendedProtectionPolicy to use for each request
        /// </summary>
        public ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
        {
            get { return _listener.ExtendedProtectionSelectorDelegate; }
            set { _listener.ExtendedProtectionSelectorDelegate = value; }
        }

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether your application receives exceptions that occur when an HttpListener sends the response to the client
        /// </summary>
        public bool IgnoreWriteExceptions
        {
            get { return _listener.IgnoreWriteExceptions; }
            set { _listener.IgnoreWriteExceptions = value; }
        }

        /// <summary>
        /// Gets or sets the realm, or resource partition, associated with this HttpListener object
        /// </summary>
        public string Realm
        {
            get { return _listener.Realm; }
            set { _listener.Realm = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether HttpListener can be used with the current operating system
        /// </summary>
        public bool IsSupported => HttpListener.IsSupported;

        /// <summary>
        /// Gets or sets a Boolean value that controls whether, when NTLM is used, additional requests using the same Transmission Control Protocol (TCP) connection are required to authenticate
        /// </summary>
        public bool UnsafeConnectionNtlmAuthentication
        {
            get { return _listener.UnsafeConnectionNtlmAuthentication; }
            set { _listener.UnsafeConnectionNtlmAuthentication = value; }
        }

        /// <summary>
        /// Shuts down the HttpListener object immediately, discarding all currently queued requests
        /// </summary>
        public void Abort()
        {
            _listener.Abort();
        }

        /// <summary>
        /// Shuts down the HttpListener
        /// </summary>
        public void Close()
        {
            _listener.Close();
        }

        /// <summary>
        /// Allows this instance to receive incoming requests
        /// </summary>
        public void Start()
        {
            _listener.Start();
        }

        /// <summary>
        /// Causes this instance to stop receiving incoming requests
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
        }
    }
}