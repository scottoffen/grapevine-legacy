using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;
using ExtendedProtectionSelector = System.Net.HttpListener.ExtendedProtectionSelector;
using HttpListener = Grapevine.Interfaces.Server.HttpListener;

namespace Grapevine.Server
{
    /// <summary>
    /// Delegate for the <see cref="IRestServer.BeforeStarting"/>, <see cref="IRestServer.AfterStarting"/>, <see cref="IRestServer.BeforeStopping"/> and <see cref="IRestServer.AfterStopping"/> events
    /// </summary>
    /// <param name="server"></param>
    public delegate void ServerEventHandler(RestServer server);

    /// <summary>
    /// Provides a programmatically controlled REST implementation for a single Prefix using HttpListener
    /// </summary>
    public interface IRestServer : IServerSettings, IDynamicProperties, IDisposable
    {
        /// <summary>
        /// Gets a value that indicates whether HttpListener has been started
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Gets the prefix created by combining the Protocol, Host and Port properties into a scheme and authority
        /// </summary>
        string ListenerPrefix { get; }

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
        private readonly UriBuilder _uriBuilder = new UriBuilder("http", "localhost", 1234, "/");

        private IGrapevineLogger _logger;
        protected bool IsStopping;
        protected bool IsStarting;
        protected readonly IHttpListener Listener;
        protected readonly Thread Listening;
        protected readonly ManualResetEvent StopEvent;

        protected internal bool TestingMode;

        public event ServerEventHandler AfterStarting;
        public event ServerEventHandler AfterStopping;
        public event ServerEventHandler BeforeStarting;
        public event ServerEventHandler BeforeStopping;

        public bool EnableThrowingExceptions { get; set; }
        public Action OnBeforeStart { get; set; }
        public Action OnAfterStart { get; set; }
        public Action OnBeforeStop { get; set; }
        public Action OnAfterStop { get; set; }
        public IList<IPublicFolder> PublicFolders { get; }
        public IRouter Router { get; set; }

        public RestServer() : this(new ServerSettings()) { }

        protected internal RestServer(IHttpListener listener) : this(new ServerSettings())
        {
            TestingMode = true;
            Listener = listener;
        }

        public RestServer(IServerSettings options)
        {
            Listener = new HttpListener(new System.Net.HttpListener());
            Listening = new Thread(HandleRequests);
            StopEvent = new ManualResetEvent(false);

            options.CloneEventHandlers(this);
            Host = options.Host;
            Logger = options.Logger;
            Port = options.Port;
            PublicFolders = options.PublicFolders;
            Router = options.Router;
            UseHttps = options.UseHttps;

            /* Obsolete */
            Connections = options.Connections;
            OnBeforeStart = options.OnBeforeStart;
            OnAfterStart = options.OnAfterStart;
            OnBeforeStop = options.OnBeforeStop;
            OnAfterStop = options.OnAfterStop;

            Advanced = new AdvancedRestServer(Listener);
            Listener.IgnoreWriteExceptions = true;
        }

        public static RestServer For(Action<ServerSettings> configure)
        {
            var options = new ServerSettings();
            configure(options);
            return new RestServer(options);
        }

        public static RestServer For<T>() where T : ServerSettings, new()
        {
            return new RestServer(new T());
        }

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpListener instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedRestServer Advanced { get; }

        public int Connections { get; set; }

        public string Host
        {
            get { return _uriBuilder.Host; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _uriBuilder.Host = value == "0.0.0.0" ? "+" : value.ToLower();
            }
        }

        public bool IsListening => Listener?.IsListening ?? false;

        public IGrapevineLogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value ?? NullLogger.GetInstance();
                if (Router != null) Router.Logger = _logger;
            }
        }

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

        public string ListenerPrefix => _uriBuilder.ToString();

        public string Port
        {
            get { return _uriBuilder.Port.ToString(); }
            set
            {
                if (IsListening) throw new ServerStateException();
                _uriBuilder.Port = int.Parse(value);
            }
        }

        public IPublicFolder PublicFolder
        {
            get
            {
                if (!PublicFolders.Any()) PublicFolders.Add(new PublicFolder());
                return PublicFolders.First();
            }
            set
            {
                if (value == null) return;
                if (PublicFolders.Any())
                {
                    PublicFolders[0] = value;
                    return;
                }
                PublicFolders.Add(value);
            }
        }

        public bool UseHttps
        {
            get { return _uriBuilder.Scheme == "https"; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _uriBuilder.Scheme = value ? "https" : "http";
            }
        }

        public void Start()
        {
            if (IsListening || IsStarting) return;
            if (IsStopping) throw new UnableToStartHostException("Cannot start server until server has finished stopping");
            IsStarting = true;

            try
            {
                OnBeforeStarting();
                if (Router.RoutingTable.Count == 0) Router.ScanAssemblies();

                Listener.Prefixes?.Clear();
                Listener.Prefixes?.Add(ListenerPrefix);
                Listener.Start();

                if (!TestingMode) Listening.Start();

                Logger.Trace($"Listening: {ListenerPrefix}");
                if (IsListening) OnAfterStarting();
            }
            catch (Exception e)
            {
                throw new UnableToStartHostException($"An error occured when trying to start the {GetType().FullName}", e);
            }
            finally
            {
                IsStarting = false;
            }
        }

        public void Stop()
        {
            if (!IsListening || IsStopping) return;
            if (IsStarting) throw new UnableToStopHostException("Cannot stop server until server has finished starting");
            IsStopping = true;

            try
            {
                OnBeforeStopping();

                StopEvent.Set();
                if (!TestingMode) Listening.Join();
                Listener.Stop();

                if (!IsListening) OnAfterStopping();
            }
            catch (Exception e)
            {
                throw new UnableToStopHostException($"An error occured while trying to stop {GetType().FullName}", e);
            }
            finally
            {
                IsStopping = false;
            }
        }

        public void Dispose()
        {
            if (IsListening) Stop();
            Listener?.Close();
        }

        public void CloneEventHandlers(IRestServer server)
        {
            if (BeforeStarting != null)
            {
                foreach (var action in BeforeStarting.GetInvocationList().Reverse().Cast<ServerEventHandler>())
                {
                    server.BeforeStarting += action;
                }
            }

            if (AfterStarting != null)
            {
                foreach (var action in AfterStarting.GetInvocationList().Reverse().Cast<ServerEventHandler>())
                {
                    server.AfterStarting += action;
                }
            }

            if (BeforeStopping != null)
            {
                foreach (var action in BeforeStopping.GetInvocationList().Reverse().Cast<ServerEventHandler>())
                {
                    server.BeforeStopping += action;
                }
            }

            if (AfterStopping != null)
            {
                foreach (var action in AfterStopping.GetInvocationList().Reverse().Cast<ServerEventHandler>())
                {
                    server.AfterStopping += action;
                }
            }
        }

        public IRestServer LogToConsole()
        {
            Logger = new ConsoleLogger();
            return this;
        }

        private List<Exception> InvokeServerEventHandlers(IEnumerable<ServerEventHandler> actions)
        {
            var exceptions = new List<Exception>();

            foreach (var action in actions)
            {
                try
                {
                    action.Invoke(this);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            return exceptions;
        }

        protected void OnBeforeStarting()
        {
            OnBeforeStart?.Invoke();
            if (BeforeStarting == null) return;
            var exceptions = InvokeServerEventHandlers(BeforeStarting.GetInvocationList().Reverse().Cast<ServerEventHandler>());
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

        protected void OnAfterStarting()
        {
            OnAfterStart?.Invoke();
            if (AfterStarting == null) return;
            var exceptions = InvokeServerEventHandlers(AfterStarting.GetInvocationList().Reverse().Cast<ServerEventHandler>());
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

        protected void OnBeforeStopping()
        {
            OnBeforeStop?.Invoke();
            if (BeforeStopping == null) return;
            var exceptions = InvokeServerEventHandlers(BeforeStopping.GetInvocationList().Reverse().Cast<ServerEventHandler>());
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

        protected void OnAfterStopping()
        {
            OnAfterStop?.Invoke();
            if (AfterStopping == null) return;
            var exceptions = InvokeServerEventHandlers(AfterStopping.GetInvocationList().Reverse().Cast<ServerEventHandler>());
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
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
                var context = new HttpContext(Listener.EndGetContext(result), this);
                ThreadPool.QueueUserWorkItem(Router.Route, context);
            }
            catch (ObjectDisposedException) { /* Intentionally not doing anything with this */ }
            catch (Exception e)
            {
                /* Ignore exceptions thrown by incomplete async methods listening for incoming requests */
                if (IsStopping && e is HttpListenerException && ((HttpListenerException)e).NativeErrorCode == 995) return;
                Logger.Debug(e);
            }
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpListener instance in use. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedRestServer
    {
        private readonly IHttpListener _listener;

        internal AdvancedRestServer(IHttpListener listener)
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
        public bool IsSupported => System.Net.HttpListener.IsSupported;

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

    public static class RestServerExtensions
    {
        /// <summary>
        /// For use in routes that want to stop the server; starts a new thread and then calls Stop on the server
        /// </summary>
        public static void ThreadSafeStop(this IRestServer server)
        {
            new Thread(server.Stop).Start();
        }
    }
}