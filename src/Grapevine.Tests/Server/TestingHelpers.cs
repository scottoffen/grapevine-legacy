using System;
using Grapevine.Server;
using Grapevine.Util;
using Rhino.Mocks;

namespace Grapevine.Tests.Server
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

        public void FailureRouteOne(IHttpContext context)
        {
            /* This method intentionally left blank */
        }

        public IHttpContext FailureRouteTwo(IHttpContext context, bool makeThisFail)
        {
            return context;
        }

        public IHttpContext FailureRouteThree()
        {
            return null;
        }

        public IHttpContext FailureRouteFour(bool makeThisFail)
        {
            return null;
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

    [RestResource(BasePath = "/one")]
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

    [RestResource(BasePath = "four/")]
    public class RouterTestingHelperFour
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

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = @"^/\d+")]
        public static IHttpContext RouteWithRegex(IHttpContext context)
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

            var server = MockRepository.Mock<IRestServer>();
            server.Stub(x => x.Logger).Return(new NullLogger());

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);
            context.Stub(x => x.Response).Return(response);
            context.Stub(x => x.Server).Return(server);

            return context;
        }
    }

    public class MyRouter : Router
    {
        public string AnonName { get; }

        public MyRouter()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            AnonName = function.Method.Name;

            Register<RouterTestingHelperOne>();
            Register<RouterTestingHelperTwo>();
            Register(function);
            Register<RouterTestingHelperTwo>();
            Register<RouterTestingHelperThree>();
        }
    }

    public class LoadedRouter : Router
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
