using System;
using System.IO;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    public class ServerOptions
    {
        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server start; synonym for OnAfterStart
        /// </summary>
        public Action OnStart
        {
            get { return OnAfterStart; }
            set { OnAfterStart = value; }
        }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to start the server
        /// </summary>
        public Action OnBeforeStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server start
        /// </summary>
        public Action OnAfterStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server stop; synonym for OnAfterStop
        /// </summary>
        public Action OnStop
        {
            get { return OnAfterStop; }
            set { OnAfterStop = value; }
        }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to stop the server
        /// </summary>
        public Action OnBeforeStop { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server stops
        /// </summary>
        public Action OnAfterStop { get; set; }

        /// <summary>
        /// Gets or sets the instance of IRouter to be used by this server to route incoming HTTP requests
        /// </summary>
        public IRouter Router { get; set; }

        /// <summary>
        /// Gets or sets the host name used to create the HttpListener prefix, defaults to localhost
        /// <para>&#160;</para>
        /// Use "*" to indicate that the HttpListener accepts requests sent to the port if the requested URI does not match any other prefix. Similarly, to specify that the HttpListener accepts all requests sent to a port, replace the host element with the "+" character.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port number (as a string) used to create the prefix used by the HttpListener for incoming traffic
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sets the case insensitive URI scheme (protocol) to be used when creating the HttpListener prefix; e.g. "http" or "https"
        /// <para>&#160;</para>
        /// Note that if you create an HttpListener using https, you must select a Server Certificate for the listener. See the MSDN documentation on the HttpListener class for more information.<br />
        /// https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the name of the default file to return when a directory is requested without a file name; defaults to index.html
        /// </summary>
        public string DirIndex { get; set; }

        /// <summary>
        /// Gets or sets the path to the top-level directory containing static files
        /// </summary>
        public string WebRoot { get; set; }

        /// <summary>
        /// Gets or sets the number of HTTP connection threads maintained per processor; defaults to 50
        /// </summary>
        public int Connections { get; set; }

        public IGrapevineLogger Logger { get; set; }

        public ServerOptions()
        {
            Router = new Router();
            Host = "localhost";
            Port = "1234";
            Protocol = "http";
            DirIndex = "index.html";
            Connections = 50;
            Logger = new NullLogger();

            var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (path != null) WebRoot = Path.Combine(path, "webroot");
        }
    }
}
