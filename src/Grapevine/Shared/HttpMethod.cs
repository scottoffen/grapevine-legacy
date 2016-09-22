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


    }
}
