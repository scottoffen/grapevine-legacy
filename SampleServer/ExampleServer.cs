using System;
using System.Linq;
using System.Net;
using Grapevine;

namespace SampleServer
{
    class ExampleServer : RestServer
    {
        // This simple route catches all get traffic to /foo/[numbers]
        [RestRoute(Method = HttpMethod.GET, PathInfo = @"^/foo/\d+$")]
        public void HandleFoo(HttpListenerContext context)
        {
            var num = NumberFunction(context.Request.RawUrl.GrabFirst(@"^/foo/(\d+)$"));
            this.SendTextResponse(context, "Foo is : " + num);
        }

        [RestRoute(Method = HttpMethod.GET, PathInfo = @"^/foo/\D+$")]
        public void HandleMoeFoo(HttpListenerContext context)
        {
            context.Response.StatusDescription = "I'm a teapot";
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 418;

            this.SendTextResponse(context, "Make your own coffee, foo!");
        }

        [RestRoute(Method = HttpMethod.POST, PathInfo = @"/.?")]
        public void HandleAllPosts(HttpListenerContext context)
        {
            this.SendTextResponse(context, "Rain or shine, snow or sleet!");
        }

        [RestRoute(Method = HttpMethod.DELETE, PathInfo = @"^/shutdown$")]
        public virtual void RemoteShutDown(HttpListenerContext context)
        {
            this.SendTextResponse(context, "Shutting down, Mr. Bond...");
            this.IsListening = false;
        }

        private string NumberFunction(string values)
        {
            if (Object.ReferenceEquals(values, null))
            {
                return null;
            }
            else
            {
                char[] tokens = values.ToCharArray();
                string[] strs = Array.ConvertAll<char, string>(tokens, char.ToString);
                return Array.ConvertAll<string, int>(strs, int.Parse).Sum().ToString();
            }
        }
    }
}
