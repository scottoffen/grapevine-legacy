using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Server.Attributes;

namespace Grapevine.Tests.Server.Helpers
{
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

        internal static string BasePathSanitizer(this IRouteScanner scanner, string basepath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("SanitizeBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basepath });
        }
    }
}
