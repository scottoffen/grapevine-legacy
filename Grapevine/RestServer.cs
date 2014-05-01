using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Grapevine
{
    public abstract class RestServer : IDisposable
    {
        #region Instance Variables

        private volatile bool _listening;

        private string _webroot = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\webroot";
        private string _default = "index.html";
        private string _host = "localhost";
        private string _port = "1234";
        private int _maxt = 5;

        private readonly Thread _listenerThread;
        private readonly Thread[] _workers;
        private readonly List<MethodInfo> _methods;

        private readonly HttpListener _listener = new HttpListener();
        private readonly ManualResetEvent _stop = new ManualResetEvent(false);
        private readonly ManualResetEvent _ready = new ManualResetEvent(false);
        private Queue<HttpListenerContext> _queue = new Queue<HttpListenerContext>();

        #endregion

        #region Constructors

        public RestServer()
        {
            _workers = new Thread[_maxt];
            _listenerThread = new Thread(HandleRequests);
            _methods = this.GetType().GetMethods().Where(mi => mi.GetCustomAttributes(true).Any(attr => attr is RestRoute)).ToList<MethodInfo>();
        }

        public RestServer(string host) : this(host, "1234", 5, null) { }

        public RestServer(string host, string port) : this(host, port, 5, null) { }

        public RestServer(string host, string port, string webroot) : this(host, port, 5, webroot) { }

        public RestServer(string host, string port, int maxThreads) : this(host, port, maxThreads, null) { }

        public RestServer(string host, string port, string webroot, int maxThreads) : this(host, port, maxThreads, webroot) { }

        public RestServer(string host, string port, int maxThreads, string webroot) : this()
        {
            this.Host = host;
            this.Port = port;
            this.MaxThreads = maxThreads;
            this.WebRoot = webroot;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            _listening = true;
            _listener.Prefixes.Add(String.Format(@"http://{0}:{1}/", _host, _port));
            _listener.Start();
            _listenerThread.Start();

            for (int i = 0; i < _workers.Length; i++)
            {
                _workers[i] = new Thread(Worker);
                _workers[i].Start();
            }
        }

        public void Stop()
        {
            _stop.Set();
            foreach (Thread worker in _workers)
                worker.Join();
            _listener.Stop();
            _listening = false;
        }

        public void Dispose()
        {
            Stop();
        }

        #endregion

        #region Public Properties

        public bool IsListening
        {
            get
            {
                return this._listening;
            }
            set
            {
                this._listening = false;
            }
        }

        public string WebRoot
        {
            get
            {
                return _webroot;
            }
            set
            {
                if (VerifyWebRoot(value))
                {
                    _webroot = value;
                }
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                if (!_listening)
                {
                    _host = value;
                }
                else
                {
                    throw new ServerStateException();
                }
            }
        }

        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (!_listening)
                {
                    _port = value;
                }
                else
                {
                    throw new ServerStateException();
                }
            }
        }

        public int MaxThreads
        {
            get
            {
                return _maxt;
            }
            set
            {
                if (!_listening)
                {
                    _maxt = value;
                }
                else
                {
                    throw new ServerStateException();
                }
            }
        }

        public string BaseUrl
        {
            get
            {
                return "http://" + this._host + ":" + this._port;
            }
        }

        #endregion

        #region Threading, Queuing and Processing

        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = _listener.GetContext();
                    QueueContext(context);
                }
                catch { }
            }
        }

        private void QueueContext(HttpListenerContext context)
        {
            try
            {
                lock (_queue)
                {
                    _queue.Enqueue(context);
                    _ready.Set();
                }
            }
            catch
            {
                return;
            }
        }

        private void Worker()
        {
            WaitHandle[] wait = new[] { _ready, _stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpListenerContext context;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                        context = _queue.Dequeue();
                    else
                    {
                        _ready.Reset();
                        continue;
                    }
                }

                try
                {
                    ProcessRequest(context);
                }
                catch { return; }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var method = _methods.Where(mi => mi.GetCustomAttributes(true).Any(attr => context.Request.RawUrl.Matches(((RestRoute)attr).PathInfo) && ((RestRoute)attr).Method.ToString().Equals(context.Request.HttpMethod.ToUpper()))).First();
                method.Invoke(this, new object[] { context });
            }
            catch
            {
                if ((context.Request.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase)) && (VerifyWebRoot(_webroot)))
                {
                    var filename = GetFilePath(context.Request.RawUrl);
                    if (!Object.ReferenceEquals(filename, null))
                    {
                        SendFileResponse(context, filename);
                        return;
                    }
                }

                NotFound(context);
            }
        }

        #endregion

        #region Responding Helpers

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
                catch { }
            }
            return false;
        }

        protected void NotFound(HttpListenerContext context)
        {
            var payload = "<h1>Not Found</h1>";
            var buffer = Encoding.UTF8.GetBytes(payload);
            var length = buffer.Length;

            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Not Found";
            context.Response.ContentLength64 = length;
            context.Response.OutputStream.Write(buffer, 0, length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        protected void SendTextResponse(HttpListenerContext context, Encoding encoding, string payload)
        {
            var buffer = encoding.GetBytes(payload);
            var length = buffer.Length;

            context.Response.ContentEncoding = encoding;
            context.Response.ContentLength64 = length;
            context.Response.OutputStream.Write(buffer, 0, length);
            context.Response.OutputStream.Close();

            context.Response.Close();
        }

        protected void SendTextResponse(HttpListenerContext context, string payload)
        {
            this.SendTextResponse(context, Encoding.UTF8, payload);
        }

        protected void SendFileResponse(HttpListenerContext context, string path)
        {
            var ext  = Path.GetExtension(path).ToUpper();
            var type = (Enum.IsDefined(typeof(ContentType), ext)) ? (ContentType)Enum.Parse(typeof(ContentType), ext) : ContentType.DEFAULT;
 
            var buffer = GetFileBytes(path, type.IsText());
            var length = buffer.Length;

            context.Response.ContentType = type.ToValue();
            context.Response.ContentLength64 = length;
            context.Response.OutputStream.Write(buffer, 0, length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        private string GetFilePath(string rawurl)
        {
            var filename = ((rawurl.IndexOf("?") > -1) ? rawurl.Split('?') : rawurl.Split('#'))[0].Replace('/', '\\').Substring(1);
            var path = Path.Combine(_webroot, filename);

            if (Directory.Exists(path))
            {
                path = Path.Combine(path, _default);
            }

            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        private byte[] GetFileBytes(string path, bool istext)
        {
            byte[] buffer;

            if (istext)
            {
                var reader = new StreamReader(path);
                buffer = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                reader.Close();
            }
            else
            {
                var finfo = new FileInfo(path);
                var reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
                buffer = reader.ReadBytes((int)finfo.Length);
                reader.Close();
            }

            return buffer;
        }

        #endregion
    }

    public static class StringExtensions
    {
        public static bool Matches(this String s, string pattern)
        {
            return ((Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))) ? true : false;
        }
    }
}
