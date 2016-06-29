using System.Security.Principal;
using System.Net;

namespace Grapevine.Server
{
    public interface IHttpContext
    {
        dynamic Dynamic { get; }
        IHttpRequest Request { get; }
        IHttpResponse Response { get; }
        IPrincipal User { get; }
        IRestServer Server { get; }

        bool WasRespondedTo();
    }

    public class HttpContext : DynamicAspect, IHttpContext
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IPrincipal User { get; }
        public IRestServer Server { get; protected internal set; }

        internal HttpContext (HttpListenerContext context, IRestServer server)
        {
            Request = new HttpRequest(context.Request);
            Response = new HttpResponse(context.Response, context.Request.Headers);
            User = context.User;
            Server = server;
        }

        public bool WasRespondedTo()
        {
            return Response.ResponseSent;
        }
    }
}
