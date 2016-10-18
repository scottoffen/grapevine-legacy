using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;
using Grapevine.TestAssembly;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouterFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultProperties()
            {
                var router = new Router();

                router.RoutingTable.ShouldNotBeNull();
                router.RoutingTable.ShouldBeEmpty();

                router.Logger.ShouldNotBeNull();
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Scanner.ShouldNotBeNull();
                router.Scanner.ShouldBeOfType<RouteScanner>();

                router.Scope.ShouldNotBeNull();
                string.IsNullOrWhiteSpace(router.Scope).ShouldBeTrue();
            }

            [Fact]
            public void DefaultPropertiesWithScope()
            {
                const string scope = "MyScope";
                var router = new Router(scope);

                router.RoutingTable.ShouldNotBeNull();
                router.RoutingTable.ShouldBeEmpty();

                router.Logger.ShouldNotBeNull();
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Scanner.ShouldNotBeNull();
                router.Scanner.ShouldBeOfType<RouteScanner>();

                router.Scope.ShouldNotBeNull();
                router.Scope.ShouldBe(scope);
            }

            [Fact]
            public void FluentInitialization()
            {
                const string scope = "MyScope";

                var router = Router.For(_ =>
                {
                    _.Register(Route.For(HttpMethod.ALL).Use(context => context));
                    _.ContinueRoutingAfterResponseSent = true;
                }, scope);

                router.Scope.ShouldBe(scope);
                router.RoutingTable.Count.ShouldBe(1);
                router.ContinueRoutingAfterResponseSent.ShouldBeTrue();
            }
        }

        public class AddRouteToTableMethod
        {
            [Fact]
            public void ThrowsExceptionIfRouteFunctionIsNull()
            {
                var router = new Router();
                var route = Substitute.For<IRoute>();
                route.Function.ReturnsNull();
                Should.Throw<ArgumentNullException>(() => router.AddRouteToTable(route));
            }
        }

        public class AfterProperty
        {
            [Fact]
            public void ExecutesAfterRouting()
            {
                var executionOrder = new List<string>();

                IList<IRoute> routing = new List<IRoute>();
                var route = new Route(ctx => { executionOrder.Add("function"); return ctx; });
                routing.Add(route);

                var router = new Router { After = ctx => { executionOrder.Add("after"); return ctx; } };

                router.Route(Mocks.HttpContext(), routing);

                executionOrder[0].ShouldBe("function");
                executionOrder[1].ShouldBe("after");
            }
        }

        public class BeforeProperty
        {
            [Fact]
            public void ExecutesBeforeRouting()
            {
                var executionOrder = new List<string>();

                IList<IRoute> routing = new List<IRoute>();
                var route = new Route(ctx => { executionOrder.Add("function"); return ctx; });
                routing.Add(route);

                var router = new Router { Before = ctx => { executionOrder.Add("before"); return ctx; } };

                router.Route(Mocks.HttpContext(), routing);

                executionOrder[0].ShouldBe("before");
                executionOrder[1].ShouldBe("function");
            }
        }

        public class ContinueRoutingAfterResponseSentProperty
        {
            [Fact]
            public void ContinuesExecutionAfterResponseSentWhenTrue()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var router = new Router { ContinueRoutingAfterResponseSent = true }.Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }).Register(ctx =>
                {
                    executed = true;
                    return ctx;
                });

                router.Route(context).ShouldBeTrue();
                executed.ShouldBeTrue();
            }

            [Fact]
            public void StopsExecutionAfterResponseSentWhenFalse()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var router = new Router().Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }).Register(ctx =>
                {
                    executed = true;
                    return ctx;
                });

                router.Route(context).ShouldBeTrue();
                executed.ShouldBeFalse();
            }
        }

        public class ImportMethod
        {
            [Fact]
            public void ThrowsExceptionWhenTypeIsNotAClass()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(IRouter)));
            }

            [Fact]
            public void ThrowsExceptionWhenTypeDoesNotImplementIRouter()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(NotARouter)));
            }

            [Fact]
            public void ThrowsExceptionWhenTypeIsAbstract()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(AbstractRouter)));
            }

            [Fact]
            public void ImportsRoutesFromAnotherRouterInstance()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(new RouterToImport());

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void ImportsRoutesFromSpecifiedRouterType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(typeof(RouterToImport));

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void ImportsRoutesFromGenericRouterType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import<RouterToImport>();

                router.RoutingTable.Count.ShouldBe(8);
            }
        }

        public class InsertMethod
        {
            [Fact]
            public void InsertBeforeByDefault()
            {
            }

            [Fact]
            public void InsertsBeforeIndex()
            {
            }

            [Fact]
            public void InsertsToTop()
            {
            }

            [Fact]
            public void InsertsToBottom()
            {
            }

            [Fact]
            public void InsertsAfterIndex()
            {
            }

            [Fact]
            public void DoesNotInsertDuplicate()
            {
            }

            [Fact]
            public void DoesNotInsertWhenIndexIsLessThanZero()
            {
            }

            [Fact]
            public void InsertsBeforeMarker()
            {
            }

            [Fact]
            public void InsertsAfterMarker()
            {
            }

            [Fact]
            public void DoesNotInsertWhenMarkerIsNotFound()
            {
            }

            [Fact]
            public void InsertsListBeforeIndex()
            {
            }

            [Fact]
            public void InsertsListAfterIndex()
            {
            }

            [Fact]
            public void InsertsListBeforeMarker()
            {
            }

            [Fact]
            public void InsertsListAfterMarker()
            {
            }
        }

        public class LoggerProperty
        {
            [Fact]
            public void SetToNullSetsNullLogger()
            {
                var router = new Router { Logger = new InMemoryLogger() };
                router.Logger.ShouldBeOfType<InMemoryLogger>();

                router.Logger = null;

                router.Logger.ShouldBeOfType<NullLogger>();
            }

            [Fact]
            public void PropogatesToScanner()
            {
                var scanner = new RouteScanner();
                scanner.Logger.ShouldBeOfType<NullLogger>();

                var router = new Router { Scanner = scanner };
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Logger = new InMemoryLogger();

                router.Logger.ShouldBeOfType<InMemoryLogger>();
                scanner.Logger.ShouldBeOfType<InMemoryLogger>();

                router.Scanner = null;
                Should.NotThrow(() => router.Logger = NullLogger.GetInstance());
            }
        }

        public class RegisterMethod
        {
            [Fact]
            public void RegistersRoute()
            {
                var route = new Route(context => context);
                var router = new Router();

                router.Register(route);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                var route = new Route(func);
                var router = new Router();

                router.Register(func);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAndHttpMethodAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const HttpMethod verb = HttpMethod.GET;

                var route = new Route(func, verb);
                var router = new Router();

                router.Register(func, verb);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAndPathInfoAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const string pathinfo = "/path";

                var route = new Route(func, pathinfo);
                var router = new Router();

                router.Register(func, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionHttpMethodAndPathInfoAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const HttpMethod verb = HttpMethod.GET;
                const string pathinfo = "/path";

                var route = new Route(func, verb, pathinfo);
                var router = new Router();

                router.Register(func, verb, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethod_as_route()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                var route = new Route(method);
                var router = new Router();

                router.Register(method);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodAndHttpMethodAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const HttpMethod verb = HttpMethod.GET;

                var route = new Route(method, verb);
                var router = new Router();

                router.Register(method, verb);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodAndPathInfoAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const string pathinfo = "/path";

                var route = new Route(method, pathinfo);
                var router = new Router();

                router.Register(method, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodHttpMethodAndPathInfoAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const HttpMethod verb = HttpMethod.GET;
                const string pathinfo = "/path";

                var route = new Route(method, verb, pathinfo);
                var router = new Router();

                router.Register(method, verb, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(MethodsToRegister));

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void RegistersGenericType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register<MethodsToRegister>();

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void RegistersAssembly()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(DefaultRouter).Assembly);

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void PreventsDuplicateRouteRegistrations()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                var route1 = new Route(method);
                var route2 = new Route(method);
                var router = new Router();

                router.Register(route1);
                router.Register(route2);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route1).ShouldBe(true);
                router.RoutingTable[0].Equals(route2).ShouldBe(true);
            }
        }

        public class RouteMethod
        {
            [Fact]
            public void ThrowsExceptionWhenRoutingIsNull()
            {
                var router = new Router();
                IList<IRoute> routing = null;
                Should.Throw<RouteNotFoundException>(() => router.Route(Mocks.HttpContext(), routing));
            }

            [Fact]
            public void ThrowsExceptionWhenRoutingIsEmpty()
            {
                var router = new Router();
                IList<IRoute> routing = new List<IRoute>();
                Should.Throw<RouteNotFoundException>(() => router.Route(Mocks.HttpContext(), routing));
            }

            [Fact]
            public void ReturnsTrueWhenContextHasBeenRespondedTo()
            {
                var executed = false;
                var route = new Route(ctx => { executed = true; return ctx; });

                var context = Substitute.For<IHttpContext>();
                context.WasRespondedTo.Returns(true);

                var router = new Router();
                IList<IRoute> routing = new List<IRoute>();
                routing.Add(route);

                router.Route(context, routing).ShouldBeTrue();

                executed.ShouldBeFalse();
            }
        }

        public class RouteForMethod
        {
            [Fact]
            public void ReturnsRoutesForGet()
            {
                var router = new Router().Import<RouterToImport>();
                var context = Mocks.HttpContext(new Dictionary<string, object> {{"HttpMethod", HttpMethod.GET}, {"PathInfo", "/user/list"}});

                var routes = router.RouteFor(context);

                router.RoutingTable.Count.ShouldBe(8);
                routes.Count.ShouldBe(1);
            }

            [Fact]
            public void ReturnsRoutesForPost()
            {
                var router = new Router().Import<RouterToImport>();
                var context = Mocks.HttpContext(new Dictionary<string, object> { { "HttpMethod", HttpMethod.POST }, { "PathInfo", "/user" } });

                var routes = router.RouteFor(context);

                router.RoutingTable.Count.ShouldBe(8);
                routes.Count.ShouldBe(1);
            }
        }

        public class ScanAssembliesMethod
        {
            [Fact]
            public void AddsRoutesToRouter()
            {
                var scanner = Substitute.For<IRouteScanner>();
                scanner.Scan().Returns(new List<IRoute>());

                var router = new Router { Scanner = scanner };

                router.ScanAssemblies();

                scanner.Received().Scan();
            }
        }
    }

    public abstract class AbstractRouter : Router
    {
        /* This class intentionally left blank */
        /* This is an IRouter but it's abstract*/
    }

    public class NotARouter
    {
        /* This class intentionally left blank */
        /* This is not an IRouter */
    }

    public class MethodsToRegister
    {
        [RestRoute]
        public IHttpContext Method(IHttpContext context) { return context; }
    }

    public class RouterToImport : Router
    {
        public RouterToImport()
        {
            AddToRoutingTable(Scanner.ScanAssembly(typeof(DefaultRouter).Assembly));
        }
    }

    public static class RouterExtensions
    {
        internal static void AddRouteToTable(this Router router, IRoute route)
        {
            var memberInfo = router.GetType();
            var method = memberInfo?.GetMethod("AddToRoutingTable", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(IRoute) }, null);

            try
            {
                method.Invoke(router, new object[] { route });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }
    }
}
