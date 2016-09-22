using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace Grapevine.TestAssembly
{
    [RestResource(BasePath = "/todo")]
    public class ToDoListRoutes
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/list")]
        public IHttpContext GetItems(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.PUT, PathInfo = "/[id]")]
        public IHttpContext UpdateItem(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IHttpContext InsertItem(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.DELETE, PathInfo = "/[id]")]
        public IHttpContext DeleteItem(IHttpContext context)
        {
            return context;
        }

        public IHttpContext NoAttribute(IHttpContext context)
        {
            return context;
        }
    }
}
