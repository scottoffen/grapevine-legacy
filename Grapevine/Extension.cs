using System;
using System.Text.RegularExpressions;
using System.Reflection;
using Grapevine.REST;

namespace Grapevine
{
    public static class Extension
    {
        public static bool Matches(this String s, string pattern)
        {
            return ((Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))) ? true : false;
        }

        public static string Value (this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] info = type.GetMember(en.ToString());

            if ((info != null) && (info.Length > 0))
            {
                object[] attrs = info[0].GetCustomAttributes(typeof(RequestContentTypeValue), false);
                if ((attrs != null) && (attrs.Length > 0))
                {
                    return ((RequestContentTypeValue)attrs[0]).Value;
                }
            }

            return en.ToString();
        }
    }
}
