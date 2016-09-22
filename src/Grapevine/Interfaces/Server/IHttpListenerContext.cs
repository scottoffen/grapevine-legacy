using System.Net;
using System.Security.Principal;

namespace Grapevine.Interfaces.Server
{
    public interface IHttpListenerContext
    {
        /// <summary>
        /// Gets the HttpListenerRequest that represents a client's request for a resource.
        /// </summary>
        HttpListenerRequest Request { get; }

        /// <summary>
        /// Gets the HttpListenerResponse object that will be sent to the client in response to the client's request.
        /// </summary>
        HttpListenerResponse Response { get; }

        /// <summary>
        /// Gets an object used to obtain identity, authentication information, and security roles for the client whose request is represented by this HttpListenerContext object.
        /// </summary>
        IPrincipal User { get; }
    }
}
