using System;
using System.Text.RegularExpressions;

namespace Grapevine
{
    public static class StringExtensions
    {
        public static string Capitalize(this String s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                return char.ToUpper(s[0]) + s.Substring(1).ToLower();
            }
            return s;
        }

        public static GroupCollection GrabAll(this String s, string pattern)
        {
            return s.GrabAll(pattern, true);
        }

        public static GroupCollection GrabAll(this String s, string pattern, bool ignoreCase)
        {
            Match match = (ignoreCase) ? Regex.Match(s, pattern) : Regex.Match(s, pattern, RegexOptions.IgnoreCase);
            return match.Groups;
        }

        public static string GrabFirst(this String s, string pattern)
        {
            return s.GrabFirst(pattern, true);
        }

        public static string GrabFirst(this String s, string pattern, bool ignoreCase)
        {
            Match match = (ignoreCase) ? Regex.Match(s, pattern) : Regex.Match(s, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public static bool Matches(this String s, string pattern, bool ignoreCase)
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

        public static bool Matches(this String s, string pattern)
        {
            return s.Matches(pattern, true);
        }
    }

}
