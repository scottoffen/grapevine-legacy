using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace Grapevine.Util
{
    public static class InternalExtensions
    {
        private static readonly Regex CamelCaseInner = new Regex(@"(\P{Ll})(\P{Ll}\p{Ll})", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex CamelCaseOuter = new Regex(@"(\p{Ll})(\P{Ll})", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Returns the string with spaces between inserted in the camel casing.
        /// </summary>
        internal static string ConvertCamelCase(this string s)
        {
            return CamelCaseOuter.Replace(CamelCaseInner.Replace(s, "$1 $2"), "$1 $2");
        }

        /// <summary>
        /// Returns false if the string contains any invalid path characters; doesn't not mean that the string is a path, only that is it might be.
        /// </summary>
        internal static bool IsValidPath(this string s)
        {
            return s.IndexOfAny(Path.GetInvalidPathChars()) == -1;
        }

        /// <summary>
        /// Capitalizes the first letter in the string and lower-cases the remainder
        /// </summary>
        internal static string Capitalize(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return char.ToUpper(s[0]) + s.Substring(1).ToLower();
            }
            return s;
        }

        /// <summary>
        /// Returns the entire GroupCollection matched by the regular expression string pattern provided; case-insensative by default
        /// </summary>
        /// <returns></returns>
        internal static GroupCollection GrabAll(this string s, string pattern, bool ignoreCase = true)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            Match match = (ignoreCase) ? Regex.Match(s, pattern, RegexOptions.IgnoreCase) : Regex.Match(s, pattern);
            return match.Groups;
        }

        /// <summary>
        /// Returns the first group matched by the regular expression string pattern provided; case-insensative by default
        /// </summary>
        internal static string GrabFirst(this string s, string pattern, bool ignoreCase = true)
        {
            Match match = (ignoreCase) ? Regex.Match(s, pattern, RegexOptions.IgnoreCase) : Regex.Match(s, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the string matches  the regular expression string patter provided; case-insensative by default
        /// </summary>
        internal static bool Matches(this string s, string pattern, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
            }
            else
            {
                return Regex.IsMatch(s, pattern);
            }
        }

        /// <summary>
        /// Returns true if this is of type &lt;T&gt;
        /// </summary>
        internal static bool IsA<T>(this object obj)
        {
            return obj is T || obj.GetType().IsInstanceOfType(typeof(T));
        }

        /// <summary>
        /// Returns true if this is NOT of type &lt;T&gt;
        /// </summary>
        internal static bool IsNot<T>(this object obj)
        {
            return !obj.IsA<T>();
        }

        /// <summary>
        /// Shorthand for assembly.GetName().Name
        /// </summary>
        public static string Name(this Assembly s)
        {
            return s.GetName().Name;
        }
    }
}
