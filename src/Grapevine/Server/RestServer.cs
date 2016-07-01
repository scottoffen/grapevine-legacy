using System;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using Grapevine.Util;
using HttpStatusCode = Grapevine.Util.HttpStatusCode;

namespace Grapevine.Server
{
    public interface IRestServer
    {
        /// <summary>
        /// Gets or sets the case insensative URI scheme (protocol) to be used when<br />
        /// creating the HttpListener prefix; e.g. "http" or "https"
        /// <para>&#160;</para>
        /// Note that if you create an HttpListener using https, you must select a<br />
        /// Server Certificate for the listener. See the MSDN documentation on the<br />
        /// HttpListener class for more information.<br />
        /// https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
        /// </summary>
        string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the host name used to create the HttpListener prefix, defaults<br />
        /// to localhost
        /// <para>&#160;</para>
        /// Use "*" to indicate that the HttpListener accepts requests sent to the port<br />
        /// if the requested URI does not match any other prefix. Similarly, to specify that<br />
        /// the HttpListener accepts all requests sent to a port, replace the host element with<br />
        /// the "+" character.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the port number (as a string) used to create the prefix used<br />
        /// by the HttpListener for incoming traffic
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Gets or sets the name of the default file to return when a directory is<br />
        /// requested without a file name; defaults to index.html
        /// </summary>
        string DirIndex { get; set; }

        /// <summary>
        /// Gets or sets the path to the top-level directory containing static files
        /// </summary>
        string WebRoot { get; set; }

        /// <summary>
        /// Gets or sets the number of HTTP connection threads maintained per processor;<br />
        /// defaults to 50
        /// </summary>
        int Connections { get; set; }

        /// <summary>
        /// Gets the prefix created by combining the Protocol, Host and Port properties<br />
        /// into a scheme and authority
        /// </summary>
        string Origin { get; }

        /// <summary>
        /// Gets a value that indicates whether HttpListener has been started
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Gets or sets the instance of IRouter to be used by this server to route<br />
        /// incoming HTTP requests
        /// </summary>
        IRouter Router { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server<br />
        /// start; synonym for OnAfterStart
        /// </summary>
        Action OnStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to start<br />
        /// the server
        /// </summary>
        Action OnBeforeStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server<br />
        /// start
        /// </summary>
        Action OnAfterStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server<br />
        /// stop; synonym for OnAfterStop
        /// </summary>
        Action OnStop { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to stop<br />
        /// the server
        /// </summary>
        Action OnBeforeStop { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server<br />
        /// stops
        /// </summary>
        Action OnAfterStop { get; set; }

        /// <summary>
        /// Starts the server: executes OnBeforeStart, starts the HttpListener, then<br />
        /// executes OnAfterStart if the HttpListener is listening
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server; executes OnBeforeStop, stops the HttpListener, then<br />
        /// executes OnAfterStop is the HttpListener is not listening
        /// </summary>
        void Stop();
    }

    public class RestServer : DynamicAspect, IRestServer, IDisposable
    {
        public Action OnBeforeStart { get; set; }
        public Action OnAfterStart { get; set; }
        public Action OnBeforeStop { get; set; }
        public Action OnAfterStop { get; set; }
        public IRouter Router { get; set; }
        public bool IsListening => _listener?.IsListening ?? false;

        /// <summary>
        /// Returns true if EnableThrowingExceptions method has been called
        /// </summary>
        public bool ThrowErrors { get; private set; }

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal<br />
        /// HttpListener instance in use; do not used unless you are fully aware of what<br />
        /// you are doing and the consequences involved
        /// </summary>
        public AdvancedRestServer Advanced { get; }

        private string _host;
        private string _port;
        private string _protocol;
        private int _connections;
        private readonly ContentRoot _contentRoot;
        private readonly HttpListener _listener;
        private readonly Thread _listening;
        private readonly ConcurrentQueue<HttpListenerContext> _queue;
        private readonly ManualResetEvent _ready, _stop;
        private Thread[] _workers;

        public RestServer() : this(new ServerOptions()) { }

        public RestServer(ServerOptions options)
        {
            _contentRoot = new ContentRoot();
            _listener = new HttpListener();
            _listening = new Thread(HandleRequests);
            _queue = new ConcurrentQueue<HttpListenerContext>();
            _ready = new ManualResetEvent(false);
            _stop = new ManualResetEvent(false);

            Host = options.Host;
            Port = options.Port;
            Protocol = options.Protocol;
            DirIndex = options.DirIndex;
            WebRoot = options.WebRoot;
            Connections = options.Connections;

            Router = options.Router;

            OnBeforeStart = options.OnBeforeStart;
            OnAfterStart = options.OnAfterStop;
            OnBeforeStop = options.OnBeforeStop;
            OnAfterStop = options.OnAfterStop;

            Advanced = new AdvancedRestServer(_listener);
            _listener.IgnoreWriteExceptions = true;
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

        public string Protocol
        {
            get { return _protocol; }
            set { if (IsListening) throw new ServerStateException(); _protocol = value.ToLower(); }
        }

        public string Host
        {
            get { return _host; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _host = (value == "0.0.0.0") ? "+" : value.ToLower();
            }
        }

        public string Port
        {
            get { return _port; }
            set { if (IsListening) throw new ServerStateException(); _port = value; }
        }

        public string Origin => $"{Protocol}://{Host}:{Port}/";

        public string WebRoot
        {
            get { return _contentRoot.Folder; }
            set { _contentRoot.Folder = value; }
        }

        public string DirIndex
        {
            get { return _contentRoot.DefaultFileName; }
            set { _contentRoot.DefaultFileName = value; }
        }

        public int Connections
        {
            get { return _connections; }
            set { if (IsListening) throw new ServerStateException(); _connections = value; }
        }

        public void Start()
        {
            if (IsListening) return;
            try
            {
                if (Router.RoutingTable.Count == 0) Router.RegisterAssembly();

                OnBeforeStart?.Invoke();

                _listener.Prefixes.Add(Origin);
                _listener.Start();
                _listening.Start();

                _workers = new Thread[_connections*Environment.ProcessorCount];
                for (var i = 0; i < _workers.Length; i++)
                {
                    _workers[i] = new Thread(Worker);
                    _workers[i].Start();
                }

                if (IsListening) OnAfterStart?.Invoke();
            }
            catch (Exception e)
            {
                throw new CantStartHostException($"An error occured when trying to start the {GetType().FullName}", e);
            }
        }

        public Action OnStart
        {
            get { return OnAfterStart; }
            set { OnAfterStart = value; }
        }

        public void Stop()
        {
            if (!IsListening) return;
            try
            {
                OnBeforeStop?.Invoke();

                _stop.Set();
                _listening.Join();
                foreach (Thread worker in _workers) worker.Join();
                _listener.Stop();

                if (!IsListening) OnAfterStop?.Invoke();
            }
            catch (Exception e)
            {
                throw new CantStartHostException($"An error occured while trying to stop {GetType().FullName}", e);
            }
        }

        public Action OnStop
        {
            get { return OnAfterStop; }
            set { OnAfterStop = value; }
        }

        public void Dispose() { Stop(); }

        /// <summary>
        /// Not entirely sure what this is going to do yet, but it will involve the new logger
        /// </summary>
        public RestServer EnableThrowingExceptions()
        {
            ThrowErrors = true;
            return this;
        }

        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                var context = _listener.BeginGetContext(ContextReady, null);
                if (0 == WaitHandle.WaitAny(new[] { _stop, context.AsyncWaitHandle })) return;
            }
        }

        private void ContextReady(IAsyncResult result)
        {
            try
            {
                lock (_queue)
                {
                    _queue.Enqueue(_listener.EndGetContext(result));
                    _ready.Set();
                }
            }
            catch { /* ignored */ }
        }

        private void Worker()
        {
            WaitHandle[] wait = { _ready, _stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpContext context;

                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        HttpListenerContext ctx;
                        _queue.TryDequeue(out ctx);
                        if (ctx == null) continue;
                        context = new HttpContext(ctx, this);
                    }
                    else { _ready.Reset(); continue; }
                }

                try
                {
                    if (!Router.Route(_contentRoot.ReturnFile(context))) throw new RouteNotFound(context);
                }
                catch (RouteNotFound e)
                {
                    context.Response.SendResponse(HttpStatusCode.NotFound);
                }
                catch (NotImplementedException)
                {
                    context.Response.SendResponse(HttpStatusCode.NotImplemented);
                }
                catch (Exception e)
                {
                    context.Response.SendResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }
    }

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
        public HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate
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
