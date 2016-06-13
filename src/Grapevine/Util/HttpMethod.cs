namespace Grapevine.Util
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

    public static class HttpMethodExenstions
    {
        public static bool IsEquivalent(this HttpMethod h, HttpMethod other)
        {
            return h == HttpMethod.ALL || other == HttpMethod.ALL || h == other;
        }
    }
}
