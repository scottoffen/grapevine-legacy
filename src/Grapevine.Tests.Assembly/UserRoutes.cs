using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace Grapevine.TestAssembly
{
    [RestResource(BasePath = "/user")]
    public class UserRoutes
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/list")]
        public IHttpContext GetUsers(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.PUT, PathInfo = "/[id]")]
        public IHttpContext UpdateUser(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IHttpContext InsertUser(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.DELETE, PathInfo = "/[id]")]
        public IHttpContext DeleteUser(IHttpContext context)
        {
            return context;
        }

        public IHttpContext NoAttribute(IHttpContext context)
        {
            return context;
        }
    }
}
