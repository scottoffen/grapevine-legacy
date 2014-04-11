using System;
using System.Text.RegularExpressions;

namespace Grapevine
{
    public static class Extension
    {
        public static bool Matches(this String s, string pattern)
        {
            return ((Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))) ? true : false;
        }
    }
}
