using System;

namespace Grapevine
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RestRoute : Attribute
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
