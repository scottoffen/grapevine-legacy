namespace Grapevine
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class Handler : System.Attribute
    {
        public HttpMethod Method;
        public string PathInfo;

        public Handler()
        {
            this.Method = HttpMethod.GET;
            this.PathInfo = "/";
        }
    }

    public enum HttpMethod
    {
        GET,
        PUT,
        POST,
        PATCH,
        DELETE,
        COPY,
        HEAD,
        OPTIONS,
        LINK,
        UNLINK,
        PURGE,
        LOCK,
        UNLOCK,
        PROPFIND
    };
}
