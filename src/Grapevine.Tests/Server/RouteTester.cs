using System;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Server.Exceptions;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Rhino.Mocks;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteTester
    {
        public RouteTester()
        {
            Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add(
                typeof(System.Security.Permissions.ReflectionPermissionAttribute));
            Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add(
                typeof(System.Security.Permissions.PermissionSetAttribute));
        }

        [Fact]
        public void route_convert_method_to_func_throws_excpetion_if_methodinfo_is_null()
        {
            MethodInfo method = null;
            Should.Throw<ArgumentNullException>(() => { Route.ConvertMethodToFunc(method); });
        }

        [Fact]
        public void route_convert_method_to_func_throws_exception_if_methodinfo_return_type_is_not_ihttpcontext()
        {
            var method = typeof(RouteTesterHelper).GetMethod("DoesNotReturnContext");
            Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(method); });
        }

        [Fact]
        public void route_convert_method_to_func_throws_exception_if_methodinfo_number_of_args_is_not_one()
        {
            var methodA = typeof(RouteTesterHelper).GetMethod("MethodTakesZeroArgs");
            var methodB = typeof(RouteTesterHelper).GetMethod("MethodTakesTwoArgs");

            Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(methodA); });
            Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(methodB); });
        }

        [Fact]
        public void route_convert_method_to_func_throws_exception_if_methodinfo_first_argument_is_not_ihttpcontext()
        {
            var method = typeof(RouteTesterHelper).GetMethod("DoesNotTakeContextArg");
            Should.Throw<InvalidRouteMethodExceptions>(() => { var route = new Route(method); });
        }

        [Fact]
        public void route_ctor_methodinfo_only_correctly_sets_properties()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var route = new Route(method);

            route.Enabled.ShouldBeTrue();
            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
            route.Description.ShouldBe($"{HttpMethod.ALL}  > {method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_ctor_methodinfo_and_httpmethod_correctly_sets_properties()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var route = new Route(method, HttpMethod.GET);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
            route.Description.ShouldBe($"{HttpMethod.GET}  > {method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_ctor_methodinfo_and_pathinfo_correctly_sets_properties()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var pathinfo = "/path/to/resource";
            var pattern = "^/path/to/resource$";

            var route = new Route(method, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
            route.Description.ShouldBe($"{HttpMethod.ALL} {pathinfo} > {method.ReflectedType.FullName}.{method.Name}");
        }

        [Fact]
        public void route_ctor_methodinfo_http_method_and_pathinfo_correctly_sets_properties()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            const string pathinfo = "/path/[param1]/[param2]";
            const string pattern = "^/path/(.+)/(.+)$";

            var route = new Route(method, HttpMethod.GET, pathinfo);

            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
            route.Description.ShouldBe($"{HttpMethod.GET} {pathinfo} > {method.ReflectedType.FullName}.{method.Name}");

            var keys = route.GetPatternKeys();
            keys.Count.ShouldBe(2);
            keys[0].ShouldBe("param1");
            keys[1].ShouldBe("param2");
        }

        [Fact]
        public void route_ctor_throws_exception_if_function_is_null()
        {
            Func<IHttpContext, IHttpContext> func = null;
            Should.Throw<ArgumentNullException>(() => { var route = new Route(func); });
        }

        [Fact]
        public void route_ctor_function_only_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var route = new Route(function);

            route.Enabled.ShouldBeTrue();
            route.Function.ShouldBe(function);
            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_function_and_httpmethod_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            var route = new Route(function, HttpMethod.GET);

            route.Function.ShouldBe(function);
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(string.Empty);
            route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void route_ctor_function_and_pathinfo_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            const string pathinfo = "/path/to/resource";
            const string pattern = "^/path/to/resource$";

            var route = new Route(function, pathinfo);

            route.Function.ShouldBe(function);
            route.HttpMethod.ShouldBe(HttpMethod.ALL);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);
        }

        [Fact]
        public void route_ctor_function_http_method_and_pathinfo_correctly_sets_properties()
        {
            Func<IHttpContext, IHttpContext> function = context => context;
            const string pathinfo = "/path/[param1]/[param2]";
            const string pattern = "^/path/(.+)/(.+)$";

            var route = new Route(function, HttpMethod.GET, pathinfo);

            route.Function.ShouldBe(function);
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe(pathinfo);
            route.PathInfoPattern.ToString().ShouldBe(pattern);

            var keys = route.GetPatternKeys();
            keys.Count.ShouldBe(2);
            keys[0].ShouldBe("param1");
            keys[1].ShouldBe("param2");
        }

        [Fact]
        public void route_enable_and_disable_set_toggle_correctly()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var route = new Route(method);

            route.Enabled.ShouldBeTrue();

            route.Disable();
            route.Enabled.ShouldBeFalse();

            route.Enable();
            route.Enabled.ShouldBeTrue();
        }

        [Fact]
        public void route_invoke_does_nothing_when_disabled()
        {
            var executed = false;
            Func<IHttpContext, IHttpContext> function = context =>
            {
                executed = true;
                return context;
            };

            var route = new Route(function);
            route.Disable();
            route.Invoke(MockContext.GetMockContext());

            executed.ShouldBeFalse();
        }

        [Fact]
        public void route_invokes_using_function()
        {
            const string expected = "1234";
            var actual = string.Empty;

            Func<IHttpContext, IHttpContext> function = context =>
            {
                actual = context.Request.PathParameters["id"];
                return context;
            };

            var route = new Route(function, "/id/[id]");
            route.Invoke(MockContext.GetMockContext(MockContext.GetMockRequest(new MockProperties { PathInfo = $"/id/{expected}" })));

            actual.Equals(expected).ShouldBeTrue();
        }

        [Fact]
        public void route_invokes_using_non_static_methodinfo()
        {
            var method = typeof(RouteTesterHelper).GetMethod("InstanceMethodInfo");
            var route = new Route(method);

            RouteTesterHelper.InstanceMethodInfoHit.ShouldBeFalse();

            route.Invoke(MockContext.GetMockContext());

            RouteTesterHelper.InstanceMethodInfoHit.ShouldBeTrue();
        }

        [Fact]
        public void route_invokes_using_non_static_methodinfo_context_not_shared()
        {
            var method1 = typeof(RouteTesterHelper).GetMethod("ContextNotShared");
            var method2 = typeof(RouteTesterHelper).GetMethod("ContextNotShared");

            var route1 = new Route(method1);
            var route2 = new Route(method2);

            var response = MockContext.GetMockResponse();
            var context = MockContext.GetMockContext(response);

            route1.Invoke(context);
            response.AssertWasNotCalled(r => r.Abort());

            route2.Invoke(context);
            response.AssertWasNotCalled(r => r.Abort());
        }

        [Fact]
        public void route_invokes_using_static_methodinfo()
        {
            var method = typeof(RouteTesterHelper).GetMethod("StaticMethodInfo");
            var route = new Route(method);

            RouteTesterHelper.StaticMethodInfoHit.ShouldBeFalse();

            route.Invoke(MockContext.GetMockContext());

            RouteTesterHelper.StaticMethodInfoHit.ShouldBeTrue();
        }

        [Fact]
        public void route_invokes_using_static_method()
        {
            var route = new Route(RouteTesterHelper.StaticMethod);

            RouteTesterHelper.StaticMethodHit.ShouldBeFalse();

            route.Invoke(MockContext.GetMockContext());

            RouteTesterHelper.StaticMethodHit.ShouldBeTrue();
        }

        [Fact]
        public void route_invokes_using_instance_method()
        {
            var instance = new RouteTesterHelper();
            var route = new Route(instance.InstanceMethod);

            RouteTesterHelper.InstanceMethodHit.ShouldBeFalse();

            route.Invoke(MockContext.GetMockContext());

            RouteTesterHelper.InstanceMethodHit.ShouldBeTrue();
            instance.InstanceHits.ShouldBe(1);

            route.Invoke(MockContext.GetMockContext());
            instance.InstanceHits.ShouldBe(2);
        }

        [Fact]
        public void route_invokes_using_shared_instance_method_appropriately()
        {
            var method = typeof(RouteTesterHelper).GetMethod("SharedInstanceMethod");
            var instance = new RouteTesterHelper();

            var route1 = new Route(instance.SharedInstanceMethod);
            var route2 = new Route(instance.SharedInstanceMethod);
            var route3 = new Route(method);

            RouteTesterHelper.SharedInstanceMethodHit.ShouldBeFalse();

            route1.Invoke(MockContext.GetMockContext());

            RouteTesterHelper.SharedInstanceMethodHit.ShouldBeTrue();
            instance.InstanceHits.ShouldBe(1);

            route2.Invoke(MockContext.GetMockContext());
            instance.InstanceHits.ShouldBe(2);

            route3.Invoke(MockContext.GetMockContext());
            instance.InstanceHits.ShouldBe(2);

            route1.Invoke(MockContext.GetMockContext());
            instance.InstanceHits.ShouldBe(3);
        }

        [Fact]
        public void route_matches_true_with_exact_match()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verb, pathinfo);

            route.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void route_matches_true_with_equivalent_httpmethod()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, HttpMethod.ALL, pathinfo);

            route.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void route_matches_true_with_wildcard_pathinfo()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verb);

            route.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void route_matches_true_with_equivalent_httpmethod_and_wildcard_pathinfo()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod);

            route.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void route_matches_true_with_path_params()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verb, @"/path/to/[something]");

            route.Matches(context).ShouldBeTrue();
        }

        [Fact]
        public void route_matches_false_with_different_httpmethod()
        {
            const string pathinfo = "/path/to/resource";
            const HttpMethod verbOnRequest = HttpMethod.GET;
            const HttpMethod verbOnRoute = HttpMethod.POST;

            var properties = new MockProperties { HttpMethod = verbOnRequest, PathInfo = pathinfo };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verbOnRoute, pathinfo);

            route.Matches(context).ShouldBeFalse();
        }

        [Fact]
        public void route_matches_false_with_different_pathinfo()
        {
            const string pathinfoOnRequest = "/path/to/resource";
            const string pathinfoOnRoute = "/path/ot/resource";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfoOnRequest };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verb, pathinfoOnRoute);

            route.Matches(context).ShouldBeFalse();
        }

        [Fact]
        public void route_matches_false_when_missing_path_params()
        {
            const string pathinfoOnRequest = "/path/to/";
            const string pathinfoOnRoute = "/path/to/[resource]";
            const HttpMethod verb = HttpMethod.GET;

            var properties = new MockProperties { HttpMethod = verb, PathInfo = pathinfoOnRequest };
            var request = MockContext.GetMockRequest(properties);
            var context = MockContext.GetMockContext(request);

            var route = new Route(RouteTesterHelper.StaticMethod, verb, pathinfoOnRoute);

            route.Matches(context).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_false_when_object_compared_to_is_not_route()
        {
            var route1 = new Route(RouteTesterHelper.StaticMethod);
            var route2 = new AltRoute();

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_true_when_routes_use_same_methodinfo()
        {
            var route1 = new Route(RouteTesterHelper.StaticMethod);
            var route2 = new Route(RouteTesterHelper.StaticMethod);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_false_when_routes_use_different_methodinfo()
        {
            var route1 = new Route(RouteTesterHelper.StaticMethod);
            var route2 = new Route(RouteTesterHelper.StaticMethodInfo);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_true_when_routes_use_same_function()
        {
            Func<IHttpContext, IHttpContext> function = context => context;

            var route1 = new Route(function);
            var route2 = new Route(function);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_false_when_routes_use_different_function()
        {
            Func<IHttpContext, IHttpContext> function1 = context => context;
            Func<IHttpContext, IHttpContext> function2 = context => context;

            var route1 = new Route(function1);
            var route2 = new Route(function2);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_false_when_routes_use_different_function_and_methodinfo()
        {
            Func<IHttpContext, IHttpContext> function = context => context;

            var route1 = new Route(function);
            var route2 = new Route(RouteTesterHelper.StaticMethod);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_true_when_routes_use_same_function_and_methodinfo()
        {
            Func<IHttpContext, IHttpContext> function = RouteTesterHelper.StaticMethod;

            var route1 = new Route(function);
            var route2 = new Route(RouteTesterHelper.StaticMethod);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_true_when_httpmethod_is_equivalent()
        {
            var route1 = new Route(RouteTesterHelper.StaticMethod, HttpMethod.POST);
            var route2 = new Route(RouteTesterHelper.StaticMethod);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_false_when_httpmethods_are_different()
        {
            var route1 = new Route(RouteTesterHelper.StaticMethod, HttpMethod.POST);
            var route2 = new Route(RouteTesterHelper.StaticMethod, HttpMethod.DELETE);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_true_when_using_same_path_info()
        {
            const string pathinfo = "/path/to/resource";
            var route1 = new Route(RouteTesterHelper.StaticMethod, pathinfo);
            var route2 = new Route(RouteTesterHelper.StaticMethod, pathinfo);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_false_when_using_different_path_info()
        {
            const string pathinfo1 = "/path/to/resource1";
            const string pathinfo2 = "/path/to/resource2";

            var route1 = new Route(RouteTesterHelper.StaticMethod, pathinfo1);
            var route2 = new Route(RouteTesterHelper.StaticMethod, pathinfo2);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_equality_true_when_using_overlapping_patterns()
        {
            const string pathinfo1 = "/path/to/[resource1]";
            const string pathinfo2 = "/path/to/[resource2]";

            var route1 = new Route(RouteTesterHelper.StaticMethod, pathinfo1);
            var route2 = new Route(RouteTesterHelper.StaticMethod, pathinfo2);

            route1.Equals(route2).ShouldBeTrue();
        }

        [Fact]
        public void route_equality_false_when_not_using_overlapping_patterns()
        {
            const string pathinfo1 = "/path1/to/[resource]";
            const string pathinfo2 = "/path2/to/[resource]";

            var route1 = new Route(RouteTesterHelper.StaticMethod, pathinfo1);
            var route2 = new Route(RouteTesterHelper.StaticMethod, pathinfo2);

            route1.Equals(route2).ShouldBeFalse();
        }

        [Fact]
        public void route_gethashcode_overrides()
        {
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var route = new Route(method);

            route.GetHashCode().Equals(route.ToString().GetHashCode()).ShouldBeTrue();
        }

        [Fact]
        public void route_tostring_overrides_with_function()
        {
            const string pathinfo = "/path/to/resource";
            Func<IHttpContext, IHttpContext> function = context => context;
            var fname = $"{function.Method.ReflectedType.FullName}.{function.Method.Name}";

            var route = new Route(function, HttpMethod.ALL, pathinfo);

            route.ToString().ShouldBe($"{HttpMethod.ALL} {pathinfo} > {fname}");
        }

        [Fact]
        public void route_tostring_overrides_with_methodinfo()
        {
            const string pathinfo = "/path/to/resource";
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            var fname = $"{method.ReflectedType.FullName}.{method.Name}";

            var route = new Route(method, HttpMethod.POST, "/path/to/resource");

            route.ToString().ShouldBe($"{HttpMethod.POST} {pathinfo} > {fname}");
        }

        [Fact]
        public void route_tostring_overrides_with_method()
        {
            const string pathinfo = "/path/to/resource";
            var method = typeof(RouteTesterHelper).GetMethod("ShouldThrowNoExceptions");
            Func<IHttpContext, IHttpContext> function = new RouteTesterHelper().ShouldThrowNoExceptions;
            var fname = $"{method.ReflectedType.FullName}.{method.Name}";

            var route = new Route(function, HttpMethod.GET, pathinfo);

            route.ToString().ShouldBe($"{HttpMethod.GET} {pathinfo} > {fname}");
        }
    }
}
