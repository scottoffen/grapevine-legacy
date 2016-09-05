using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Grapevine.Server;
using Grapevine.Util;

namespace Grapevine.Tests.Server.Helpers
{
    public class RouteTesterHelper
    {
        public static bool InstanceMethodInfoHit { get; private set; }
        public static bool StaticMethodInfoHit { get; private set; }
        public static bool InstanceMethodHit { get; private set; }
        public static bool StaticMethodHit { get; private set; }
        public static bool SharedInstanceMethodHit { get; private set; }

        public int InstanceHits { get; private set; }
        private int ContextHits = 0;

        public RouteTesterHelper()
        {
            InstanceHits = 0;
        }

        public void DoesNotReturnContext (IHttpContext context)
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
            if (ContextHits != 0) context.Response.Abort();
            ContextHits++;
            return context;
        }
    }

    public class AltRoute : IRoute
    {
        public string Description { get; set; }
        public bool Enabled { get; }
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
