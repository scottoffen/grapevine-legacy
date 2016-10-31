using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Grapevine.Shared
{
    /// <summary>
    /// Http methods (or verbs) to indicate the desired action to be performed on the resource
    /// </summary>
    public enum HttpMethod
    {
        ALL,
        CONNECT,
        COPY,
        DELETE,
        GET,
        HEAD,
        LINK,
        LOCK,
        OPTIONS,
        PATCH,
        POST,
        PROPFIND,
        PURGE,
        PUT,
        TRACE,
        UNLINK,
        UNLOCK
    };

    public static class HttpMethodExtensions
    {
        private static readonly ConcurrentDictionary<string, int> Lookup;

        static HttpMethodExtensions()
        {
            Lookup = new ConcurrentDictionary<string, int>();

            foreach (var val in Enum.GetValues(typeof(HttpMethod)).Cast<HttpMethod>())
            {
                var key = val.ToString();
                Lookup[key] = (int)val;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the HttpMethods are equal OR one of them is HttpMethod.ALL
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="other"></param>
        /// <returns>bool</returns>
        public static bool IsEquivalent(this HttpMethod httpMethod, HttpMethod other)
        {
            return httpMethod == HttpMethod.ALL || other == HttpMethod.ALL || httpMethod == other;
        }

        /// <summary>
        ///  Returns an HttpMethod value for the method string provided
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static HttpMethod FromString(this HttpMethod httpMethod, string method)
        {
            var ucMethod = method.ToUpper();
            return (Lookup.ContainsKey(ucMethod)) ? (HttpMethod) Lookup[ucMethod] : 0;
        }
    }
}