using System;
using System.IO;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    public interface IServerProperties
    {
        /// <summary>
        /// Gets or sets the number of HTTP connection threads maintained per processor; defaults to 50
        /// </summary>
        int Connections { get; set; }

        /// <summary>
        /// Gets or sets the name of the default file to return when a directory is requested without a file name; defaults to index.html
        /// </summary>
        string DefaultPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that route exceptions should be rethrown instead of logged
        /// </summary>
        bool EnableThrowingExceptions { get; set; }

        /// <summary>
        /// Gets or sets the host name used to create the HttpListener prefix, defaults to localhost
        /// <para>&#160;</para>
        /// Use "*" to indicate that the HttpListener accepts requests sent to the port if the requested URI does not match any other prefix. Similarly, to specify that the HttpListener accepts all requests sent to a port, replace the host element with the "+" character.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the internal logger
        /// </summary>
        IGrapevineLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server start; synonym for OnAfterStart
        /// </summary>
        Action OnStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to start the server
        /// </summary>
        Action OnBeforeStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server start
        /// </summary>
        Action OnAfterStart { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server stop; synonym for OnAfterStop
        /// </summary>
        Action OnStop { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed before attempting to stop the server
        /// </summary>
        Action OnBeforeStop { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately following server stops
        /// </summary>
        Action OnAfterStop { get; set; }

        /// <summary>
        /// Gets or sets the port number (as a string) used to create the prefix used by the HttpListener for incoming traffic; defaults to 1234
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Gets or sets the case insensitive URI scheme (protocol) to be used when creating the HttpListener prefix; e.g. "http" or "https", defaults to http
        /// <para>&#160;</para>
        /// Note that if you create an HttpListener using https, you must select a Server Certificate for the listener. See the MSDN documentation on the HttpListener class for more information.<br />
        /// https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
        /// </summary>
        string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the path to the top-level directory containing static files; defaults to /public
        /// </summary>
        string PublicFolder { get; set; }

        /// <summary>
        /// Gets or sets the optional prefix for specifying static content should be returned
        /// </summary>
        string PublicFolderPrefix { get; set; }

        /// <summary>
        /// Gets or sets the instance of IRouter to be used by this server to route incoming HTTP requests
        /// </summary>
        IRouter Router { get; set; }
    }

    public class ServerOptions : IServerProperties
    {
        public int Connections { get; set; }
        public string DefaultPage { get; set; }
        public bool EnableThrowingExceptions { get; set; }
        public string Host { get; set; }
        public IGrapevineLogger Logger { get; set; }
        public Action OnBeforeStart { get; set; }
        public Action OnAfterStart { get; set; }
        public Action OnBeforeStop { get; set; }
        public Action OnAfterStop { get; set; }
        public string Port { get; set; }
        public string Protocol { get; set; }
        public string PublicFolder { get; set; }
        public string PublicFolderPrefix { get; set; }
        public IRouter Router { get; set; }

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

        public ServerOptions()
        {
            Connections = 50;
            DefaultPage = "index.html";
            Host = "localhost";
            Logger = new NullLogger();
            Port = "1234";
            Protocol = "http";
            PublicFolderPrefix = string.Empty;
            Router = new Router();

            var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (path != null) PublicFolder = Path.Combine(path, "public");
        }
    }
}
