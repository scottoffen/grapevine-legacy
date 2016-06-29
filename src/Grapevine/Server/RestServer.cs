using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Grapevine.Util;
using HttpStatusCode = Grapevine.Util.HttpStatusCode;

namespace Grapevine.Server
{
    public interface IRestServer
    {
        /// <summary>
        /// <para>The URI scheme (protocol) to be used in creating the HttpListener prefix; ex. "http" or "https"</para>
        /// <para>&#160;</para>
        /// <para>Note that if you create an HttpListener using https, you must select a Server Certificate for that listener. See the MSDN documentation on the HttpListener class for more information.</para>
        /// </summary>
        string Protocol { get; set; }

        /// <summary>
        /// <para>The host name used to create the HttpListener prefix</para>
        /// <para>&#160;</para>
        /// <para>Use "*" to indicate that the HttpListener accepts requests sent to the port if the requested URI does not match any other prefix. Similarly, to specify that the HttpListener accepts all requests sent to a port, replace the host element with the "+" character.</para>
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// The port number (as a string) used to create the HttpListener prefix
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Default file to return when a directory is requested without a file name
        /// </summary>
        string DirIndex { get; set; }

        /// <summary>
        /// Specifies the top-level directory containing website content
        /// </summary>
        string WebRoot { get; set; }

        /// <summary>
        /// The number of http connections maintained, defaults to 50
        /// </summary>
        int Connections { get; set; }

        /// <summary>
        /// Returns the prefix created by combining the Protocol, Host and Port properties into a scheme and authority
        /// </summary>
        string Origin { get; }

        /// <summary>
        /// Returns true if the server is currently listening for incoming traffic
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Instance of IRouter to be used by this server to route incoming HTTP requests
        /// </summary>
        IRouter Router { get; set; }

        /// <summary>
        /// Synonym for OnAfterStart
        /// </summary>
        Action OnStart { get; set; }

        /// <summary>
        /// Action that will be executed before attempting server start
        /// </summary>
        Action OnBeforeStart { get; set; }

        /// <summary>
        /// Action that will be executed immediately following server start
        /// </summary>
        Action OnAfterStart { get; set; }

        /// <summary>
        /// Synonym for OnAfterStop
        /// </summary>
        Action OnStop { get; set; }

        /// <summary>
        /// Action that will be executed before attempting server stop
        /// </summary>
        Action OnBeforeStop { get; set; }

        /// <summary>
        /// Action that will be executed immediately following server stop
        /// </summary>
        Action OnAfterStop { get; set; }

        /// <summary>
        /// Start the server
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the server
        /// </summary>
        void Stop();
    }

    public class RestServer : IRestServer, IDisposable
    {
        /// <summary>
        /// Action that will be executed before attempting server start
        /// </summary>
        public Action OnBeforeStart { get; set; }

        /// <summary>
        /// Action that will be executed immediately following server start
        /// </summary>
        public Action OnAfterStart { get; set; }

        /// <summary>
        /// Action that will be executed before attempting server stop
        /// </summary>
        public Action OnBeforeStop { get; set; }

        /// <summary>
        /// Action that will be executed immediately following server stop
        /// </summary>
        public Action OnAfterStop { get; set; }

        /// <summary>
        /// Instance of IRouter to be used by this server to route incoming HTTP requests
        /// </summary>
        public IRouter Router { get; set; }

        /// <summary>
        /// Returns true if the server is currently listening for incoming traffic
        /// </summary>
        public bool IsListening => _listener?.IsListening ?? false;

        /// <summary>
        /// Is throw errors turned on
        /// </summary>
        public bool ThrowErrors { get; private set; }

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
        /// <para>The URI scheme (or protocol) to be used in creating the HttpListener prefix; ex. "http" or "https"</para>
        /// <para>&#160;</para>
        /// <para>Note that if you create an HttpListener using https, you must select a Server Certificate for that listener. See the MSDN documentation on the HttpListener class for more information.</para>
        /// <para>&#160;</para>
        /// <para>This value cannot be changed while the server is running.</para>
        /// </summary>
        public string Protocol
        {
            get { return _protocol; }
            set { if (IsListening) throw new ServerStateException(); _protocol = value.ToLower(); }
        }

        /// <summary>
        /// <para>The host name used to create the HttpListener prefix</para>
        /// <para>&#160;</para>
        /// <para>Use "*" to indicate that the HttpListener accepts requests sent to the port if the requested URI does not match any other prefix. Similarly, to specify that the HttpListener accepts all requests sent to a port, replace the host element with the "+" character.</para>
        /// <para>&#160;</para>
        /// <para>This value cannot be changed while the server is running.</para>
        /// </summary>
        public string Host
        {
            get { return _host; }
            set
            {
                if (IsListening) throw new ServerStateException();
                _host = (value == "0.0.0.0") ? "+" : value.ToLower();
            }
        }

        /// <summary>
        /// The port number (as a string) used to create the HttpListener prefix.
        /// <para>&#160;</para>
        /// <para>This value cannot be changed while the server is running.</para>
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { if (IsListening) throw new ServerStateException(); _port = value; }
        }

        /// <summary>
        /// Returns the prefix created by combining the Protocol, Host and Port properties into a scheme and authority
        /// </summary>
        public string Origin => $"{Protocol}://{Host}:{Port}/";

        /// <summary>
        /// Specifies the directory containing website content; if the directory doesn't exists, it will attempt to create it
        /// </summary>
        public string WebRoot
        {
            get { return _contentRoot.Folder; }
            set { _contentRoot.Folder = value; }
        }

        /// <summary>
        /// Default file to return when a directory is requested without a file name
        /// </summary>
        public string DirIndex
        {
            get { return _contentRoot.DefaultFileName; }
            set { _contentRoot.DefaultFileName = value; }
        }

        /// <summary>
        /// The number of connections maintained, defaults to 50
        /// </summary>
        public int Connections
        {
            get { return _connections; }
            set { if (IsListening) throw new ServerStateException(); _connections = value; }
        }

        /// <summary>
        /// Starts the server
        /// </summary>
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

        /// <summary>
        /// Synonym for OnAfterStart
        /// </summary>
        public Action OnStart
        {
            get { return OnAfterStart; }
            set { OnAfterStart = value; }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
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

        /// <summary>
        /// Synonym for OnAfterStop
        /// </summary>
        public Action OnStop
        {
            get { return OnAfterStop; }
            set { OnAfterStop = value; }
        }

        public void Dispose() { Stop(); }

        /// <summary>
        /// Turns on throwing errors
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
}
