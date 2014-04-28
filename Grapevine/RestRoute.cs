namespace Grapevine
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RestRoute : System.Attribute
    {
        public HttpMethod Method;
        public string PathInfo;

        public RestRoute()
        {
            this.Method = HttpMethod.GET;
            this.PathInfo = "/";
        }
    }
}
