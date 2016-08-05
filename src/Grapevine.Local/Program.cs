using System;
using Grapevine.Server;

namespace Grapevine.Local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new RestServer())
            {
                server.PublicFolder.FolderPath = @"C:\source\github\gv-gh-pages";
                server.PublicFolder.Prefix = "/Grapevine";

                var port = server.Port;
                server.OnAfterStart = () => { Console.WriteLine($"Server is running on port {port}"); };
                server.OnAfterStop = () => { Console.WriteLine("Server has stopped"); };

                server.LogToConsole().Start();

                Console.ReadLine();
                server.Stop();
            }
        }
    }

    [RestResource(BasePath = "api")]
    public class TestDocument
    {
        [RestRoute]
        public IHttpContext Insert(IHttpContext context)
        {
            context.Response.SendResponse("This responds to everything that begins with /api/");
            return context;
        }
    }
}
