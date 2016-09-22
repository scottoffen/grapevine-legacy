using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Grapevine.Shared
{
    /// <summary>
    /// Provides methods for parsing PathInfo patterns
    /// </summary>
    public static class PatternParser
    {
        private static readonly Regex ParseForParams = new Regex(@"\[(\w+)\]", RegexOptions.IgnoreCase);

        /// <summary>
        /// Returns a list of keys parsed from the specified PathInfo pattern
        /// </summary>
        /// <param name="pathinfo"></param>
        /// <returns>List&lt;string&gt;</returns>
        public static List<string> GeneratePatternKeys(string pathinfo)
        {
            var captured = new List<string>();
            if (pathinfo == null) return captured;

            foreach (var val in from Match match in ParseForParams.Matches(pathinfo) select match.Groups[1].Value)
            {
                if (captured.Contains(val)) throw new ArgumentException($"Repeat parameters in path info expression {pathinfo}");
                captured.Add(val);
            }

            return captured;
        }

        /// <summary>
        /// Returns a Regex object that matches the specified PathInfo pattern
        /// </summary>
        /// <param name="pathinfo"></param>
        /// <returns>RegExp</returns>
        public static Regex GenerateRegEx(string pathinfo)
        {
            if (string.IsNullOrEmpty(pathinfo)) return new Regex(@"^.*$");
            if (pathinfo.StartsWith("^")) return new Regex(pathinfo);

            var pattern = new StringBuilder("^");

            pattern.Append(ParseForParams.IsMatch(pathinfo)
                ? ParseForParams.Replace(pathinfo, "(.+)")
                : pathinfo);

            if (!pathinfo.EndsWith("$")) pattern.Append("$");

            return new Regex(pattern.ToString());
        }
    }
}
