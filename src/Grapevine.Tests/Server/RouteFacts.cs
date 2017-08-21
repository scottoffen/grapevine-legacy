using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Shared;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteFacts
    {
        public class ConstructorMethods
        {
            [Fact]
            public void MethodInfoOnlyCtor()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var route = new Route(method);

                route.Enabled.ShouldBeTrue();
                route.HttpMethod.ShouldBe(HttpMethod.ALL);
                route.PathInfo.ShouldBe(string.Empty);
                route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
                route.Description.ShouldBe($"{HttpMethod.ALL}  > {method.ReflectedType.FullName}.{method.Name}");
            }

            [Fact]
            public void MethodInfoAndHttpMethodCtor()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var route = new Route(method, HttpMethod.GET);

                route.HttpMethod.ShouldBe(HttpMethod.GET);
                route.PathInfo.ShouldBe(string.Empty);
                route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
                route.Description.ShouldBe($"{HttpMethod.GET}  > {method.ReflectedType.FullName}.{method.Name}");
            }

            [Fact]
            public void MethodInfoAndPathInfoCtor()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var pathinfo = "/path/to/resource";
                var pattern = "^/path/to/resource$";

                var route = new Route(method, pathinfo);

                route.HttpMethod.ShouldBe(HttpMethod.ALL);
                route.PathInfo.ShouldBe(pathinfo);
                route.PathInfoPattern.ToString().ShouldBe(pattern);
                route.Description.ShouldBe($"{HttpMethod.ALL} {pathinfo} > {method.ReflectedType.FullName}.{method.Name}");
            }

            [Fact]
            public void MethodInfoHttpMethodAndPathInfo()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                const string pathinfo = "/path/[param1]/[param2]";
                const string pattern = "^/path/([^/]+)/([^/]+)$";

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
            public void ThrowsExceptionIfFunctionIsNull()
            {
                Func<IHttpContext, IHttpContext> func = null;
                Should.Throw<ArgumentNullException>(() => { var route = new Route(func); });
            }

            [Fact]
            public void FunctionOnlyCtor()
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
            public void FunctionAndHttpMethodCtor()
            {
                Func<IHttpContext, IHttpContext> function = context => context;
                var route = new Route(function, HttpMethod.GET);

                route.Function.ShouldBe(function);
                route.HttpMethod.ShouldBe(HttpMethod.GET);
                route.PathInfo.ShouldBe(string.Empty);
                route.PathInfoPattern.ToString().ShouldBe(@"^.*$");
            }

            [Fact]
            public void FunctionAndPathInfoCtor()
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
            public void FunctionHttpMethodAndPathInfoCtor()
            {
                Func<IHttpContext, IHttpContext> function = context => context;
                const string pathinfo = "/path/[param1]/[param2]";
                const string pattern = "^/path/([^/]+)/([^/]+)$";

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
        }

        public class EnablingMethods
        {
            [Fact]
            public void RouteDefaultsToEnabled()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var route = new Route(method);
                route.Enabled.ShouldBeTrue();
            }

            [Fact]
            public void DisableEnabledRoute()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var route = new Route(method);

                route.Disable();
                route.Enabled.ShouldBeFalse();
            }

            [Fact]
            public void EnableDisabledRoute()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                var route = new Route(method);

                route.Disable();
                route.Enable();

                route.Enabled.ShouldBeTrue();
            }
        }

        public class InvokeMethod
        {
            [Fact]
            public void DoesNotInvokeDisabledRoute()
            {
                var executed = false;
                Func<IHttpContext, IHttpContext> function = context =>
                {
                    executed = true;
                    return context;
                };

                var route = new Route(function);
                route.Disable();
                route.Invoke(Mocks.HttpContext());

                executed.ShouldBeFalse();
            }

            [Fact]
            public void InvokesFunction()
            {
                const string expected = "1234";
                var actual = string.Empty;

                Func<IHttpContext, IHttpContext> function = context =>
                {
                    actual = context.Request.PathParameters["id"];
                    return context;
                };

                var properties = new Dictionary<string, object> { { "PathInfo", $"/id/{expected}" } };
                var route = new Route(function, "/id/[id]");

                route.Invoke(Mocks.HttpContext(properties));

                actual.Equals(expected).ShouldBeTrue();
            }

            [Fact]
            public void InvokesInstanceMethod()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("InstanceMethodInfo");
                var route = new Route(method);

                TestInvokeRoutes.InstanceMethodInfoHit.ShouldBeFalse();

                route.Invoke(Mocks.HttpContext());

                TestInvokeRoutes.InstanceMethodInfoHit.ShouldBeTrue();
            }

            [Fact]
            public void RoutesDoNotShareObjectContext()
            {
                var method1 = typeof(TestInvokeRoutes).GetMethod("ContextNotShared");
                var method2 = typeof(TestInvokeRoutes).GetMethod("ContextNotShared");

                var route1 = new Route(method1);
                var route2 = new Route(method2);

                var context = Mocks.HttpContext();

                route1.Invoke(context);
                context.Response.DidNotReceive().Abort();

                route2.Invoke(context);
                context.Response.DidNotReceive().Abort();
            }

            [Fact]
            public void InvokesStaticMethodInfo()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("StaticMethodInfo");
                var route = new Route(method);

                TestInvokeRoutes.StaticMethodInfoHit.ShouldBeFalse();

                route.Invoke(Mocks.HttpContext());

                TestInvokeRoutes.StaticMethodInfoHit.ShouldBeTrue();
            }

            [Fact]
            public void InvokesStaticMethod()
            {
                var route = new Route(TestInvokeRoutes.StaticMethod);

                TestInvokeRoutes.StaticMethodHit.ShouldBeFalse();

                route.Invoke(Mocks.HttpContext());

                TestInvokeRoutes.StaticMethodHit.ShouldBeTrue();
            }

            [Fact]
            public void InvokesSharedInstanceMethod()
            {
                var instance = new TestInvokeRoutes();
                var route = new Route(instance.InstanceMethod);

                TestInvokeRoutes.InstanceMethodHit.ShouldBeFalse();

                route.Invoke(Mocks.HttpContext());

                TestInvokeRoutes.InstanceMethodHit.ShouldBeTrue();
                instance.InstanceHits.ShouldBe(1);

                route.Invoke(Mocks.HttpContext());
                instance.InstanceHits.ShouldBe(2);
            }

            [Fact]
            public void InvokesSharedAndNonSharedInstanceMethod()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("SharedInstanceMethod");
                var instance = new TestInvokeRoutes();

                // These use the same method and share an instance
                var route1 = new Route(instance.SharedInstanceMethod);
                var route2 = new Route(instance.SharedInstanceMethod);

                // This uses the same method, but should be using a different instance
                var route3 = new Route(method);

                TestInvokeRoutes.SharedInstanceMethodHit.ShouldBeFalse();

                route1.Invoke(Mocks.HttpContext());

                TestInvokeRoutes.SharedInstanceMethodHit.ShouldBeTrue();
                instance.InstanceHits.ShouldBe(1);

                route2.Invoke(Mocks.HttpContext());
                instance.InstanceHits.ShouldBe(2);

                route3.Invoke(Mocks.HttpContext());
                instance.InstanceHits.ShouldBe(2);

                route1.Invoke(Mocks.HttpContext());
                instance.InstanceHits.ShouldBe(3);
            }

            [Fact]
            public void DisposesWhenExceptionIsThrown()
            {
                var method = typeof(IsDisposeable).GetMethod("TestDisposing");
                var route = new Route(method);

                IsDisposeable.WasDisposed.ShouldBeFalse();

                Should.Throw<Exception>(() => route.Invoke(Mocks.HttpContext()));

                IsDisposeable.WasDisposed.ShouldBeTrue();
            }

            public class IsDisposeable : IDisposable
            {
                public static bool WasDisposed = false;

                public IHttpContext TestDisposing(IHttpContext context)
                {
                    throw new Exception();
                }

                public void Dispose()
                {
                    WasDisposed = true;
                }
            }
        }

        public class MatchesMethod
        {
            [Fact]
            public void ReturnsTrueWhenExactMatch()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verb = HttpMethod.GET;

                var properties = new Dictionary<string, object> {{"HttpMethod", verb}, {"PathInfo", pathinfo}};
                var context = Mocks.HttpContext(properties);
                var route = new Route(TestInvokeRoutes.StaticMethod, verb, pathinfo);

                route.Matches(context).ShouldBeTrue();
            }

            [Fact]
            public void ReturnsTrueWhenHttpMethodIsEquivalent()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verb = HttpMethod.POST;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfo } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, pathinfo);

                route.Matches(context).ShouldBeTrue();
            }

            [Fact]
            public void ReturnsTrueWhenPathInfoIsWildcard()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verb = HttpMethod.GET;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfo } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, verb);

                route.Matches(context).ShouldBeTrue();
            }

            [Fact]
            public void ReturnsTrueWhenHttpMethodIsEquivalentAndPathInfoIsWildcard()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verb = HttpMethod.DELETE;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfo } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod);

                route.Matches(context).ShouldBeTrue();
            }

            [Fact]
            public void ReturnsTrueWithPathParams()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verb = HttpMethod.GET;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfo } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, verb, @"/path/to/[something]");

                route.Matches(context).ShouldBeTrue();
            }

            [Fact]
            public void ReturnsFalseWhenHttpMethodIsDifferent()
            {
                const string pathinfo = "/path/to/resource";
                const HttpMethod verbOnRequest = HttpMethod.GET;
                const HttpMethod verbOnRoute = HttpMethod.POST;

                var properties = new Dictionary<string, object> { { "HttpMethod", verbOnRequest }, { "PathInfo", pathinfo } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, verbOnRoute, pathinfo);

                route.Matches(context).ShouldBeFalse();
            }

            [Fact]
            public void ReturnsFalseWhenPathInfoIsDifferent()
            {
                const string pathinfoOnRequest = "/path/to/resource";
                const string pathinfoOnRoute = "/path/ot/resource";
                const HttpMethod verb = HttpMethod.GET;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfoOnRequest } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, verb, pathinfoOnRoute);

                route.Matches(context).ShouldBeFalse();
            }

            [Fact]
            public void ReturnsFalseWhenMissingPathParams()
            {
                const string pathinfoOnRequest = "/path/to/";
                const string pathinfoOnRoute = "/path/to/[resource]";
                const HttpMethod verb = HttpMethod.GET;

                var properties = new Dictionary<string, object> { { "HttpMethod", verb }, { "PathInfo", pathinfoOnRequest } };
                var context = Mocks.HttpContext(properties);

                var route = new Route(TestInvokeRoutes.StaticMethod, verb, pathinfoOnRoute);

                route.Matches(context).ShouldBeFalse();
            }
        }

        public class ConvertMethodToFuncMethod
        {
            [Fact]
            public void ThrowsExcpetionWhenMethodInfoIsNull()
            {
                MethodInfo method = null;
                Should.Throw<ArgumentNullException>(() => { Route.ConvertMethodToFunc(method); });
            }

            [Fact]
            public void ThrowsExceptionWhenMethodInfoReturnTypeIsNotIHttpContext()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("DoesNotReturnContext");
                Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(method); });
            }

            [Fact]
            public void ThrowsExceptionWhenMethodInfoNumberOfArgsIsNotOne()
            {
                var methodA = typeof(TestInvokeRoutes).GetMethod("MethodTakesZeroArgs");
                var methodB = typeof(TestInvokeRoutes).GetMethod("MethodTakesTwoArgs");

                Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(methodA); });
                Should.Throw<InvalidRouteMethodExceptions>(() => { Route.ConvertMethodToFunc(methodB); });
            }

            [Fact]
            public void ThrowsExceptionWhenMethodInfoFirstArgumentIsNotIHttpContext()
            {
                var method = typeof(TestInvokeRoutes).GetMethod("DoesNotTakeContextArg");
                Should.Throw<InvalidRouteMethodExceptions>(() => { var route = new Route(method); });
            }
        }

        public class OverriddenMethods
        {
            public class EqualsMethod
            {
                [Fact]
                public void ReturnsFalseWhenComparedToNonRoute()
                {
                    var route1 = new Route(TestInvokeRoutes.StaticMethod);
                    var route2 = new AlternateRouteImpl();

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenRoutesUseSameMethodInfoWithDefaults()
                {
                    var route1 = new Route(TestInvokeRoutes.StaticMethod);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenRoutesUseDifferentMethodInfo()
                {
                    var route1 = new Route(TestInvokeRoutes.StaticMethod);
                    var route2 = new Route(TestInvokeRoutes.StaticMethodInfo);

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenRoutesUseSameFunctionWithDefaults()
                {
                    Func<IHttpContext, IHttpContext> function = context => context;

                    var route1 = new Route(function);
                    var route2 = new Route(function);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenRoutesUseDifferentFunction()
                {
                    Func<IHttpContext, IHttpContext> function1 = context => context;
                    Func<IHttpContext, IHttpContext> function2 = context => context;

                    var route1 = new Route(function1);
                    var route2 = new Route(function2);

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenRoutesUseDifferentFunctionAndMethodInfo()
                {
                    Func<IHttpContext, IHttpContext> function = context => context;

                    var route1 = new Route(function);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod);

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenRoutesUseSameFunctionAndMethodInfo()
                {
                    Func<IHttpContext, IHttpContext> function = TestInvokeRoutes.StaticMethod;

                    var route1 = new Route(function);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenHttpMethodIsEquivalent()
                {
                    var route1 = new Route(TestInvokeRoutes.StaticMethod, HttpMethod.POST);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenHttpMethodsAreDifferent()
                {
                    var route1 = new Route(TestInvokeRoutes.StaticMethod, HttpMethod.POST);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod, HttpMethod.DELETE);

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenUsingSamePathInfo()
                {
                    const string pathinfo = "/path/to/resource";
                    var route1 = new Route(TestInvokeRoutes.StaticMethod, pathinfo);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod, pathinfo);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenUsingDifferentPathInfo()
                {
                    const string pathinfo1 = "/path/to/resource1";
                    const string pathinfo2 = "/path/to/resource2";

                    var route1 = new Route(TestInvokeRoutes.StaticMethod, pathinfo1);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod, pathinfo2);

                    route1.Equals(route2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenRoutesUseOverlappingPatterns()
                {
                    const string pathinfo1 = "/path/to/[resource1]";
                    const string pathinfo2 = "/path/to/[resource2]";

                    var route1 = new Route(TestInvokeRoutes.StaticMethod, pathinfo1);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod, pathinfo2);

                    route1.Equals(route2).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenRouteUseNonOverlappingPatterns()
                {
                    const string pathinfo1 = "/path1/to/[resource]";
                    const string pathinfo2 = "/path2/to/[resource]";

                    var route1 = new Route(TestInvokeRoutes.StaticMethod, pathinfo1);
                    var route2 = new Route(TestInvokeRoutes.StaticMethod, pathinfo2);

                    route1.Equals(route2).ShouldBeFalse();
                }
            }

            public class GetHashCodeMethod
            {
                [Fact]
                public void Overrides()
                {
                    var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                    var route = new Route(method);

                    route.GetHashCode().Equals(route.ToString().GetHashCode()).ShouldBeTrue();
                }
            }

            public class ToStringMethod
            {
                [Fact]
                public void OverridesWithFunction()
                {
                    const string pathinfo = "/path/to/resource";
                    Func<IHttpContext, IHttpContext> function = context => context;
                    var fname = $"{function.Method.ReflectedType.FullName}.{function.Method.Name}";

                    var route = new Route(function, HttpMethod.ALL, pathinfo);

                    route.ToString().ShouldBe($"{HttpMethod.ALL} {pathinfo} > {fname}");
                }

                [Fact]
                public void OverridesWithMethodInfo()
                {
                    const string pathinfo = "/path/to/resource";
                    var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                    var fname = $"{method.ReflectedType.FullName}.{method.Name}";

                    var route = new Route(method, HttpMethod.POST, "/path/to/resource");

                    route.ToString().ShouldBe($"{HttpMethod.POST} {pathinfo} > {fname}");
                }

                [Fact]
                public void OverridesWithMethod()
                {
                    const string pathinfo = "/path/to/resource";
                    var method = typeof(TestInvokeRoutes).GetMethod("ShouldThrowNoExceptions");
                    Func<IHttpContext, IHttpContext> function = new TestInvokeRoutes().ShouldThrowNoExceptions;
                    var fname = $"{method.ReflectedType.FullName}.{method.Name}";

                    var route = new Route(function, HttpMethod.GET, pathinfo);

                    route.ToString().ShouldBe($"{HttpMethod.GET} {pathinfo} > {fname}");
                }
            }
        }
    }

    public class TestInvokeRoutes
    {
        public static bool InstanceMethodInfoHit { get; private set; }
        public static bool StaticMethodInfoHit { get; private set; }
        public static bool InstanceMethodHit { get; private set; }
        public static bool StaticMethodHit { get; private set; }
        public static bool SharedInstanceMethodHit { get; private set; }

        public int InstanceHits { get; private set; }
        private int _contextHits;

        public TestInvokeRoutes()
        {
            InstanceHits = 0;
        }

        public void DoesNotReturnContext(IHttpContext context)
        {
            /* This method intentionally left blank */
        }

        public IHttpContext DoesNotTakeContextArg(string arg1)
        {
            return null;
        }

        public IHttpContext MethodTakesZeroArgs()
        {
            return null;
        }

        public IHttpContext MethodTakesTwoArgs(IHttpContext context, int y)
        {
            return context;
        }

        public IHttpContext ShouldThrowNoExceptions(IHttpContext context)
        {
            return context;
        }

        public IHttpContext InstanceMethodInfo(IHttpContext context)
        {
            InstanceMethodInfoHit = true;
            return context;
        }

        public static IHttpContext StaticMethodInfo(IHttpContext context)
        {
            StaticMethodInfoHit = true;
            return context;
        }

        public IHttpContext InstanceMethod(IHttpContext context)
        {
            InstanceMethodHit = true;
            InstanceHits++;
            return context;
        }

        public static IHttpContext StaticMethod(IHttpContext context)
        {
            StaticMethodHit = true;
            return context;
        }

        public IHttpContext SharedInstanceMethod(IHttpContext context)
        {
            SharedInstanceMethodHit = true;
            InstanceHits++;
            return context;
        }

        public IHttpContext ContextNotShared(IHttpContext context)
        {
            if (_contextHits != 0) context.Response.Abort();
            _contextHits++;
            return context;
        }
    }

    public class AlternateRouteImpl : IRoute
    {
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public Func<IHttpContext, IHttpContext> Function { get; }
        public HttpMethod HttpMethod { get; }
        public string Name { get; }
        public string PathInfo { get; }
        public Regex PathInfoPattern { get; }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public IHttpContext Invoke(IHttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public bool Matches(IHttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public IRoute MatchOn(string header, Regex pattern)
        {
            throw new NotImplementedException();
        }
    }

    public static class RouteExtensions
    {
        internal static List<string> GetPatternKeys(this Route route)
        {
            var memberInfo = route.GetType();
            var field = memberInfo?.GetField("PatternKeys", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<string>)field?.GetValue(route);
        }
    }
}
