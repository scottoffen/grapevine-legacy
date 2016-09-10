using System.CodeDom;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Util;

namespace Grapevine.Tests.Server.Attributes.Helpers
{
    public class RestRouteTesterHelper
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
