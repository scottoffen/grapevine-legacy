using System;
using System.Diagnostics;
using System.Net;
using System.Text;
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
                server.PublicFolder.FolderPath = @"C:\source\gv-gh-pages";
                server.PublicFolder.Prefix = "/Grapevine";

                server.Router.Register(context =>
                {
                    context.Response.SendResponse($"Requested: {context.Request.PathInfo}", Encoding.ASCII);
                    return context;
                });

                server.Start();
                Console.WriteLine($"Server is running on port {server.Port}");
                Console.ReadLine();
                server.Stop();
            }
        }

        public static void UnitOfWork()
        {
            using (var server = new RestServer())
            {
                server.Router.Register(Route.For(HttpMethod.ALL).Use(context =>
                {
                    context.Properties.UserName = "Scotty";
                    return context;
                }));

                server.Router.Register(Route.For(HttpMethod.GET).To("/special/api").Use(context =>
                {
                    context.Response.ContentType = ContentType.TEXT;
                    context.Response.SendResponse(context.Properties.UserName);
                    return context;
                }));

                server.Router.Register(Route.For(HttpMethod.POST).Use(context =>
                {
                    var response = new StringBuilder();

                    context.Response.ContentType = ContentType.TEXT;

                    response.Append($"Method   : {context.Request.HttpMethod}\n");
                    response.Append($"PathInfo : {context.Request.PathInfo}\n");
                    response.Append($"Response : {context.Response.ContentType}\n");

                    context.Response.SendResponse(response.ToString());
                    return context;
                }));

                server.Start();

                Console.Write("Running Unit of Work Instance...");
                Console.ReadLine();
                Console.WriteLine("done");
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
                        ctx.Properties.Token = "some_token";
                        return ctx;
                    }));

                    r.Register(Route.For(HttpMethod.GET).To("/api/.*").Use(ctx =>
                    {
                        ctx.Response.AppendCookie(new Cookie { Name = "Token", Value = ctx.Properties.Token });
                        var response = ctx.Request.Headers.Get("SomeKey");
                        ctx.Response.SendResponse(response);
                        return ctx;
                    }));
                });
            });

            server.Start();

            Console.Write("Running Nested Closures Instance...");
            Console.ReadLine();

            server.Stop();
            Console.WriteLine("done");
        }

        public static void RunRestCluster()
        {
            ThreadCount("Baseline");

            var cluster = new RestCluster();

            var serverone = RestServer.For(_ =>
            {
                var serverName = "Server1";

                _.Port = PortFinder.FindNextLocalOpenPort(5000);
                _.Logger = new ConsoleLogger();
                _.Router = Router.For(r =>
                {
                    r.Register(Route.For(HttpMethod.ALL).Use(context =>
                    {
                        context.Properties.UserName = Guid.NewGuid().ToString();
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.StatusCode = Util.HttpStatusCode.ImATeapot;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.StatusCode = Util.HttpStatusCode.EnhanceYourCalm;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Properties.UserName}: I'm a post and I'm okay...");
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Properties.UserName} : Nobody gets me :(");
                        return context;
                    }));
                });

                _.OnBeforeStart = () =>
                {
                    ThreadCount("BeforeStart");
                    Console.Write($"Starting {serverName}...");
                };

                _.OnAfterStart = () =>
                {
                    ThreadCount("AfterStart");
                    Console.WriteLine("done!");
                    Console.WriteLine($"{serverName} is listening on port {_.Port}");
                };

                _.OnBeforeStop = () =>
                {
                    ThreadCount("BeforeStop");
                    Console.Write($"Stopping {serverName}...");
                };

                _.OnAfterStop = () =>
                {
                    ThreadCount("AfterStop");
                    Console.WriteLine("done!");
                    Console.WriteLine($"{serverName} is stopped");
                };
            });

            var servertwo = RestServer.For(_ =>
            {
                var serverName = "Server2";

                _.Port = PortFinder.FindNextLocalOpenPort(5500);
                _.Logger = new ConsoleLogger();
                _.Router = Router.For(r =>
                {
                    r.Register(Route.For(HttpMethod.ALL).Use(context =>
                    {
                        context.Properties.UserName = context.Request.HttpMethod == HttpMethod.GET ? "Bobby" : "Jimmy";
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.StatusCode = Util.HttpStatusCode.ImATeapot;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.StatusCode = Util.HttpStatusCode.EnhanceYourCalm;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Properties.UserName}: I'm a post and I'm okay...");
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Properties.UserName} : Nobody gets me :(");
                        return context;
                    }));
                });

                _.OnBeforeStart = () =>
                {
                    ThreadCount("BeforeStart");
                    Console.Write($"Starting {serverName}...");
                };

                _.OnAfterStart = () =>
                {
                    ThreadCount("AfterStart");
                    Console.WriteLine("done!");
                    Console.WriteLine($"{serverName} is listening on port {_.Port}");
                };

                _.OnBeforeStop = () =>
                {
                    ThreadCount("BeforeStop");
                    Console.Write($"Stopping {serverName}...");
                };

                _.OnAfterStop = () =>
                {
                    ThreadCount("AfterStop");
                    Console.WriteLine("done!");
                    Console.WriteLine($"{serverName} is stopped");
                };
            });

            ThreadCount("BeforeClusterStart");

            cluster.Add("ServerOne", serverone);
            cluster.Add("ServerTwo", servertwo);
            cluster.StartAll();

            ThreadCount("AfterClusterStart");

            Console.WriteLine("Servers Started");
            Console.ReadLine();

            ThreadCount("BeforeClusterStop");
            cluster.StopAll();
            ThreadCount("AfterClusterStop");

            Console.ReadLine();
            ThreadCount("AfterWait");

            Console.ReadLine();
        }

        public static void ThreadCount(string label = "Current Threads")
        {
            var threads = Process.GetCurrentProcess().Threads.Count;
            Console.WriteLine($"{label} : {threads}");
        }
    }
}
