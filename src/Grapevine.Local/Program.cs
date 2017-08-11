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

                server.OnBeforeStart = () => server.Logger.Trace("Starting Server");
                server.OnAfterStart = () => server.Logger.Trace("Server Started");
                server.OnBeforeStop = () => server.Logger.Trace("Stopping Server");
                server.OnAfterStop = () => server.Logger.Trace("Server Stopped");
                server.Router.BeforeRouting += ctx => server.Logger.Debug("Before Routing!!");
                server.Router.BeforeRouting += ctx => server.Logger.Debug("After Routing!!");

                server.Start();
                Console.ReadLine();
                server.Stop();
            }
        }
    }

    [RestResource]
    public class TestResource
    {
        [RestRoute(HttpMethod = HttpMethod.ALL, PathInfo = "^.*$")]
        public IHttpContext LevelOne(IHttpContext context)
        {
            throw new Exception("Killing It!");
        }

        [RestRoute]
        public IHttpContext HelloWorld(IHttpContext context)
        {
            context.Response.SendResponse("Hello,world.");
            return context;
        }
    }
}
