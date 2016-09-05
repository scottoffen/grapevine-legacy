using System;
using Grapevine.Server;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class FluentRouteCreatorTester
    {
        [Fact]
        public void route_static_ctor_using_for_to_use_with_function()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            const HttpMethod verb = HttpMethod.GET;
            const string pathinfo = "/some/route";

            var route = Route.For(verb).To(pathinfo).Use(function);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(pathinfo);
            route.Function.ShouldBe(function);
        }

        [Fact]
        public void route_static_ctor_using_for_use_with_function()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var verb = HttpMethod.GET;

            var route = Route.For(verb).Use(function);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(string.Empty);
            route.Function.ShouldBe(function);
        }

        [Fact]
        public void route_static_ctor_using_for_to_use_with_methodinfo()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            const HttpMethod verb = HttpMethod.GET;
            const string pathinfo = "/some/route";

            var route = Route.For(verb).To(pathinfo).Use(method);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(pathinfo);
            route.Name.ShouldBe($"{method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_static_ctor_using_for_use_with_methodinfo()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            const HttpMethod verb = HttpMethod.GET;

            var route = Route.For(verb).Use(method);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(string.Empty);
            route.Name.ShouldBe($"{method.ReflectedType.FullName}.{method.Name}");
        }
    }
}
