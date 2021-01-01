using System.Collections.Generic;
using System.Linq;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared.Loggers;

namespace Grapevine.Server
{
    public interface IServerSettings
    {
        /// <summary>
        /// Raised after the server has finished starting
        /// </summary>
        event ServerEventHandler AfterStarting;

        /// <summary>
        /// Raised after the server has finished stopping
        /// </summary>
        event ServerEventHandler AfterStopping;

        /// <summary>
        /// Raised when the server starts
        /// </summary>
        event ServerEventHandler BeforeStarting;

        /// <summary>
        /// Raised when the server stops
        /// </summary>
        event ServerEventHandler BeforeStopping;

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
        /// Gets or sets the port number (as a string) used to create the prefix used by the HttpListener for incoming traffic; defaults to 1234
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Gets the default PublicFolder object to use for serving static content
        /// </summary>
        IPublicFolder PublicFolder { get; }

        /// <summary>
        /// Gets the list of all PublicFolder objects used for serving static content
        /// </summary>
        IList<IPublicFolder> PublicFolders { get; }

        /// <summary>
        /// Gets or sets the instance of IRouter to be used by this server to route incoming HTTP requests
        /// </summary>
        IRouter Router { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the listner should use the https protocol instead of http
        /// <para>&#160;</para>
        /// Note that if you create an HttpListener using https, you must select a Server Certificate for the listener. See the MSDN documentation on the HttpListener class for more information.<br />
        /// https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
        /// </summary>
        bool UseHttps { get; set; }

        /// <summary>
        /// Clones the event handlers on to an <see cref="IRestServer"/> object, preserving order
        /// </summary>
        /// <param name="server">The <see cref="IRestServer"/> object to clone the events to</param>
        void CloneEventHandlers(IRestServer server);
    }

    public class ServerSettings : IServerSettings
    {
        public event ServerEventHandler AfterStarting;
        public event ServerEventHandler AfterStopping;
        public event ServerEventHandler BeforeStarting;
        public event ServerEventHandler BeforeStopping;

        public bool EnableThrowingExceptions { get; set; }

        public string Host { get; set; }
        public string Port { get; set; }
        public bool UseHttps { get; set; }

        public IGrapevineLogger Logger { get; set; }
        public IList<IPublicFolder> PublicFolders { get; }

        public IRouter Router { get; set; }

        public ServerSettings()
        {
            PublicFolders = new List<IPublicFolder>();
            Host = "localhost";
            Logger = NullLogger.GetInstance();
            Port = "1234";
            Router = new Router();
            UseHttps = false;
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
    }
}
