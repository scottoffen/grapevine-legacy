namespace Grapevine
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class Responder : System.Attribute
    {
        public HttpMethod Method;
        public string PathInfo;

        public Responder()
        {
            this.Method = HttpMethod.GET;
            this.PathInfo = "/";
        }
    }

    public enum HttpMethod { GET, HEAD, POST, PUT, DELETE };
}
