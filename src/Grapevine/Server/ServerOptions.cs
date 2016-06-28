using System;
using System.IO;
using System.Reflection;

namespace Grapevine.Server
{
    public class ServerOptions
    {
        /// <summary>
        /// Synonym for OnAfterStart
        /// </summary>
        public Action OnStart
        {
            get { return OnAfterStart; }
            set { OnAfterStart = value; }
        }

        /// <summary>
        /// Action to be taken just prior to the server starting
        /// </summary>
        public Action OnBeforeStart { get; set; }

        /// <summary>
        /// Action to be take immediately following the server starting
        /// </summary>
        public Action OnAfterStart { get; set; }

        /// <summary>
        /// Synonym for OnAfterStop
        /// </summary>
        public Action OnStop
        {
            get { return OnAfterStop; }
            set { OnAfterStop = value; }
        }

        /// <summary>
        /// Action to be taken just prior to the server stopping
        /// </summary>
        public Action OnBeforeStop { get; set; }

        /// <summary>
        /// Action to be take immediately following the server stopping
        /// </summary>
        public Action OnAfterStop { get; set; }

        /// <summary>
        /// Instance of IRouter to use to route traffic
        /// </summary>
        public IRouter Router { get; set; }

        /// <summary>
        /// Host name to listen on; defaults to localhost
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port number (as a string) to listen on; defaults to 1234
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Protocol to listen on; defaults to http
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Default filename to look for if a directory is specified, but not a filename; defaults to index.html
        /// </summary>
        public string DirIndex { get; set; }

        /// <summary>
        /// The root directory to serve files from; defaults to "webroot" in the same directory as the calling assembly
        /// </summary>
        public string WebRoot { get; set; }

        /// <summary>
        /// The number of connections maintained, defaults to 50
        /// </summary>
        public int Connections { get; set; }

        public ServerOptions()
        {
            Router = new Router();
            Host = "localhost";
            Port = "1234";
            Protocol = "http";
            DirIndex = "index.html";
            Connections = 50;

            var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (path != null) WebRoot = Path.Combine(path, "webroot");
        }
    }
}
