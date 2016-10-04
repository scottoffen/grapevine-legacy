using System;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class FluentRouteCreatorFacts
    {
        public class UsingFunctions
        {
            [Fact]
            public void ForToUseExpression()
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
            public void ForUseExpression()
            {
                Func<IHttpContext, IHttpContext> function = context => context;
                var verb = HttpMethod.GET;

                var route = Route.For(verb).Use(function);

                route.HttpMethod.ShouldBe(verb);
                route.PathInfo.ShouldBe(string.Empty);
                route.Function.ShouldBe(function);
            }
        }

        public class UsingMethodInfo
        {
            [Fact]
            public void ForToUseExpression()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                const HttpMethod verb = HttpMethod.GET;
                const string pathinfo = "/some/route";

                var route = Route.For(verb).To(pathinfo).Use(method);

                route.HttpMethod.ShouldBe(verb);
                route.PathInfo.ShouldBe(pathinfo);
                route.Name.ShouldBe($"{method.ReflectedType.FullName}.{method.Name}");
            }

            [Fact]
            public void ForUseExpression()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                const HttpMethod verb = HttpMethod.GET;

                var route = Route.For(verb).Use(method);

                route.HttpMethod.ShouldBe(verb);
                route.PathInfo.ShouldBe(string.Empty);
                route.Name.ShouldBe($"{method.ReflectedType.FullName}.{method.Name}");
            }
        }
    }
}
