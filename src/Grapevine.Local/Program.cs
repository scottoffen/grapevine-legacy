using System;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace Grapevine.Local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new RestServer())
            {
                server.LogToConsole();
                server.PublicFolder = new PublicFolder(@"C:\source\github\gv-gh-pages") {Prefix = "Grapevine"};

                server.OnBeforeStart = () => Console.WriteLine("---------------> Starting Server");
                server.OnAfterStart = () => Console.WriteLine($"<--------------- Server Started");

                server.OnBeforeStop = () => Console.WriteLine("---------------> Stopping Server");
                server.OnAfterStop = () =>
                {
                    Console.WriteLine("<--------------- Server Stopped");
                    Console.ReadLine();
                };

                server.Start();
                Console.ReadLine();
                server.Stop();
            }
        }
    }

    [RestResource]
    public class TestResource
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeFirst(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeSecond(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeThird(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        public IHttpContext HelloWorld(IHttpContext context)
        {
            context.Response.SendResponse("Hello,world.");
            return context;
        }
    }
}
