using System;
using System.Diagnostics;
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
            //NestedClosures();
            //RunRestCluster();
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

        public static void RunRestCluster()
        {
            ThreadCount("Baseline");

            var cluster = new RestCluster();

            var serverone = RestServer.For(_ =>
           {
               var sname = "Server1";

               _.Port = PortFinder.FindNextLocalOpenPort(5000);
               _.Logger = new ConsoleLogger();
               _.Router = Router.For(r =>
               {
                   r.Register(Route.For(HttpMethod.ALL).Use(context =>
                   {
                       context.Request.Dynamic.UserName = Guid.NewGuid().ToString();
                       return context;
                   }));

                   r.Register(Route.For(HttpMethod.GET).Use(context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.ImATeapot;
                       return context;
                   }));

                   r.Register(Route.For(HttpMethod.POST).Use(context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.EnhanceYourCalm;
                       return context;
                   }));

                   r.Register(Route.For(HttpMethod.POST).Use(context =>
                   {
                       context.Response.SendResponse($"{context.Request.Dynamic.UserName}: I'm a post and I'm okay...");
                       return context;
                   }));

                   r.Register(Route.For(HttpMethod.GET).Use(context =>
                   {
                       context.Response.SendResponse($"{context.Request.Dynamic.UserName} : Nobody gets me :(");
                       return context;
                   }));
               });

               _.OnBeforeStart = () =>
               {
                   ThreadCount("BeforeStart");
                   System.Console.Write($"Starting {sname}...");
               };

               _.OnAfterStart = () =>
               {
                   ThreadCount("AfterStart");
                   System.Console.WriteLine("done!");
                   System.Console.WriteLine($"{sname} is listening on port {_.Port}");
               };

               _.OnBeforeStop = () =>
               {
                   ThreadCount("BeforeStop");
                   System.Console.Write($"Stopping {sname}...");
               };

               _.OnAfterStop = () =>
               {
                   ThreadCount("AfterStop");
                   System.Console.WriteLine("done!");
                   System.Console.WriteLine($"{sname} is stopped");
               };
           });

            var servertwo = RestServer.For(_ =>
            {
                var sname = "Server2";

                _.Port = PortFinder.FindNextLocalOpenPort(5500);
                _.Logger = new ConsoleLogger();
                _.Router = Router.For(r =>
                {
                    r.Register(Route.For(HttpMethod.ALL).Use(context =>
                    {
                        context.Request.Dynamic.UserName = (context.Request.HttpMethod == HttpMethod.GET) ? "Bobby" : "Jimmy";
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.ImATeapot;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.EnhanceYourCalm;
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.POST).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Request.Dynamic.UserName}: I'm a post and I'm okay...");
                        return context;
                    }));

                    r.Register(Route.For(HttpMethod.GET).Use(context =>
                    {
                        context.Response.SendResponse($"{context.Request.Dynamic.UserName} : Nobody gets me :(");
                        return context;
                    }));
                });

                _.OnBeforeStart = () =>
                {
                    ThreadCount("BeforeStart");
                    System.Console.Write($"Starting {sname}...");
                };

                _.OnAfterStart = () =>
                {
                    ThreadCount("AfterStart");
                    System.Console.WriteLine("done!");
                    System.Console.WriteLine($"{sname} is listening on port {_.Port}");
                };

                _.OnBeforeStop = () =>
                {
                    ThreadCount("BeforeStop");
                    System.Console.Write($"Stopping {sname}...");
                };

                _.OnAfterStop = () =>
                {
                    ThreadCount("AfterStop");
                    System.Console.WriteLine("done!");
                    System.Console.WriteLine($"{sname} is stopped");
                };
            });

            ThreadCount("BeforeClusterStart");

            cluster.Add("ServerOne", serverone);
            cluster.Add("ServerTwo", servertwo);
            cluster.StartAll();

            ThreadCount("AfterClusterStart");

            System.Console.WriteLine("Servers Started");
            System.Console.ReadLine();

            ThreadCount("BeforeClusterStop");
            cluster.StopAll();
            ThreadCount("AfterClusterStop");

            System.Console.ReadLine();
            ThreadCount("AfterWait");

            System.Console.ReadLine();
        }

        public static void ThreadCount(string label = "Current Threads")
        {
            var threads = Process.GetCurrentProcess().Threads.Count;
            System.Console.WriteLine($"{label} : {threads}");
        }
    }
}
