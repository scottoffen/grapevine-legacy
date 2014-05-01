using System.Net;
using Grapevine;

namespace SampleServer
{
    class ExtendedServer : ExampleServer
    {
        [RestRoute(Method = HttpMethod.POST, PathInfo = @"/.?")]
        public void PostInterceptor(HttpListenerContext context)
        {
            this.SendTextResponse(context, "This should respond to all posts.");
        }

        public override void RemoteShutDown(HttpListenerContext context)
        {
            this.SendTextResponse(context, "Not gonna happen, bub.");
        }
    }
}
