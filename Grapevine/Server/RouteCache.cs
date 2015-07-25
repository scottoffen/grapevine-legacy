using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Grapevine.Server
{
    /// <summary>
    /// Externally, a route is a decorated method. Internally, a route is a
    /// regular expression that resolves to a method.
    /// All data is thread-safe and immutable
    /// </summary>
    internal class RouteCache
    {
        #region Constructors

        internal RouteCache(MethodInfo methodInfo, Regex regex, string method)
        {
            this.MethodInfo = methodInfo;
            this.Regex = regex;
            this.Method = method;
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// Method attribute (GET/POST/etc)
        /// </summary>
        public readonly string Method;

        /// <summary>
        /// Method to call, from reflection.
        /// </summary>
        public readonly MethodInfo MethodInfo;

        /// <summary>
        /// PathInfo attribute regular expression, compiled
        /// </summary>
        public readonly Regex Regex;

        #endregion

        #region Public Methods

        public bool Match(string url, string httpMethod, ref Match match)
        {
            match = this.Regex.Match(url);
            return match.Success && httpMethod.Equals(this.Method);
        }
         
        #endregion
    }
}
