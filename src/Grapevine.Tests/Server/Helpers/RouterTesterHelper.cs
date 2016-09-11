using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.TestAssembly;

namespace Grapevine.Tests.Server.Helpers
{
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
        internal static IList<IRoute> GetRoutingTable(this Router router)
        {
            var memberInfo = router.GetType();
            var field = memberInfo?.GetField("_routingTable", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IList<IRoute>)field?.GetValue(router);
        }

        internal static void AddRouteToTable(this Router router, IRoute route)
        {
            var memberInfo = router.GetType();
            var method = memberInfo?.GetMethod("AddToRoutingTable", BindingFlags.Instance | BindingFlags.NonPublic, null, new []{typeof(IRoute)}, null);

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
