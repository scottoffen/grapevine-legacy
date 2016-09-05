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

        internal static bool CheckIsExcluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        // TODO: How do I make sure I'm hitting the right overloaded method
        // TODO: How do I make a call using a generic parameter

        internal static bool CheckIsExcluded(this IRouteScanner scanner, string nameSpace)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)method.Invoke(scanner, new object[] { nameSpace });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)method.Invoke(scanner, new object[] { type });
        }
    }
}
