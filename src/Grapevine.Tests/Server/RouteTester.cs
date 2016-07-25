using System;
using Grapevine.Server;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteTester
    {
        [Fact]
        public void route_ctor_methodinfo_only_correctly_sets_properties()
        {
            var method = typeof (RouteTestingHelper).GetMethod("RouteOne");
            var route = new Route(method);

            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_methodinfo_and_httpmethod_correctly_sets_properties()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var route = new Route(method, HttpMethod.GET);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_methodinfo_and_pathinfo_correctly_sets_properties()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path/to/resource";
            var pattern = "^/path/to/resource$";

            var route = new Route(method, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_methodinfo_http_method_and_pathinfo_correctly_sets_properties()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path/[param1]/[param2]";
            var pattern = "^/path/(.+)/(.+)$";

            var route = new Route(method, HttpMethod.GET, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_method_and_pathinfo_with_params_correctly_sets_properties()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path/[param1]/[param2]";
            var pattern = "^/path/(.+)/(.+)$";

            var route = new Route(method, pathinfo);

            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_function_only_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var route = new Route(function);

            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_function_and_httpmethod_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var route = new Route(function, HttpMethod.GET);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_function_and_pathinfo_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var pathinfo = "/path/to/resource";
            var pattern = "^/path/to/resource$";

            var route = new Route(function, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_function_http_method_and_pathinfo_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var pathinfo = "/path/[param1]/[param2]";
            var pattern = "^/path/(.+)/(.+)$";

            var route = new Route(function, HttpMethod.GET, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_function_and_pathinfo_with_params_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var pathinfo = "/path/[param1]/[param2]";
            var pattern = "^/path/(.+)/(.+)$";

            var route = new Route(function, pathinfo);

            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_static_ctor_using_for_to_use()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var verb = HttpMethod.GET;
            var pathinfo = "/some/route";

            var route = Route.For(verb).To(pathinfo).Use(function);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(pathinfo);
            route.Function.ShouldBe(function);
        }

        [Fact]
        public void route_static_ctor_using_for_use()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var verb = HttpMethod.GET;

            var route = Route.For(verb).Use(function);

            route.HttpMethod.ShouldBe(verb);
            route.PathInfo.ShouldBe(string.Empty);
            route.Function.ShouldBe(function);
        }

        [Fact]
        public void route_equality_false_when_created_using_different_methods()
        {
            var method1 = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var method2 = typeof(RouteTestingHelper).GetMethod("RouteTwo");
            var verb = HttpMethod.GET;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(method1, verb, pathinfo);
            var route2 = new Route(method2, verb, pathinfo);

            route1.Equals(route2).ShouldBe(false);
        }

        [Fact]
        public void route_equality_false_when_created_using_different_httpmethods()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(method, HttpMethod.GET, pathinfo);
            var route2 = new Route(method, HttpMethod.POST, pathinfo);

            route1.Equals(route2).ShouldBe(false);
        }

        [Fact]
        public void route_equality_false_when_created_using_different_pathinfo()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var verb = HttpMethod.GET;
            var pathinfo1 = "/path1/[param1]/[param2]";
            var pathinfo2 = "/path2/[param1]/[param2]";

            var route1 = new Route(method, verb, pathinfo1);
            var route2 = new Route(method, verb, pathinfo2);

            route1.Equals(route2).ShouldBe(false);
        }

        [Fact]
        public void route_equality_false_when_routes_created_using_different_underlying_function_and_method()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            Func<IHttpContext, IHttpContext> function = context => context;

            var verb = HttpMethod.GET;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(method, verb, pathinfo);
            var route2 = new Route(function, verb, pathinfo);

            route1.Equals(route2).ShouldBe(false);
        }

        [Fact]
        public void route_equality_false_when_routes_created_using_identical_but_different_underlying_functions()
        {
            Func<IHttpContext, IHttpContext> function1 = context => context;
            Func<IHttpContext, IHttpContext> function2 = context => context;

            var verb = HttpMethod.GET;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(function1, verb, pathinfo);
            var route2 = new Route(function2, verb, pathinfo);

            route1.Equals(route2).ShouldBe(false);
        }

        [Fact]
        public void route_equality_true_when_routes_created_using_overlapping_patterns()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var verb = HttpMethod.GET;
            var pathinfo1 = "/path/[param1]/[param2]";
            var pathinfo2 = "/path/[id]/[key]";

            var route1 = new Route(method, verb, pathinfo1);
            var route2 = new Route(method, verb, pathinfo2);

            route1.Equals(route2).ShouldBe(true);
        }

        [Fact]
        public void route_equality_true_when_routes_created_using_same_underlying_method()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            Func<IHttpContext, IHttpContext> function = new RouteTestingHelper().RouteOne;

            var verb = HttpMethod.GET;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(method, verb, pathinfo);
            var route2 = new Route(function, verb, pathinfo);

            route1.Equals(route2).ShouldBe(true);
        }

        [Fact]
        public void route_equality_true_when_routes_created_using_same_underlying_function()
        {
            Func<IHttpContext, IHttpContext> function = context => context;

            var verb = HttpMethod.GET;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(function, verb, pathinfo);
            var route2 = new Route(function, verb, pathinfo);

            route1.Equals(route2).ShouldBe(true);
        }

        [Fact]
        public void route_equality_true_when_routes_created_using_equivalent_httpmethods()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var pathinfo = "/path/[param1]/[param2]";

            var route1 = new Route(function, HttpMethod.ALL, pathinfo);
            var route2 = new Route(function, HttpMethod.GET, pathinfo);

            route1.Equals(route2).ShouldBe(true);
        }

        [Fact]
        public void route_equality_true_when_created_from_method_only_using_same_method()
        {
            var route1 = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
            var route2 = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));

            route1.Equals(route2).ShouldBe(true);
            route1.Name.Equals(route2.Name).ShouldBe(true);
        }

        [Fact]
        public void route_equality_true_when_created_from_function_only_using_same_function()
        {
            Func<IHttpContext, IHttpContext> func = context => context;
            var route1 = new Route(func);
            var route2 = new Route(func);

            route1.Equals(route2).ShouldBe(true);
            route1.Name.Equals(route2.Name).ShouldBe(true);
        }

        [Fact]
        public void route_tostring_overrides_with_function()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var pathinfo = "/path/to/resource";
            var route = new Route(function, HttpMethod.ALL, pathinfo);

            route.ToString().ShouldBe($"{HttpMethod.ALL} {pathinfo} > {function.Method.ReflectedType.FullName}.{function.Method.Name}");
        }

        [Fact]
        public void route_tostring_overrides_with_method()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            var pathinfo = "/path/to/resource";
            var route = new Route(method, HttpMethod.GET, "/path/to/resource");

            route.ToString().ShouldBe($"{HttpMethod.GET} {pathinfo} > {method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_tostring_overrides_with_function_wrapped_method()
        {
            var method = typeof(RouteTestingHelper).GetMethod("RouteOne");
            Func<IHttpContext, IHttpContext> function = new RouteTestingHelper().RouteOne;
            var pathinfo = "/path/to/resource";
            var route = new Route(function, HttpMethod.GET, pathinfo);

            route.ToString().ShouldBe($"{HttpMethod.GET} {pathinfo} > {method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_matches_true_with_exact_match()
        {
            var pathinfo = "/path/to/resource";
            var verb = HttpMethod.GET;

            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), verb, pathinfo);
            var context = TestingMocks.MockContext(verb, pathinfo);

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_true_with_everything()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
            var context = TestingMocks.MockContext(HttpMethod.GET, "/path/to/resource");

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_true_with_equivalent_httpmethod()
        {
            var pathinfo = "/path/to/resource";
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), HttpMethod.ALL, pathinfo);
            var context = TestingMocks.MockContext(HttpMethod.GET, pathinfo);

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_true_with_no_httpmethod()
        {
            var pathinfo = "/path/to/resource";
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), pathinfo);
            var context = TestingMocks.MockContext(HttpMethod.GET, pathinfo);

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_true_with_equivalent_pathinfo()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), HttpMethod.GET);
            var context = TestingMocks.MockContext(HttpMethod.GET, "/path/to/resource");

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_true_with_equivalent_pathinfo_with_params()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), HttpMethod.GET, "/path/[param1]/[param2]");
            var context = TestingMocks.MockContext(HttpMethod.GET, "/path/user/234");

            route.Matches(context).ShouldBe(true);
        }

        [Fact]
        public void route_matches_false_with_different_pathinfo()
        {
            var verb = HttpMethod.GET;
            var pathinfo = "/path/to/resource";

            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), verb, $"{pathinfo}1");
            var context = TestingMocks.MockContext(verb, $"{pathinfo}2");

            route.Matches(context).ShouldBe(false);
        }

        [Fact]
        public void route_matches_false_with_different_httpmethod()
        {
            var pathinfo = "/path/to/resource";
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), HttpMethod.GET, pathinfo);
            var context = TestingMocks.MockContext(HttpMethod.POST, pathinfo);

            route.Matches(context).ShouldBe(false);
        }

        [Fact]
        public void route_matches_false_with_pathinfo_params()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"), "/api/user/[id]");
            var context = TestingMocks.MockContext(HttpMethod.GET, "/api/user/");

            route.Matches(context).ShouldBe(false);
        }

        [Fact]
        public void route_invoke_using_method()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("RouteOne"));
            var context = TestingMocks.MockContext();

            route.Invoke(context);

            RouteTestingHelper.WhoTriggeredMe().ShouldBe("RouteOne");
        }

        [Fact]
        public void route_invoke_using_static_method()
        {
            var route = new Route(typeof(RouteTestingHelper).GetMethod("StaticRoute"));
            var context = TestingMocks.MockContext();

            route.Invoke(context);

            RouteTestingHelper.WhoTriggeredMe().ShouldBe("StaticRoute");
        }

        [Fact]
        public void route_invoke_using_function()
        {
            var triggeredBy = "Anon";
            var context = TestingMocks.MockContext();
            var route = new Route(ctx =>
            {
                RouteTestingHelper.WasTriggeredBy(triggeredBy);
                return ctx;
            });

            route.Invoke(context);

            RouteTestingHelper.WhoTriggeredMe().ShouldBe(triggeredBy);
        }

        [Fact]
        public void route_invoke_using_function_wrapped_method()
        {
            Func<IHttpContext, IHttpContext> function = new RouteTestingHelper().RouteTwo;
            var route = new Route(function);
            var context = TestingMocks.MockContext();

            route.Invoke(context);

            RouteTestingHelper.WhoTriggeredMe().ShouldBe("RouteTwo");
        }
    }


}
