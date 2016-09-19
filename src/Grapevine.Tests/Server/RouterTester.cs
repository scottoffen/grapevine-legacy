using System;
using Grapevine.Server;
using Grapevine.Util;
using Shouldly;
using Xunit;
using System.Collections.Generic;
using Grapevine.Server.Exceptions;
using Grapevine.TestAssembly;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util.Loggers;
using Rhino.Mocks;

namespace Grapevine.Tests.Server
{
    public class RouterTester
    {
        [Fact]
        public void router_ctor_initializes_default_properties()
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
        public void router_ctor_initializes_default_properties_wiht_scope()
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
        public void router_ctor_fluent_initializes_properties()
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

        [Fact]
        public void router_set_logger_to_null_creates_null_logger()
        {
            var router = new Router {Logger = new InMemoryLogger()};
            router.Logger.ShouldBeOfType<InMemoryLogger>();

            router.Logger = null;

            router.Logger.ShouldBeOfType<NullLogger>();
        }

        [Fact]
        public void router_set_logger_sets_logger_on_scanner()
        {
            var scanner = new RouteScanner();
            scanner.Logger.ShouldBeOfType<NullLogger>();

            var router = new Router {Scanner = scanner};
            router.Logger.ShouldBeOfType<NullLogger>();

            router.Logger = new InMemoryLogger();

            router.Logger.ShouldBeOfType<InMemoryLogger>();
            scanner.Logger.ShouldBeOfType<InMemoryLogger>();

            router.Scanner = null;
            Should.NotThrow(() => router.Logger = NullLogger.GetInstance());
        }

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
        public void router_scan_assemblies_adds_routes_to_router()
        {
            var scanner = MockRepository.Mock<IRouteScanner>();
            scanner.Stub(s => s.Scan()).Return(new List<IRoute>());

            var router = new Router {Scanner = scanner};

            router.ScanAssemblies();

            scanner.AssertWasCalled(s => s.Scan());
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

        [Fact]
        public void router_protected_add_route_to_table_throws_exception_if_route_function_is_null()
        {
            var router = new Router();
            var route = MockRepository.Mock<IRoute>();
            Should.Throw<ArgumentNullException>(() => router.AddRouteToTable(route));
        }

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

        [Fact]
        public void router_returns_routes_for_get()
        {
            var router = new Router().Import<RouterToImport>();
            var context = MockContext.GetMockContext(MockContext.GetMockRequest(new MockProperties {HttpMethod = HttpMethod.GET, PathInfo = "/user/list"}));

            var routes = router.RouteFor(context);

            router.RoutingTable.Count.ShouldBe(8);
            routes.Count.ShouldBe(1);
        }

        [Fact]
        public void router_returns_routes_for_post()
        {
            var router = new Router().Import<RouterToImport>();
            var context = MockContext.GetMockContext(MockContext.GetMockRequest(new MockProperties { HttpMethod = HttpMethod.POST, PathInfo = "/user" }));

            var routes = router.RouteFor(context);

            router.RoutingTable.Count.ShouldBe(8);
            routes.Count.ShouldBe(1);
        }

        [Fact]
        public void router_route_throws_exception_if_routing_is_null()
        {
            var router = new Router();
            IList<IRoute> routing = null;
            Should.Throw<RouteNotFoundException>(() => router.Route(MockContext.GetMockContext(), routing));
        }

        [Fact]
        public void router_route_throws_exception_if_routing_is_empty()
        {
            var router = new Router();
            IList<IRoute> routing = new List<IRoute>();
            Should.Throw<RouteNotFoundException>(() => router.Route(MockContext.GetMockContext(), routing));
        }

        [Fact]
        public void router_route_returns_true_if_context_has_been_responded_to()
        {
            var executed = false;
            var route = new Route(ctx => { executed = true; return ctx; });

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.WasRespondedTo).Return(true);

            var router = new Router();
            IList<IRoute> routing = new List<IRoute>();
            routing.Add(route);

            router.Route(context, routing).ShouldBeTrue();

            executed.ShouldBeFalse();
        }

        [Fact]
        public void router_route_executes_before_delegate_prior_to_routing()
        {
            var executionOrder = new List<string>();

            IList<IRoute> routing = new List<IRoute>();
            var route = new Route(ctx => { executionOrder.Add("function"); return ctx; });
            routing.Add(route);

            var router = new Router {Before = ctx => { executionOrder.Add("before"); return ctx; } };

            router.Route(MockContext.GetMockContext(), routing);

            executionOrder[0].ShouldBe("before");
            executionOrder[1].ShouldBe("function");
        }

        [Fact]
        public void router_route_executes_after_delegate_after_routing()
        {
            var executionOrder = new List<string>();

            IList<IRoute> routing = new List<IRoute>();
            var route = new Route(ctx => { executionOrder.Add("function"); return ctx; });
            routing.Add(route);

            var router = new Router { After = ctx => { executionOrder.Add("after"); return ctx; } };

            router.Route(MockContext.GetMockContext(), routing);

            executionOrder[0].ShouldBe("function");
            executionOrder[1].ShouldBe("after");
        }

        [Fact]
        public void router_route_continues_execution_after_response_sent_if_continue_flag_is_true()
        {
            var executed = false;
            var context = MockContext.GetMockContext();

            var router = new Router {ContinueRoutingAfterResponseSent = true}.Register(ctx =>
            {
                context.Stub(x => x.WasRespondedTo).Return(true);
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
            var context = MockContext.GetMockContext();

            var router = new Router().Register(ctx =>
            {
                context.Stub(x => x.WasRespondedTo).Return(true);
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
}
