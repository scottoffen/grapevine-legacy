using System;
using Grapevine.Server;
using Grapevine.Util;
using Shouldly;
using Xunit;
using System.Collections.Generic;
using Grapevine.Server.Exceptions;
using Rhino.Mocks;

namespace Grapevine.Tests.Server
{
    public class RouterTester
    {
        [Fact]
        public void router_ctor_initializes_properties()
        {
            var router = new Router();

            router.GetExclusions().ShouldNotBeNull();
            router.GetRoutingTable().ShouldNotBeNull();
            router.Logger.ShouldBeOfType<NullLogger>();
            router.Scope.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void router_ctor_initializes_properties_with_scope()
        {
            const string scope = "MyScope";
            var router = new Router(scope);

            router.GetExclusions().ShouldNotBeNull();
            router.GetRoutingTable().ShouldNotBeNull();
            router.Logger.ShouldBeOfType<NullLogger>();
            router.Scope.Equals(scope).ShouldBeTrue();
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
        public void router_excludes_type()
        {
            var router = new Router();
            router.Exclude(typeof(RouteTestingHelper));

            router.Exclusions.Types.Count.ShouldBe(1);
            router.Exclusions.Types[0].ShouldBe(typeof(RouteTestingHelper));
        }

        [Fact]
        public void router_excludes_type_only_once()
        {
            var router = new Router();
            router.Exclusions.Types.Count.ShouldBe(0);

            router.Exclude<Route>();
            router.Exclusions.Types.Count.ShouldBe(1);

            router.Exclude<Router>();
            router.Exclusions.Types.Count.ShouldBe(2);

            router.Exclude<Route>();
            router.Exclusions.Types.Count.ShouldBe(2);
        }

        [Fact]
        public void router_excludes_generic_type()
        {
            var router = new Router();
            router.Exclude<RouteTestingHelper>();

            router.Exclusions.Types.Count.ShouldBe(1);
            router.Exclusions.Types[0].ShouldBe(typeof(RouteTestingHelper));
        }

        [Fact]
        public void router_excludes_namespace()
        {
            var ns = "My.Fictional.Namespace";
            var router = new Router();
            router.ExcludeNameSpace(ns);

            router.Exclusions.NameSpaces.Count.ShouldBe(1);
            router.Exclusions.NameSpaces[0].ShouldBe(ns);
        }

        [Fact]
        public void router_excludes_namespace_only_once()
        {
            var router = new Router();
            router.Exclusions.NameSpaces.Count.ShouldBe(0);

            router.ExcludeNameSpace("FakeNamespace");
            router.Exclusions.NameSpaces.Count.ShouldBe(1);

            router.ExcludeNameSpace("NamespaceFake");
            router.Exclusions.NameSpaces.Count.ShouldBe(2);

            router.ExcludeNameSpace("FakeNamespace");
            router.Exclusions.NameSpaces.Count.ShouldBe(2);
        }

        [Fact]
        public void router_registers_route()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
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
            var verb = HttpMethod.GET;

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
            var pathinfo = "/path";

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
            var verb = HttpMethod.GET;
            var pathinfo = "/path";

            var route = new Route(func, verb, pathinfo);
            var router = new Router();

            router.Register(func, verb, pathinfo);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route).ShouldBe(true);
        }

        [Fact]
        public void router_registers_method_as_route()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var route = new Route(method);
            var router = new Router();

            router.Register(method);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route).ShouldBe(true);
        }

        [Fact]
        public void router_registers_method_and_httpmethod_as_route()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var verb = HttpMethod.GET;

            var route = new Route(method, verb);
            var router = new Router();

            router.Register(method, verb);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route).ShouldBe(true);
        }

        [Fact]
        public void router_registers_method_and_pathinfo_as_route()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path";

            var route = new Route(method, pathinfo);
            var router = new Router();

            router.Register(method, pathinfo);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route).ShouldBe(true);
        }

        [Fact]
        public void router_registers_method_httpmethod_and_pathinfo_as_route()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var verb = HttpMethod.GET;
            var pathinfo = "/path";

            var route = new Route(method, verb, pathinfo);
            var router = new Router();

            router.Register(method, verb, pathinfo);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route).ShouldBe(true);
        }

        [Fact]
        public void router_registers_all_methods_in_type_without_baseurl()
        {
            var router = new Router();
            router.Register(typeof(RouterTestingHelperTwo));

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/two/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/");
        }

        [Fact]
        public void router_registers_all_methods_in_type_with_baseurl()
        {
            var router = new Router();
            router.Register(typeof(RouterTestingHelperOne));

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/one/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/one/");
        }

        [Fact]
        public void router_registers_all_methods_in_type_with_malformed_baseurl()
        {
            var router = new Router();
            router.Register(typeof(RouterTestingHelperFour));

            router.RoutingTable.Count.ShouldBe(3);
            router.RoutingTable[0].PathInfo.ShouldBe("/four/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/four/");
            router.RoutingTable[2].PathInfo.ShouldBe(@"^/four/\d+");
        }

        [Fact]
        public void router_registers_all_methods_in_type_without_attribute()
        {
            var router = new Router();
            router.Register(typeof(RouterTestingHelperThree));

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/three/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/");
        }

        [Fact]
        public void router_registers_all_methods_in_generic_type_with_baseurl()
        {
            var router = new Router();
            router.Register<RouterTestingHelperOne>();

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/one/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/one/");
        }

        [Fact]
        public void router_registers_all_methods_in_generic_type_without_baseurl()
        {
            var router = new Router();
            router.Register<RouterTestingHelperTwo>();

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/two/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/");
        }

        [Fact]
        public void router_registers_all_methods_in_generic_type_without_attribute()
        {
            var router = new Router();
            router.Register<RouterTestingHelperThree>();

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/three/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/");
        }

        [Fact]
        public void router_registers_by_scanning_assembly()
        {
            var router = new Router();

            router.RegisterAssembly();

            router.RoutingTable.Count.ShouldBe(7);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[3].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
            router.RoutingTable[4].Name.ShouldBe($"{typeof(RouterTestingHelperFour).FullName}.RouteOne");
            router.RoutingTable[5].Name.ShouldBe($"{typeof(RouterTestingHelperFour).FullName}.StaticRoute");
            router.RoutingTable[6].Name.ShouldBe($"{typeof(RouterTestingHelperFour).FullName}.RouteWithRegex");
        }

        [Fact]
        public void router_registers_by_scanning_assembly_and_skips_excluded_types()
        {
            var router = new Router();
            router.Exclude<RouterTestingHelperFour>();

            router.RegisterAssembly();

            router.RoutingTable.Count.ShouldBe(4);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[3].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
        }

        [Fact]
        public void router_registers_by_scanning_assembly_using_scope()
        {
            var router = new Router("myscope");

            router.RegisterAssembly();

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
        }

        [Fact]
        public void router_prevents_duplicate_route_registrations()
        {
            var route1 = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
            var route2 = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
            var router = new Router();

            router.Register(route1);
            router.Register(route2);

            router.RoutingTable.Count.ShouldBe(1);
            router.RoutingTable[0].Equals(route1).ShouldBe(true);
            router.RoutingTable[0].Equals(route2).ShouldBe(true);
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
            var myrouter = new MyRouter();
            var router = new Router();

            router.Import(myrouter);

            router.RoutingTable.Count.ShouldBe(7);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[3].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[4].Name.StartsWith(typeof(MyRouter).FullName).ShouldBe(true);
            router.RoutingTable[4].Name.EndsWith(myrouter.AnonName).ShouldBe(true);
            router.RoutingTable[5].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.RouteOne");
            router.RoutingTable[6].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.StaticRoute");
        }

        [Fact]
        public void router_imports_routes_from_type_of_router()
        {
            var router = new Router();
            var myrouter = new MyRouter();

            router.Import(typeof(MyRouter));

            router.RoutingTable.Count.ShouldBe(7);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[3].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[4].Name.StartsWith(typeof(MyRouter).FullName).ShouldBe(true);
            router.RoutingTable[4].Name.EndsWith(myrouter.AnonName).ShouldBe(true);
            router.RoutingTable[5].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.RouteOne");
            router.RoutingTable[6].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.StaticRoute");
        }

        [Fact]
        public void router_imports_routes_from_generic_type_of_router()
        {
            var router = new Router();
            var myrouter = new MyRouter();

            router.Import<MyRouter>();

            router.RoutingTable.Count.ShouldBe(7);
            router.RoutingTable[0].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.RouteOne");
            router.RoutingTable[1].Name.ShouldBe($"{typeof(RouterTestingHelperOne).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[3].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.StaticRoute");
            router.RoutingTable[2].Name.ShouldBe($"{typeof(RouterTestingHelperTwo).FullName}.RouteOne");
            router.RoutingTable[4].Name.StartsWith(typeof(MyRouter).FullName).ShouldBe(true);
            router.RoutingTable[4].Name.EndsWith(myrouter.AnonName).ShouldBe(true);
            router.RoutingTable[5].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.RouteOne");
            router.RoutingTable[6].Name.ShouldBe($"{typeof(RouterTestingHelperThree).FullName}.StaticRoute");
        }

        [Fact]
        public void router_returns_routes_for_get()
        {
            var router = new Router().Import<LoadedRouter>();
            var context = TestingMocks.MockContext(HttpMethod.GET);

            var routes = router.RouteFor(context);

            router.RoutingTable.Count.ShouldBe(10);
            routes.Count.ShouldBe(5);
        }

        [Fact]
        public void router_returns_routes_for_post()
        {
            var router = new Router().Import<LoadedRouter>();
            var context = TestingMocks.MockContext(HttpMethod.POST);

            var routes = router.RouteFor(context);

            router.RoutingTable.Count.ShouldBe(10);
            routes.Count.ShouldBe(6);
        }

        [Fact]
        public void router_route_throws_exception_if_routing_is_null()
        {
            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/path");

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);

            var router = new Router();
            IList<IRoute> routing = null;
            Should.Throw<RouteNotFoundException>(() => router.Route(context, routing));
        }

        [Fact]
        public void router_route_throws_exception_if_routing_is_empty()
        {
            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/path");

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);

            var router = new Router();
            IList<IRoute> routing = new List<IRoute>();
            Should.Throw<RouteNotFoundException>(() => router.Route(context, routing));
        }

        [Fact]
        public void router_route_returns_true_if_context_has_been_responded_to()
        {
            var executed = false;

            Func<IHttpContext, IHttpContext> function = ctx => { executed = true; return ctx; };
            var route = new Route(function);

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
            Func<IHttpContext, IHttpContext> before = ctx => { executionOrder.Add("before"); return ctx; };
            Func<IHttpContext, IHttpContext> function = ctx => { executionOrder.Add("function"); return ctx; };

            var route = new Route(function);
            IList<IRoute> routing = new List<IRoute>();

            var router = new Router();
            router.Before = before;
            routing.Add(route);

            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/");
            request.Stub(x => x.Id).Return("12345");
            request.Stub(x => x.Name).Return("fake request");

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);

            router.Route(context, routing);

            executionOrder[0].ShouldBe("before");
            executionOrder[1].ShouldBe("function");
        }

        [Fact]
        public void router_route_executes_after_delegate_after_routing()
        {
            var executionOrder = new List<string>();
            Func<IHttpContext, IHttpContext> after = ctx => { executionOrder.Add("after"); return ctx; };
            Func<IHttpContext, IHttpContext> function = ctx => { executionOrder.Add("function"); return ctx; };

            var route = new Route(function);
            IList<IRoute> routing = new List<IRoute>();

            var router = new Router();
            router.After = after;
            routing.Add(route);

            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/");
            request.Stub(x => x.Id).Return("12345");
            request.Stub(x => x.Name).Return("fake request");

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);

            router.Route(context, routing);

            executionOrder[0].ShouldBe("function");
            executionOrder[1].ShouldBe("after");
        }

        [Fact]
        public void router_route_continues_execution_after_response_sent_if_continue_flag_is_true()
        {
            var executed = false;

            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/");
            request.Stub(x => x.Id).Return("12345");
            request.Stub(x => x.Name).Return("fake request");

            var response = MockRepository.Mock<IHttpResponse>();

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);
            context.Stub(x => x.Response).Return(response);

            Func<IHttpContext, IHttpContext> one = ctx => { context.Stub(x => x.WasRespondedTo).Return(true); return ctx; };
            Func<IHttpContext, IHttpContext> two = ctx => { executed = true; return ctx; };

            var router = new Router();
            router.ContinueRoutingAfterResponseSent = true;
            router.Register(one);
            router.Register(two);

            router.Route(context).ShouldBeTrue();
            executed.ShouldBeTrue();
        }

        [Fact]
        public void router_route_stops_execution_after_response_sent_if_continue_flag_is_false()
        {
            var executed = false;

            var request = MockRepository.Mock<IHttpRequest>();
            request.Stub(x => x.HttpMethod).Return(HttpMethod.GET);
            request.Stub(x => x.PathInfo).Return("/");
            request.Stub(x => x.Id).Return("12345");
            request.Stub(x => x.Name).Return("fake request");

            var response = MockRepository.Mock<IHttpResponse>();

            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(x => x.Request).Return(request);
            context.Stub(x => x.Response).Return(response);

            Func<IHttpContext, IHttpContext> one = ctx => { context.Stub(x => x.WasRespondedTo).Return(true); return ctx; };
            Func<IHttpContext, IHttpContext> two = ctx => { executed = true; return ctx; };

            var router = new Router();
            router.Register(one);
            router.Register(two);

            router.Route(context).ShouldBeTrue();
            executed.ShouldBeFalse();
        }

        [Fact]
        public void router_protected_add_route_to_table_throws_exception_if_route_function_is_null()
        {
            var router = new Router();
            var route = MockRepository.Mock<IRoute>();
            Should.Throw<ArgumentNullException>(() => router.AddRouteToTable(route));
        }

        [Fact]
        public void router_runs_routes_for_get_context()
        {
            var loaded = new LoadedRouter();
            var router = new Router().Import(loaded);
            var context = TestingMocks.MockContext(HttpMethod.GET);

            router.Route(context);

            loaded.GetHits.ShouldBe(5);
            loaded.PostHits.ShouldBe(0);
        }

        [Fact]
        public void router_runs_routes_for_post_context()
        {
            var loaded = new LoadedRouter();
            var router = new Router().Import(loaded);
            var context = TestingMocks.MockContext(HttpMethod.POST);

            router.Route(context);

            loaded.GetHits.ShouldBe(0);
            loaded.PostHits.ShouldBe(6);
        }

        [Fact]
        public void router_runs_supplied_routes_for_context()
        {
            var hitme = false;
            var router = new Router().Import<LoadedRouter>();
            var context = TestingMocks.MockContext(HttpMethod.POST);

            var routes = router.RouteFor(context);
            routes.Add(new Route(ctx =>
            {
                hitme = true;
                return ctx;
            }));

            router.Route(context, routes);
            hitme.ShouldBe(true);
        }
    }
}
