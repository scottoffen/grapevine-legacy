using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Grapevine
{
    public static class Extension
    {
        public static bool Matches(this String s, string pattern)
        {
            return ((System.Text.RegularExpressions.Regex.IsMatch(s, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))) ? true : false;
        }
    }
}
