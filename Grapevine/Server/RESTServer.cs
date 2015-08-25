using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Grapevine.Server
{
    /// <summary>
    /// Delegate for pre and post starting and stopping of RESTServer
    /// </summary>
    public delegate void ToggleServerHandler();

    public class RESTServer : Responder, IDisposable
    {
        #region Instance Variables

        private readonly RouteCache _routeCache;

        private readonly Thread _listenerThread;
        private readonly Thread[] _workers;

        private readonly HttpListener _listener = new HttpListener();
        private readonly ManualResetEvent _stop = new ManualResetEvent(false);
        private readonly ManualResetEvent _ready = new ManualResetEvent(false);
        private Queue<HttpListenerContext> _queue = new Queue<HttpListenerContext>();

        #endregion

        #region Constructors

		public RESTServer(string host = "localhost", string port = "1234", string protocol = "http", string dirindex = "index.html", string webroot = null, int maxthreads = 5, object tag = null)
        {
            this.IsListening = false;
            this.DirIndex = dirindex;

            this.Host = host;
            this.Port = port;
            this.Protocol = protocol;
            this.MaxThreads = maxthreads;
			this.Tag = tag;

            this.WebRoot = webroot;
            if (object.ReferenceEquals(this.WebRoot, null))
            {
                this.WebRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "webroot");
            }

            this._routeCache = new RouteCache(this, this.BaseUrl);

            this._workers = new Thread[this.MaxThreads];
            this._listenerThread = new Thread(this.HandleRequests);
        }

		public RESTServer(Config config, object tag = null) : this(host: config.Host, port: config.Port, protocol: config.Protocol, dirindex: config.DirIndex, webroot: config.WebRoot, maxthreads: config.MaxThreads, tag: tag) { }

        private bool VerifyWebRoot(string webroot)
        {
            if (!Object.ReferenceEquals(webroot, null))
            {
                try
                {
                    if (!Directory.Exists(webroot))
                    {
                        Directory.CreateDirectory(webroot);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    EventLogger.Log(e);
                }
            }
            return false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Delegate used to execute custom code before attempting server start
        /// </summary>
        public ToggleServerHandler OnBeforeStart { get; set; }

        /// <summary>
        /// Delegate used to execute custom code after successful server start
        /// </summary>
        public ToggleServerHandler OnAfterStart { get; set; }

        /// <summary>
        /// Delegate used to execute custom code before attempting server stop
        /// </summary>
        public ToggleServerHandler OnBeforeStop { get; set; }

        /// <summary>
        /// Delegate used to execute custom code after successful server stop
        /// </summary>
        public ToggleServerHandler OnAfterStop { get; set; }

        /// <summary>
        /// Delegate used to execute custom code after successful server start; synonym for OnAfterStart
        /// </summary>
        public ToggleServerHandler OnStart
        {
            get
            {
                return this.OnAfterStart;
            }
            set
            {
                this.OnAfterStart = value;
            }
        }

        /// <summary>
        /// Delegate used to execute custom code after successful server stop; synonym for OnAfterStop
        /// </summary>
        public ToggleServerHandler OnStop
        {
            get
            {
                return this.OnAfterStop;
            }
            set
            {
                this.OnAfterStop = value;
            }
        }

        /// <summary>
        /// Returns true if the server is currently listening for incoming traffic
        /// </summary>
        public bool IsListening { get; private set; }

        /// <summary>
        /// Default file to return when a directory is requested without a file name
        /// </summary>
        public string DirIndex { get; set; }

        /// <summary>
        /// Specifies the top-level directory containing website content; value will not be set unless the directory exists or can be created
        /// </summary>
        public string WebRoot
        {
            get
            {
                return this._webroot;
            }
            set
            {
                if (VerifyWebRoot(value))
                {
                    this._webroot = value;
                }
            }
        }
        private string _webroot;

        /// <summary>
        /// <para>The host name used to create the HttpListener prefix</para>
        /// <para>&#160;</para>
        /// <para>Use "*" to indicate that the HttpListener accepts requests sent to the port if the requested URI does not match any other prefix. Similarly, to specify that the HttpListener accepts all requests sent to a port, replace the host element with the "+" character.</para>
        /// </summary>
        public string Host
        {
            get
            {
                return this._host;
            }
            set
            {
                if (!this.IsListening)
                {
                    this._host = value;
                }
                else
                {
                    var e = new ServerStateException("Attempted to modify Host property after server start.");
                    EventLogger.Log(e);
                    throw e;
                }
            }
        }
        private string _host;

        /// <summary>
        /// The port number (as a string) used to create the HttpListener prefix
        /// </summary>
        public string Port
        {
            get
            {
                return this._port;
            }
            set
            {
                if (!this.IsListening)
                {
                    this._port = value;
                }
                else
                {
                    var e = new ServerStateException("Attempted to modify Port property after server start.");
                    EventLogger.Log(e);
                    throw e;
                }
            }
        }
        private string _port;

        /// <summary>
        /// Returns the prefix created by combining the Protocol, Host and Port properties
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return String.Format("{0}://{1}:{2}/", this.Protocol, this.Host, this.Port);
            }
        }

        /// <summary>
        /// The number of threads that will be started to respond to queued requests
        /// </summary>
        public int MaxThreads
        {
            get
            {
                return this._maxt;
            }
            set
            {
                if (!this.IsListening)
                {
                    if (value >= 1)
                    {
                        this._maxt = value;
                    }
                    else
                    {
                        var e = new ServerStateException("MaxThreads cannot be set to a value less than 1");
                        EventLogger.Log(e);
                        throw e;
                    }
                }
                else
                {
                    var e = new ServerStateException("Attempted to modify MaxThreads property after server start.");
                    EventLogger.Log(e);
                    throw e;
                }
            }
        }
        private int _maxt;

        /// <summary>
        /// <para>The URI scheme (or protocol) to be used in creating the HttpListener prefix; ex. "http" or "https"</para>
        /// <para>&#160;</para>
        /// <para>Note that if you create an HttpListener using https, you must select a Server Certificate for that listener. See the MSDN documentation on the HttpListener class for more information.</para>
        /// </summary>
        public string Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                if (!this.IsListening)
                {
                    this._protocol = value;
                }
                else
                {
                    var e = new ServerStateException("Attempted to modify Protocol property after server start.");
                    EventLogger.Log(e);
                    throw e;
                }
            }
        }
        private string _protocol;

		/// <summary>
		/// Arbitary object to tag the server with.
		/// </summary>
		/// <value>The tag.</value>
		public object Tag {
			get;
			set;
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Attempts to start the server; executes delegates for OnBeforeStart and OnAfterStart
        /// </summary>
        public void Start()
        {
            if (!this.IsListening)
            {
                try
                {
                    this.FireDelegate(this.OnBeforeStart);

                    this.IsListening = true;
                    this._listener.Prefixes.Add(this.BaseUrl);

                    this._listener.Start();
                    this._listenerThread.Start();

                    for (int i = 0; i < _workers.Length; i++)
                    {
                        _workers[i] = new Thread(Worker);
                        _workers[i].Start();
                    }

                    this.FireDelegate(this.OnAfterStart);
                }
                catch (Exception e)
                {
                    this.IsListening = false;
                    EventLogger.Log(e);
                }
            }
        }

        /// <summary>
        /// Attempts to stop the server; executes delegates for OnBeforeStop and OnAfterStop
        /// </summary>
        public void Stop()
        {
            try
            {
                this.FireDelegate(this.OnBeforeStop);

                this._stop.Set();
                if (!object.ReferenceEquals(this._workers, null))
                {
                    foreach (Thread worker in this._workers)
                    {
                        worker.Join();
                    }
                }
                this._listener.Stop();
                this.IsListening = false;

                this.FireDelegate(this.OnAfterStop);
            }
            catch (Exception e)
            {
                EventLogger.Log(e);
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface; explicitly calls the Stop() method
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }

        #endregion

        #region Private Threading Methods

        private void HandleRequests()
        {
            while (this.IsListening)
            {
                try
                {
                    var context = this._listener.GetContext();
                    this.QueueRequest(context);
                }
                catch (Exception e)
                {
                    EventLogger.Log(e);
                }
            }
        }

        private void QueueRequest(HttpListenerContext context)
        {
            try
            {
                lock (this._queue)
                {
                    this._queue.Enqueue(context);
                    this._ready.Set();
                }
            }
            catch (Exception e)
            {
                EventLogger.Log(e);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var notfound = true;
                if (this._routeCache.FindAndInvokeRoute(context))
                {
                    notfound = false;
                }
                else if ((context.Request.HttpMethod.ToUpper().Equals("GET")) && (!object.ReferenceEquals(this.WebRoot, null)))
                {
                    var filename = this.GetFilePath(context.Request.RawUrl);
                    if (!object.ReferenceEquals(filename, null))
                    {
                        this.SendFileResponse(context, filename);
                        notfound = false;
                    }
                }

                if (notfound)
                {
                    this.NotFound(context);
                }
            }
            catch (Exception e)
            {
                try
                {
                    EventLogger.Log(e);
                    this.InternalServerError(context, e);
                }
                catch (Exception) // We're really in trouble?
                {
                    context.Response.StatusCode = 500; // make sure code is at least error.
                }
            }
            finally
            {
                context.Response.OutputStream.Close();
                context.Response.Close(); // paranoia
            }
        }

        private void Worker()
        {
            WaitHandle[] wait = new[] { this._ready, this._stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                try
                {
                    HttpListenerContext context;
                    lock (this._queue)
                    {
                        if (this._queue.Count > 0)
                        {
                            context = this._queue.Dequeue();
                        }
                        else
                        {
                            this._ready.Reset();
                            continue;
                        }
                    }

                    this.ProcessRequest(context);
                }
                catch (Exception e)
                {
                    EventLogger.Log(e);
                }
            }
        }

        #endregion

        #region Private Utility Methods

        private string GetFilePath(string rawurl)
        {
            var filename = ((rawurl.IndexOf("?") > -1) ? rawurl.Split('?') : rawurl.Split('#'))[0].Replace('/', Path.DirectorySeparatorChar).Substring(1);
            var path = Path.Combine(this.WebRoot, filename);

            if (File.Exists(path))
            {
                return path;
            }
            else if (Directory.Exists(path))
            {
                path = Path.Combine(path, this.DirIndex);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        private void FireDelegate(ToggleServerHandler method)
        {
            if (!object.ReferenceEquals(method, null))
            {
                try
                {
                    method();
                }
                catch (Exception e)
                {
                    EventLogger.Log(e);
                }
            }
        }

        #endregion
    }
}