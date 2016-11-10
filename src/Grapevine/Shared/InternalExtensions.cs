using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Grapevine.Shared
{
    public static class InternalExtensions
    {
        private static readonly Regex CamelCaseInner = new Regex(@"(\P{Ll})(\P{Ll}\p{Ll})", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex CamelCaseOuter = new Regex(@"(\p{Ll})(\P{Ll})", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly ConcurrentDictionary<Type, bool> CanDispose = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        /// Returns the string with spaces between inserted in the camel casing.
        /// </summary>
        internal static string ConvertCamelCase(this string s)
        {
            return CamelCaseOuter.Replace(CamelCaseInner.Replace(s, "$1 $2"), "$1 $2");
        }

        internal static bool Implements<T>(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(T));
        }

        /// <summary>
        /// Tries to dispose of an object of unknown type
        /// </summary>
        /// <param name="obj"></param>
        internal static bool TryDisposing(this object obj)
        {
            var type = obj.GetType();
            if (!type.Implements<IDisposable>()) return true;

            if (!CanDispose.ContainsKey(type))
            {
                var method = type.GetMethod("Dispose", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
                CanDispose.TryAdd(type, method != null);
            }

            if (CanDispose[type]) ((IDisposable)obj).Dispose();
            return true;
        }

        /// <summary>
        /// Returns the section of the guid following the last dash
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>string</returns>
        internal static string Truncate(this Guid guid)
        {
            return guid.ToString().Split('-').Last();
        }
    }
}
