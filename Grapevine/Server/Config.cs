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
        /// Host name to listen on; defaults to localhost. Set to "+" to listen on all interfaces.
        /// </summary>
        public string Host { get; set; }
        
        /// <summary>
        /// Port number (as a string) (don't ask me why) to listen on; defaults to 1234
        /// </summary>
        public string Port { get; set; }
        
        /// <summary>
        /// The root directory to serve files from; no default value. If null, will "webroot".
        /// </summary>
        public string WebRoot { get; set; }
        
        /// <summary>
        /// Default filename to look for if a directory is specified, but not a filename; defaults to index.html
        /// </summary>
        public string DirIndex { get; set; }
        
        /// <summary>
        /// Number of threads to use to respond to incoming requests; defaults to 5
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        /// Should the program automatically scan for and load RESTResource objects when
        /// it starts? Default is true for backward compatibility.
        /// 
        /// If false, use RESTServer.AddRoute() to add routes manually.
        /// </summary>
        public bool AutoLoadRestResources { get; set; }

        public Config()
        {
            this.Protocol = "http";
            this.Host = "localhost";
            this.Port = "1234";
            this.DirIndex = "index.html";
            this.MaxThreads = 5;
            this.AutoLoadRestResources = true;
        }
    }
}
