using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Grapevine.Util
{
    public static class PatternParser
    {
        private static readonly Regex ParseForParams = new Regex(@"\[(\w+)\]", RegexOptions.IgnoreCase);

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

        public static Regex GenerateRegEx(string pathinfo)
        {
            if (string.IsNullOrEmpty(pathinfo)) return new Regex(@"^.*$");
            if (pathinfo.StartsWith("^") && IsValidRegex(pathinfo)) return new Regex(pathinfo);

            var pattern = new StringBuilder();
            if (!pathinfo.StartsWith("^")) pattern.Append("^");

            pattern.Append(ParseForParams.IsMatch(pathinfo)
                ? ParseForParams.Replace(pathinfo, "(.+)")
                : pathinfo);

            if (!pathinfo.EndsWith("$")) pattern.Append("$");

            var regex = pattern.ToString();
            if (IsValidRegex(regex)) return new Regex(regex);

            throw new PatternNotParseableToValidRegex($"\tPathInfo:{pathinfo}{Environment.NewLine}\tExpression:{regex}");
        }

        private static bool IsValidRegex(string pattern)
        {
            try
            {
                return (Regex.IsMatch("", pattern)) ? true : true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
