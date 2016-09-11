using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Util;

namespace Grapevine.Tests.Server.Helpers
{
    public class MethodsToScan
    {
        [RestRoute]
        public void InvalidRoute() { /* method intentionally left blank */ }

        public IHttpContext HasNoAttributes(IHttpContext context) { return context; }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

        [RestRoute(HttpMethod = HttpMethod.DELETE, PathInfo = "/more/stuff")]
        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/stuff/[id]")]
        public IHttpContext HasMultipleAttributes(IHttpContext context) { return context; }
    }

    [RestResource]
    public class TypeWithoutBasePath
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

    }

    [RestResource(BasePath = "/with")]
    public class TypeWithBasePath
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

    }

    public abstract class AbstractToScan
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }
    }

    public interface InterfaceToScan
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        IHttpContext HasOneAttribute(IHttpContext context);
    }

    public class EmptyClassToScan {  /* class body intentionally left blank */ }

    [RestResource(Scope = "ScopeA")]
    public class ClassInScopeA {  /* class body intentionally left blank */ }

    [RestResource(Scope = "ScopeB")]
    public class ClassInScopeB {  /* class body intentionally left blank */ }

    public static class RouteScannerExtensions
    {
        internal static List<string> ExcludedNamespaces(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedNamespaces", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<string>)field?.GetValue(scanner);
        }

        internal static List<string> IncludedNamespaces(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedNamespaces", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<string>)field?.GetValue(scanner);
        }

        internal static List<Type> ExcludedTypes(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Type>)field?.GetValue(scanner);
        }

        internal static List<Type> IncludedTypes(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Type>)field?.GetValue(scanner);
        }

        internal static List<Assembly> ExcludedAssemblies(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedAssemblies", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Assembly>)field?.GetValue(scanner);
        }

        internal static List<Assembly> IncludedAssemblies(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedAssemblies", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Assembly>)field?.GetValue(scanner);
        }

        internal static string GetScope(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_scope", BindingFlags.Instance | BindingFlags.NonPublic);
            return (string)field?.GetValue(scanner);
        }

        internal static bool CheckIsInScope(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsInScope", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { typeof(Type) }, null);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, string nameSpace)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
            return (bool)method.Invoke(scanner, new object[] { nameSpace });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, Assembly assembly)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Assembly) }, null);
            return (bool)method.Invoke(scanner, new object[] { assembly });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Type) }, null);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, string nameSpace)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
            return (bool)method.Invoke(scanner, new object[] { nameSpace });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, Assembly assembly)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Assembly) }, null);
            return (bool)method.Invoke(scanner, new object[] { assembly });
        }

        internal static string PathInfoGenerator(this IRouteScanner scanner, string pathinfo, string basepath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("GeneratePathInfo", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { pathinfo, basepath });
        }

        internal static string BasePathGenerator(this IRouteScanner scanner, string basepath, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("GenerateBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basepath, type });
        }

        internal static string BasePathSanitizer(this IRouteScanner scanner, string basepath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("SanitizeBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basepath });
        }
    }
}
