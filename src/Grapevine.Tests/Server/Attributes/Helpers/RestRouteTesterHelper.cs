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

    public abstract class TypicalAbstractClass
    {
        public void TypicalNonAbstractMethod()
        {
        }

        public abstract void TypicalAbstractMethod();

        public abstract class TypicalNestedAbstractClass
        {
            public void TypicalNonAbstractMethod()
            {
            }

            public abstract void TypicalAbstractMethod();
        }
    }

    public class TypicalClass
    {
        public void TypicalMethod()
        {
        }
    }

    public interface TypicalInterface
    {
        void TypicalInterfaceMethod();
    }

    public class TypicalInterfaceConcretion : TypicalInterface
    {
        public void TypicalMethod()
        {
        }

        public void TypicalInterfaceMethod()
        {
            throw new System.NotImplementedException();
        }
    }

    public class TypicalAbstractConcretion : TypicalAbstractClass
    {
        public void TypicalMethod()
        {
        }

        public override void TypicalAbstractMethod()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ContainsNestedAbstract
    {
        public abstract class NestedAbstractClass
        {

        }
    }

    //public class OneValidRoute : AbstractRoutes
    //{
    //    public string Stuff { get; set; }

    //    public OneValidRoute()
    //    {
    //        Stuff = string.Empty;
    //    }

    //    [RestRoute]
    //    private IHttpContext PrivateMethod(IHttpContext context)
    //    {
    //        return context;
    //    }

    //    [RestRoute]
    //    protected IHttpContext ProtectedMethod(IHttpContext context)
    //    {
    //        return PrivateMethod(context);
    //    }

    //    [RestRoute]
    //    internal IHttpContext InternalMethod(IHttpContext context)
    //    {
    //        return context;
    //    }

    //    [RestRoute]
    //    public IHttpContext TheValidRoute(IHttpContext context)
    //    {
    //        return context;
    //    }

    //    [RestRoute]
    //    public sealed override IHttpContext AbstractMethod(IHttpContext context)
    //    {
    //        return context;
    //    }
    //}
}
