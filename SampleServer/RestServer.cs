using System;
using System.Net;
using Grapevine;

namespace SampleServer
{
    class RestServer : HttpResponder
    {
        [Responder(Method = "GET", PathInfo = @"/foo/\d+")]
        public void HandleSomething(HttpListenerContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "Success";
            this.SendTextResponse(context, "SUCCESS");
        }

        [Responder(Method = "POST", PathInfo = @"/.?")]
        public void HandleSomethingElse(HttpListenerContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "Success";
            this.SendTextResponse(context, "This is bad");
        }
    }
}
