using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Util;

namespace Grapevine.Tests.Server.Attributes.Helpers
{
    public class RestRouteTesterHelper
    {
        [RestRoute]
        public IHttpContext RouteHasNoArgs(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.DELETE)]
        public IHttpContext RouteHasHttpMethodOnly(IHttpContext context)
        {
            return context;
        }

        [RestRoute(PathInfo = "/some/path")]
        public IHttpContext RouteHasPathInfoOnly(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/some/other/path")]
        public IHttpContext RouteHasBothArgs(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/index.html")]
        [RestRoute(HttpMethod = HttpMethod.HEAD, PathInfo = "/index")]
        public IHttpContext RouteHasMultipleAttrs(IHttpContext context)
        {
            return context;
        }
    }

    public abstract class AbstractRoutes
    {
        [RestRoute]
        public virtual IHttpContext InheritedMethod(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        public abstract IHttpContext AbstractMethod(IHttpContext context);
    }

    public class OneValidRoute : AbstractRoutes
    {
        public string Stuff { get; set; }

        public OneValidRoute()
        {
            Stuff = string.Empty;
        }

        [RestRoute]
        private IHttpContext PrivateMethod(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        protected IHttpContext ProtectedMethod(IHttpContext context)
        {
            return PrivateMethod(context);
        }

        [RestRoute]
        internal IHttpContext InternalMethod(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        public IHttpContext TheValidRoute(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        public sealed override IHttpContext AbstractMethod(IHttpContext context)
        {
            return context;
        }
    }
}
