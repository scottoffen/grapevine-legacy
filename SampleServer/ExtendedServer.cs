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
    }
}
