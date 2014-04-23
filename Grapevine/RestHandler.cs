namespace Grapevine
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RestHandler : System.Attribute
    {
        public HttpMethod Method;
        public string PathInfo;

        public RestHandler()
        {
            this.Method = HttpMethod.GET;
            this.PathInfo = "/";
        }
    }
}
