using System;
using System.Text.RegularExpressions;

namespace Grapevine
{
    public static class Extensions
    {
        /// <summary>
        /// Capitalizes the first letter in the string and lower-cases the remainder
        /// </summary>
        public static string Capitalize(this String s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                return char.ToUpper(s[0]) + s.Substring(1).ToLower();
            }
            return s;
        }

        /// <summary>
        /// Returns the entire GroupCollection matched by the regular expression string pattern provided; case-insensative by default
        /// </summary>
        /// <returns></returns>
        public static GroupCollection GrabAll(this String s, string pattern, bool ignoreCase = true)
        {
            Match match = (ignoreCase) ? Regex.Match(s, pattern, RegexOptions.IgnoreCase) : Regex.Match(s, pattern);
            return match.Groups;
        }

        /// <summary>
        /// Returns the first group matched by the regular expression string pattern provided; case-insensative by default
        /// </summary>
        public static string GrabFirst(this String s, string pattern, bool ignoreCase = true)
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
        public static bool Matches(this String s, string pattern, bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                return ((Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))) ? true : false;
            }
            else
            {
                return (Regex.IsMatch(s, pattern)) ? true : false;
            }
        }

        /// <summary>
        /// Returns true if this is of type &lt;T&gt;
        /// </summary>
        public static bool IsA<T>(this object obj)
        {
            return (obj is T);
        }

        /// <summary>
        /// Returns true if this is NOT of type &lt;T&gt;
        /// </summary>
        public static bool IsNot<T>(this object obj)
        {
            return !(obj is T);
        }
    }
}
