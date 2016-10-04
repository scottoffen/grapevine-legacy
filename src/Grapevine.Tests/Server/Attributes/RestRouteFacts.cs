using System;
using NSubstitute;
using System.Linq;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server.Attributes
{
    public class RestRouteFacts
    {
        public class ExtensionMethods
        {
            public class GetRouteAttributesMethod
            {
                [Fact]
                public void ReturnsEmptyListWhenAttributeNotPresent()
                {
                    var attributes = typeof(TestClass).GetMethod("IsEligibleButNoAttribute").GetRouteAttributes();
                    attributes.ShouldNotBeNull();
                    attributes.Count().ShouldBe(0);
                }

                [Fact]
                public void NoArgumentsGetsDefaultProperties()
                {
                    var method = typeof(RouteTestMethods).GetMethod("RouteHasNoArgs");
                    var attrs = method.GetRouteAttributes().ToList();

                    attrs.Count.ShouldBe(1);
                    attrs[0].HttpMethod.ShouldBe(HttpMethod.ALL);
                    attrs[0].PathInfo.Equals(string.Empty).ShouldBeTrue();
                }

                [Fact]
                public void HttpMethodArgumentOnlyGetsDefaultPathInfo()
                {
                    var method = typeof(RouteTestMethods).GetMethod("RouteHasHttpMethodOnly");
                    var attrs = method.GetRouteAttributes().ToList();

                    attrs.Count.ShouldBe(1);
                    attrs[0].HttpMethod.ShouldBe(HttpMethod.DELETE);
                    attrs[0].PathInfo.Equals(string.Empty).ShouldBeTrue();
                }

                [Fact]
                public void PathInfoArgumentOnlyGetsDefaultHttpMethod()
                {
                    var method = typeof(RouteTestMethods).GetMethod("RouteHasPathInfoOnly");
                    var attrs = method.GetRouteAttributes().ToList();

                    attrs.Count.ShouldBe(1);
                    attrs[0].HttpMethod.ShouldBe(HttpMethod.ALL);
                    attrs[0].PathInfo.Equals("/some/path").ShouldBeTrue();
                }

                [Fact]
                public void BothArgumentsGetSetCorrectly()
                {
                    var method = typeof(RouteTestMethods).GetMethod("RouteHasBothArgs");
                    var attrs = method.GetRouteAttributes().ToList();

                    attrs.Count.ShouldBe(1);
                    attrs[0].HttpMethod.ShouldBe(HttpMethod.POST);
                    attrs[0].PathInfo.Equals("/some/other/path").ShouldBeTrue();
                }

                [Fact]
                public void CanHaveMultipleAttributes()
                {
                    var method = typeof(RouteTestMethods).GetMethod("RouteHasMultipleAttrs");
                    var attrs = method.GetRouteAttributes().ToList();

                    attrs.Count.ShouldBe(2);
                    attrs[0].HttpMethod.ShouldBe(HttpMethod.GET);
                    attrs[0].PathInfo.Equals("/index.html").ShouldBeTrue();
                    attrs[1].HttpMethod.ShouldBe(HttpMethod.HEAD);
                    attrs[1].PathInfo.Equals("/index").ShouldBeTrue();
                }
            }

            public class HasParameterlessConstructorMethod
            {
                [Fact]
                public void ReturnsTrueWithImplicitConstructor()
                {
                    typeof(ImplicitConstructor).HasParameterlessConstructor().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWithExplicitConstructor()
                {
                    typeof(ExplicitConstructor).HasParameterlessConstructor().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWithMultipleConstructors()
                {
                    typeof(MultipleConstructor).HasParameterlessConstructor().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenNoParameterlessConstructorExists()
                {
                    typeof(NoParameterlessConstructor).HasParameterlessConstructor().ShouldBeFalse();
                }
            }

            public class CanInvokeMethod
            {
                [Fact]
                public void ReturnsTrueForStaticMethods()
                {
                    typeof(TestClass).GetMethod("TestStaticMethod").CanInvoke().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenClassIsAbstract()
                {
                    typeof(TestAbstract).GetMethod("TestAbstractMethod").CanInvoke().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenReflectedTypeIsNull()
                {
                    var method = Substitute.For<MethodInfo>();
                    method.ReflectedType.ReturnsNull();

                    method.IsStatic.ShouldBeFalse();
                    method.IsAbstract.ShouldBeFalse();
                    method.CanInvoke().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenReflectedTypeIsNotAClass()
                {
                    typeof(TestInterface).GetMethod("TestInterfaceMethod").CanInvoke().ShouldBeFalse();
                    typeof(TestStruct).GetMethod("TestStructMethod").CanInvoke().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenReflectedTypeIsAbstract()
                {
                    typeof(TestAbstract).GetMethod("TestVirtualMethod").CanInvoke().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueOnInvokableMethod()
                {
                    typeof(TestClass).GetMethod("TestMethod").CanInvoke().ShouldBeTrue();
                }
            }

            public class IsRestRouteEligibleMethod
            {
                [Fact]
                public void ReturnsFalseWhenMethodInfoIsNull()
                {
                    MethodInfo method = null;
                    Action<MethodInfo> action = info => info.IsRestRouteEligible();
                    Should.Throw<ArgumentNullException>(() => action(method));
                }

                [Fact]
                public void ReturnsFalseWhenMethodInfoIsNotInvokable()
                {
                    typeof(TestAbstract).GetMethod("TestAbstractMethod").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenMethodInfoReflectedTypeHasNoParameterlessConstructor()
                {
                    typeof(NoParameterlessConstructor).GetMethod("TestMethod").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenMethodIsSpecialName()
                {
                    typeof(TestClass).GetMethod("get_TestProperty").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenReturnTypeIsNotIHttpContext()
                {
                    typeof(TestClass).GetMethod("HasNoReturnValue").IsRestRouteEligible().ShouldBeFalse();
                    typeof(TestClass).GetMethod("ReturnValueIsWrongType").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenMethodAcceptsMoreOrLessThanOneArgument()
                {
                    typeof(TestClass).GetMethod("TakesZeroArgs").IsRestRouteEligible().ShouldBeFalse();
                    typeof(TestClass).GetMethod("TakesTwoArgs").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenFirstArgumentIsNotIHttpContext()
                {
                    typeof(TestClass).GetMethod("TakesWrongArgs").IsRestRouteEligible().ShouldBeFalse();
                }

                [Fact]
                public void ThrowsAggregateExceptionWhenThrowExceptionsIsTrue()
                {
                    Should.Throw<InvalidRouteMethodExceptions>(
                        () => typeof(TestClass).GetMethod("TakesWrongArgs").IsRestRouteEligible(true));
                }

                [Fact]
                public void ReturnsTrueWhenMethodInfoIsEligible()
                {
                    typeof(TestClass).GetMethod("ValidRoute").IsRestRouteEligible().ShouldBeTrue();
                }
            }

            public class IsRestRouteMethod
            {
                [Fact]
                public void ReturnsFalseWhenMethodIsNotEligible()
                {
                    typeof(TestClass).GetMethod("HasAttributeButIsNotEligible").IsRestRoute().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenAttributeIsNotPresent()
                {
                    typeof(TestClass).GetMethod("IsEligibleButNoAttribute").IsRestRoute().ShouldBeFalse();
                }

                [Fact]
                public void ThrowsExceptionWhenThrowExceptionsIsTrue()
                {
                    Should.Throw<InvalidRouteMethodExceptions>(() => typeof(TestClass).GetMethod("HasAttributeButIsNotEligible").IsRestRoute(true));
                    Should.Throw<InvalidRouteMethodExceptions>(() => typeof(TestClass).GetMethod("IsEligibleButNoAttribute").IsRestRoute(true));
                }

                [Fact]
                public void ReturnsTrueWhenAttributeIsPresentAndMethodInfoIsEligible()
                {

                }
            }
        }
    }

    /* Classes and methods used in testing */

    public class RouteTestMethods
    {
        [RestRoute]
        public IHttpContext RouteHasNoArgs(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.DELETE)]
        public IHttpContext RouteHasHttpMethodOnly(IHttpContext context)
        {
            return context;
        }

        [RestRoute(PathInfo = "/some/path")]
        public IHttpContext RouteHasPathInfoOnly(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/some/other/path")]
        public IHttpContext RouteHasBothArgs(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/index.html")]
        [RestRoute(HttpMethod = HttpMethod.HEAD, PathInfo = "/index")]
        public IHttpContext RouteHasMultipleAttrs(IHttpContext context)
        {
            return context;
        }
    }

    public class ImplicitConstructor
    {
        public int Value { get; set; }
    }

    public class ExplicitConstructor
    {
        public int Value { get; set; }

        public ExplicitConstructor()
        {
            Value = 30;
        }
    }

    public class MultipleConstructor
    {
        public int Value { get; set; }

        public MultipleConstructor()
        {
            Value = 12;
        }

        public MultipleConstructor(int i)
        {
            Value = i;
        }
    }

    public class NoParameterlessConstructor
    {
        public int Value { get; set; }

        public NoParameterlessConstructor(int i)
        {
            Value = i;
        }

        public void TestMethod() { /* intentionally left blank */ }
    }

    public interface TestInterface
    {
        void TestInterfaceMethod();
    }

    public struct TestStruct
    {
        public int Id { get; set; }

        public void TestStructMethod() { /* intentionally left blank */ }
    }

    public abstract class TestAbstract
    {
        public abstract void TestAbstractMethod();

        public virtual void TestVirtualMethod() { /* intentionally left blank */ }
    }

    public class TestClass
    {
        public string TestProperty { get; set; }

        public static void TestStaticMethod() { /* intentionally left blank */ }

        public void TestMethod() { /* intentionally left blank */ }

        public IHttpContext TakesZeroArgs()
        {
            return null;
        }

        [RestRoute]
        public IHttpContext ValidRoute(IHttpContext context)
        {
            return context;
        }

        public IHttpContext TakesTwoArgs(IHttpContext context, int y)
        {
            return context;
        }

        public IHttpContext TakesWrongArgs(int y)
        {
            return null;
        }

        public void HasNoReturnValue(IHttpContext context) { /* intentionally left blank */ }

        public int ReturnValueIsWrongType(IHttpContext context)
        {
            return 1;
        }

        [RestRoute]
        public void HasAttributeButIsNotEligible() { /* intentionally left blank */ }

        public IHttpContext IsEligibleButNoAttribute(IHttpContext context)
        {
            return context;
        }
    }
}
