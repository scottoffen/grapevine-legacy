using System.Net;
using System.Text;
using Grapevine.Server;
using Grapevine.Util;
using HttpStatusCode = Grapevine.Util.HttpStatusCode;

namespace Grapevine.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnitOfWork();
            NestedClosures();
        }

        public static void UnitOfWork()
        {
            using (var server = new RestServer())
            {
                server.Router.Register(Route.For(HttpMethod.ALL).Use(context =>
                {
                    context.Dynamic.UserName = "Scotty";
                    return context;
                }));

                server.Router.Register(Route.For(HttpMethod.GET).To("/special/api").Use(context =>
                {
                    context.Response.ContentType = ContentType.TEXT.ToValue();
                    context.Response.SendResponse(context.Dynamic.UserName);
                    return context;
                }));

                server.Router.Register(Route.For(HttpMethod.POST).Use(context =>
                {
                    var response = new StringBuilder();

                    context.Response.ContentType = ContentType.TEXT.ToValue();

                    response.Append($"Method   : {context.Request.HttpMethod}\n");
                    response.Append($"PathInfo : {context.Request.PathInfo}\n");
                    response.Append($"Response : {context.Response.ContentType}\n");

                    context.Response.SendResponse(response.ToString());
                    return context;
                }));

                server.Start();

                System.Console.Write("Running Unit of Work Instance...");
                System.Console.ReadLine();
                System.Console.WriteLine("done");
            }
        }

        public static void NestedClosures()
        {
            var server = RestServer.For(s =>
            {
                s.Router = Router.For(r =>
                {
                    r.Register(Route.For(HttpMethod.GET).Use(ctx =>
                    {
                        ctx.Dynamic.Token = "some_token";
                        return ctx;
                    }));

                    r.Register(Route.For(HttpMethod.GET).To("/api/.*").Use(ctx =>
                    {
                        ctx.Response.AppendCookie(new Cookie { Name = "Token", Value = ctx.Dynamic.Token });
                        var response = ctx.Request.Headers.Get("SomeKey");
                        ctx.Response.SendResponse(response);
                        return ctx;
                    }));
                });
            });

            server.Start();

            System.Console.Write("Running Nested Closures Instance...");
            System.Console.ReadLine();

            server.Stop();
            System.Console.WriteLine("done");
        }
    }
}
