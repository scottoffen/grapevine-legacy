using System;
using Grapevine.Server;
using Grapevine.Util;

namespace Grapevine.Local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new RestServer())
            {
                server.LogToConsole().Start();
                Console.ReadLine();
                server.Stop();
            }
        }
    }

    [RestResource]
    public class TestDocument
    {
        [RestRoute]
        public IHttpContext Start(IHttpContext context)
        {
            context.Properties.User = "Some User";
            return context;
        }

        [RestRoute]
        public IHttpContext Stop(IHttpContext context)
        {
            context.Response.SendResponse($"User: {context.Properties.User}");
            return context;
        }
    }
}
