using System.Security.Principal;
using System.Net;
using Grapevine.Server;
using Grapevine.Shared;

namespace Grapevine.Interfaces.Server
{
    /// <summary>
    /// Provides modified access to the request and response objects used by the HttpListener class
    /// </summary>
    public interface IHttpContext : IDynamicProperties
    {
        /// <summary>
        /// Gets the IHttpRequest that represents a client's request for a resource
        /// </summary>
        IHttpRequest Request { get; }

        /// <summary>
        /// Gets the IHttpResponse object that will be sent to the client in response to the client's request
        /// </summary>
        IHttpResponse Response { get; }

        /// <summary>
        /// Gets an object used to obtain identity, authentication information, and security roles for the client whose request is represented by the underlying HttpListenerContext object
        /// </summary>
        IPrincipal User { get; }

        /// <summary>
        /// Gets the IRestServer object the client request was sent to
        /// </summary>
        IRestServer Server { get; }

        /// <summary>
        /// Returns a value that indicate whether or not the client request has been responded to
        /// </summary>
        bool WasRespondedTo { get; }
    }

    public class HttpContext : DynamicProperties, IHttpContext
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IPrincipal User { get; }
        public IRestServer Server { get; }

        internal HttpContext (HttpListenerContext context, IRestServer server)
        {
            Request = new HttpRequest(context.Request);
            Response = new HttpResponse(context.Response, context.Request.Headers);
            User = context.User;
            Server = server;
        }

        public bool WasRespondedTo => Response.ResponseSent;
    }
}
