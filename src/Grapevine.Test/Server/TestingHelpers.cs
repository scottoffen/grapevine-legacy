using Grapevine.Server;
using Grapevine.Util;
using Rhino.Mocks;

namespace Grapevine.Test.Server
{
    public class RouteTestingHelper
    {
        protected static string TriggeredBy;

        public IHttpContext RouteOne(IHttpContext context)
        {
            TriggeredBy = "RouteOne";
            return context;
        }

        public IHttpContext RouteTwo(IHttpContext context)
        {
            TriggeredBy = "RouteTwo";
            return context;
        }

        public static IHttpContext StaticRoute(IHttpContext context)
        {
            TriggeredBy = "StaticRoute";
            return context;
        }

        public static void WasTriggeredBy(string triggeredBy)
        {
            TriggeredBy = triggeredBy;
        }

        public static string WhoTriggeredMe()
        {
            var current = TriggeredBy;
            TriggeredBy = null;
            return current;
        }
    }

    [RestResource(BaseUrl = "/one")]
    public class RouterTestingHelperOne
    {
        public IHttpContext NotARoute(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/[topic]/[id]")]
        public IHttpContext RouteOne(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET)]
        public static IHttpContext StaticRoute(IHttpContext context)
        {
            return context;
        }
    }

    [RestResource(Scope = "myscope")]
    public class RouterTestingHelperTwo
    {
        public IHttpContext NotARoute(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/two/[topic]/[id]")]
        public IHttpContext RouteOne(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET)]
        public static IHttpContext StaticRoute(IHttpContext context)
        {
            return context;
        }
    }

    public class RouterTestingHelperThree
    {
        public IHttpContext NotARoute(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/three/[topic]/[id]")]
        public IHttpContext RouteOne(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET)]
        public static IHttpContext StaticRoute(IHttpContext context)
        {
            return context;
        }
    }

    public static class TestingMocks
    {
        public static IHttpContext MockContext(HttpMethod httpMethod = HttpMethod.ALL, string pathInfo = "/path")
        {
            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.PathInfo).Return(pathInfo);
            request.Stub(x => x.HttpMethod).Return(httpMethod);

            var response = MockRepository.Mock<IHttpResponse>();

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);
            context.Stub(x => x.Response).Return(response);

            return context;
        }
    }

    public class MyRouter : Router
    {
        public MyRouter()
        {
            Register<RouterTestingHelperOne>();
            Register<RouterTestingHelperTwo>();
            Register(context => context);
            Register<RouterTestingHelperTwo>();
            Register<RouterTestingHelperThree>();
        }
    }

    public class LoadedRouter : Router, IRouter
    {
        public int PostHits { get; set; }
        public int GetHits { get; set; }

        public LoadedRouter():base("*")
        {
            Register(context =>
            {
                if (context.Request.HttpMethod.Equals(HttpMethod.POST))
                    PostHits++;
                if (context.Request.HttpMethod.Equals(HttpMethod.GET))
                    GetHits++;
                return context;
            }, HttpMethod.ALL);

            Register(context =>
            {
                GetHits++;
                return context;
            }, HttpMethod.GET);

            Register(context =>
            {
                PostHits++;
                return context;
            }, HttpMethod.POST);

            Register(context =>
            {
                GetHits++;
                return context;
            }, HttpMethod.GET);

            Register(context =>
            {
                PostHits++;
                return context;
            }, HttpMethod.POST);

            Register(context =>
            {
                GetHits++;
                return context;
            }, HttpMethod.GET);

            Register(context =>
            {
                PostHits++;
                return context;
            }, HttpMethod.POST);

            Register(context =>
            {
                GetHits++;
                return context;
            }, HttpMethod.GET);

            Register(context =>
            {
                PostHits++;
                return context;
            }, HttpMethod.POST);

            Register(context =>
            {
                PostHits++;
                return context;
            }, HttpMethod.POST);
        }
    }
}
