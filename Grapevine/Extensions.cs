using System;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Grapevine
{
    public static class Extensions
    {
        public static bool Matches(this String s, string pattern)
        {
            return ((Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))) ? true : false;
        }
    }
}