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
            public void router_protected_add_route_to_table_throws_exception_if_route_function_is_null()
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
            public void router_route_executes_after_delegate_after_routing()
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
            public void router_route_executes_before_delegate_prior_to_routing()
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
            public void router_route_continues_execution_after_response_sent_if_continue_flag_is_true()
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
            public void router_route_stops_execution_after_response_sent_if_continue_flag_is_false()
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
            public void router_import_throws_error_if_type_is_not_a_class()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(IRouter)));
            }

            [Fact]
            public void router_import_throws_error_if_type_does_not_implement_irouter()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(NotARouter)));
            }

            [Fact]
            public void router_import_throws_error_if_type_is_abstract()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(AbstractRouter)));
            }

            [Fact]
            public void router_imports_routes_from_another_router_instance()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(new RouterToImport());

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void router_imports_routes_from_type_of_router()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(typeof(RouterToImport));

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void router_imports_routes_from_generic_type_of_router()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import<RouterToImport>();

                router.RoutingTable.Count.ShouldBe(8);
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
            public void router_registers_route()
            {
                var route = new Route(context => context);
                var router = new Router();

                router.Register(route);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void router_registers_function_as_route()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                var route = new Route(func);
                var router = new Router();

                router.Register(func);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void router_registers_function_and_httpmethod_as_route()
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
            public void router_registers_function_and_pathinfo_as_route()
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
            public void router_registers_function_httpmethod_and_pathinfo_as_route()
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
            public void router_registers_method_as_route()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                var route = new Route(method);
                var router = new Router();

                router.Register(method);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void router_registers_method_and_httpmethod_as_route()
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
            public void router_registers_method_and_pathinfo_as_route()
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
            public void router_registers_method_httpmethod_and_pathinfo_as_route()
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
            public void router_registers_type()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(MethodsToRegister));

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void router_registers_generic_type()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register<MethodsToRegister>();

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void router_registers_assembly()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(DefaultRouter).Assembly);

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void router_prevents_duplicate_route_registrations()
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
            public void router_route_throws_exception_if_routing_is_null()
            {
                var router = new Router();
                IList<IRoute> routing = null;
                Should.Throw<RouteNotFoundException>(() => router.Route(Mocks.HttpContext(), routing));
            }

            [Fact]
            public void router_route_throws_exception_if_routing_is_empty()
            {
                var router = new Router();
                IList<IRoute> routing = new List<IRoute>();
                Should.Throw<RouteNotFoundException>(() => router.Route(Mocks.HttpContext(), routing));
            }

            [Fact]
            public void router_route_returns_true_if_context_has_been_responded_to()
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
            public void router_returns_routes_for_get()
            {
                var router = new Router().Import<RouterToImport>();
                var context = Mocks.HttpContext(new Dictionary<string, object> {{"HttpMethod", HttpMethod.GET}, {"PathInfo", "/user/list"}});

                var routes = router.RouteFor(context);

                router.RoutingTable.Count.ShouldBe(8);
                routes.Count.ShouldBe(1);
            }

            [Fact]
            public void router_returns_routes_for_post()
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
            public void router_scan_assemblies_adds_routes_to_router()
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
