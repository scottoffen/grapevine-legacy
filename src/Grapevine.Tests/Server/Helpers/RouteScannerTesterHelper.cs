using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;

namespace Grapevine.Tests.Server.Helpers
{
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

        internal static string BasePathSanitizer(this IRouteScanner scanner, string basePath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("SanitizeBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basePath });
        }
    }
}
