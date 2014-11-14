namespace Grapevine.Server
{
    /// <summary>
    /// A serializable configuration object for configuring a RESTServer.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Protocol to listen on; defaults to http
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Host name to listen on; defaults to localhost
        /// </summary>
        public string Host { get; set; }
        
        /// <summary>
        /// Port number (as a string) to listen on; defaults to 1234
        /// </summary>
        public string Port { get; set; }
        
        /// <summary>
        /// The root directory to serve files from; no default value
        /// </summary>
        public string WebRoot { get; set; }
        
        /// <summary>
        /// Default filename to look for if a directory is specified, but not a fielname; defaults to index.html
        /// </summary>
        public string DirIndex { get; set; }
        
        /// <summary>
        /// Number of threads to use to respond to incoming requests; defaults to 5
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        /// Used to configure the EventLogger Exception logging
        /// </summary>
        public bool LogExceptions { get; set; }

        public Config()
        {
            this.Protocol = "http";
            this.Host = "localhost";
            this.Port = "1234";
            this.DirIndex = "index.html";
            this.MaxThreads = 5;
            this.LogExceptions = false;
        }
    }
}
