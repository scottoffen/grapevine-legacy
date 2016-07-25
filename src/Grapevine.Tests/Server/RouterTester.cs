using System;
using Grapevine.Server;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouterTester
    {
        [Fact]
        public void router_exclude_type()
        {
            var router = new Router();
            router.Exclude(typeof(RouteTestingHelper));

            router.Exclusions.Types.Count.ShouldBe(1);
            router.Exclusions.Types[0].ShouldBe(typeof(RouteTestingHelper));
        }

        [Fact]
        public void router_exclude_generic_type()
        {
            var router = new Router();
            router.Exclude<RouteTestingHelper>();

            router.Exclusions.Types.Count.ShouldBe(1);
            router.Exclusions.Types[0].ShouldBe(typeof(RouteTestingHelper));
        }

        [Fact]
        public void router_exclude_namespace()
        {
            var ns = "My.Fictional.Namespace";
            var router = new Router();
            router.ExcludeNameSpace(ns);

            router.Exclusions.NameSpaces.Count.ShouldBe(1);
            router.Exclusions.NameSpaces[0].ShouldBe(ns);
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
            var method = typeof (RouteTestingHelper).GetMethod("RouteOne");
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
        public void router_registers_all_methods_in_type_with_baseurl()
        {
            var router = new Router();
            router.Register(typeof(RouterTestingHelperOne));

            router.RoutingTable.Count.ShouldBe(2);
            router.RoutingTable[0].PathInfo.ShouldBe("/one/[topic]/[id]");
            router.RoutingTable[1].PathInfo.ShouldBe("/one/");
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
