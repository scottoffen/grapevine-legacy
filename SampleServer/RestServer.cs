using System.Net;
using Grapevine;

namespace SampleServer
{
    class RestServer : HttpResponder
    {
        [Responder(Method = HttpMethod.GET, PathInfo = @"/foo/\d+")]
        public void HandleFoo(HttpListenerContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "Success";
            this.SendTextResponse(context, "FOO IS SUCCESS");
        }

        [Responder(Method = HttpMethod.POST, PathInfo = @"/.?")]
        public void HandleAllPosts(HttpListenerContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "Success";
            this.SendTextResponse(context, "All Post Go to Heaven");
        }

        [Responder(Method = HttpMethod.DELETE, PathInfo = @"^/shutdown$")]
        public void RemoteShutDown(HttpListenerContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "Success";
            this.SendTextResponse(context, "Shutting down, Mr. Bond...");
            this._listening = false;
        }
    }
}
