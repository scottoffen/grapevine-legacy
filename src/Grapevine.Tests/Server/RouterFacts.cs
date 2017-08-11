using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;
using Grapevine.TestAssembly;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouterFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultProperties()
            {
                var router = new Router();

                router.RoutingTable.ShouldNotBeNull();
                router.RoutingTable.ShouldBeEmpty();

                router.Logger.ShouldNotBeNull();
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Scanner.ShouldNotBeNull();
                router.Scanner.ShouldBeOfType<RouteScanner>();

                router.Scope.ShouldNotBeNull();
                string.IsNullOrWhiteSpace(router.Scope).ShouldBeTrue();
            }

            [Fact]
            public void DefaultPropertiesWithScope()
            {
                const string scope = "MyScope";
                var router = new Router(scope);

                router.RoutingTable.ShouldNotBeNull();
                router.RoutingTable.ShouldBeEmpty();

                router.Logger.ShouldNotBeNull();
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Scanner.ShouldNotBeNull();
                router.Scanner.ShouldBeOfType<RouteScanner>();

                router.Scope.ShouldNotBeNull();
                router.Scope.ShouldBe(scope);
            }

            [Fact]
            public void FluentInitialization()
            {
                const string scope = "MyScope";

                var router = Router.For(_ =>
                {
                    _.Register(Route.For(HttpMethod.ALL).Use(context => context));
                    _.ContinueRoutingAfterResponseSent = true;
                }, scope);

                router.Scope.ShouldBe(scope);
                router.RoutingTable.Count.ShouldBe(1);
                router.ContinueRoutingAfterResponseSent.ShouldBeTrue();
            }
        }

        public class AddRouteToTableMethod
        {
            [Fact]
            public void ThrowsExceptionIfRouteFunctionIsNull()
            {
                var router = new Router();
                var route = Substitute.For<IRoute>();
                route.Function.ReturnsNull();
                Should.Throw<ArgumentNullException>(() => router.AddRouteToTable(route));
            }
        }

        public class AfterEvent
        {
            [Fact]
            public void ExecutesAfterRouting()
            {
                var executionOrder = new List<string>();
                var context = Mocks.HttpContext();

                IList<IRoute> routing = new List<IRoute>();
                var route = new Route(ctx => { executionOrder.Add("function");
                    context.WasRespondedTo.Returns(true); return ctx; });
                routing.Add(route);

                var router = new Router { After = ctx => { executionOrder.Add("after"); return ctx; } };

                router.Route(context, routing);

                executionOrder[0].ShouldBe("function");
                executionOrder[1].ShouldBe("after");
            }

            [Fact]
            public void ExecutesOnAfterRoutingAfterRouting()
            {
                var firingOrder = new List<string>();
                var context = Mocks.HttpContext();
                RoutingEventHandler one = ctx => { firingOrder.Add("1"); };
                RoutingEventHandler two = ctx => { firingOrder.Add("2"); };

                var router = new Router().Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                });

                router.AfterRouting += one;
                router.AfterRouting += two;

                router.Route(context);

                firingOrder.Count.ShouldBe(2);
                firingOrder[0].ShouldBe("2");
                firingOrder[1].ShouldBe("1");
            }
        }

        public class BeforeEvent
        {
            [Fact]
            public void ExecutesBeforeRouting()
            {
                var executionOrder = new List<string>();
                var context = Mocks.HttpContext();

                IList<IRoute> routing = new List<IRoute>();
                var route = new Route(ctx => { executionOrder.Add("function");
                    context.WasRespondedTo.Returns(true); return ctx; });
                routing.Add(route);

                var router = new Router { Before = ctx => { executionOrder.Add("before"); return ctx; } };

                router.Route(context, routing);

                executionOrder[0].ShouldBe("before");
                executionOrder[1].ShouldBe("function");
            }

            [Fact]
            public void ExecutesOnBeforeRoutingBeforeRouting()
            {
                var firingOrder = new List<string>();
                var context = Mocks.HttpContext();
                RoutingEventHandler one = ctx => { firingOrder.Add("1"); };
                RoutingEventHandler two = ctx => { firingOrder.Add("2"); };

                var router = new Router().Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                });

                router.BeforeRouting += one;
                router.BeforeRouting += two;

                router.Route(context);

                firingOrder.Count.ShouldBe(2);
                firingOrder[0].ShouldBe("1");
                firingOrder[1].ShouldBe("2");
            }
        }

        public class ContinueRoutingAfterResponseSentProperty
        {
            [Fact]
            public void ContinuesExecutionAfterResponseSentWhenTrue()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var router = new Router { ContinueRoutingAfterResponseSent = true }.Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }).Register(ctx =>
                {
                    executed = true;
                    return ctx;
                });

                router.Route(context);
                executed.ShouldBeTrue();
            }

            [Fact]
            public void StopsExecutionAfterResponseSentWhenFalse()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var router = new Router().Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }).Register(ctx =>
                {
                    executed = true;
                    return ctx;
                });

                router.Route(context);
                executed.ShouldBeFalse();
            }
        }

        public class ImportMethod
        {
            [Fact]
            public void ThrowsExceptionWhenTypeIsNotAClass()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(IRouter)));
            }

            [Fact]
            public void ThrowsExceptionWhenTypeDoesNotImplementIRouter()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(NotARouter)));
            }

            [Fact]
            public void ThrowsExceptionWhenTypeIsAbstract()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(AbstractRouter)));
            }

            [Fact]
            public void ThrowsExceptionWhenTypeDoesNotHaveParameterlessConstructor()
            {
                var router = new Router();
                Should.Throw<ArgumentException>(() => router.Import(typeof(NoParameterlessConstructor)));
            }

            [Fact]
            public void ImportsRoutesFromAnotherRouterInstance()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(new RouterToImport());

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void ImportsRoutesFromSpecifiedRouterType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import(typeof(RouterToImport));

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void ImportsRoutesFromGenericRouterType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Import<RouterToImport>();

                router.RoutingTable.Count.ShouldBe(8);
            }
        }

        public class InsertAfterMethod
        {
            private readonly Route routeA;
            private readonly Route routeB;
            private readonly Route routeC;
            private readonly Route route1;
            private readonly Route route2;
            private readonly Route route3;
            private readonly List<IRoute> routes;
            private readonly Router router;

            public InsertAfterMethod()
            {
                routeA = new Route(context => context);
                routeB = new Route(context => context);
                routeC = new Route(context => context);
                route1 = new Route(context => context);
                route2 = new Route(context => context);
                route3 = new Route(context => context);

                routes = new List<IRoute> {route1, route2, route3};

                router = new Router();
            }

            [Fact]
            public void DoesNotInsertDuplicate()
            {
                var expected = new List<IRoute> { routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(1, routeB);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertWhenIndexIsLessThanZero()
            {
                var expected = new List<IRoute> { routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(-1, routeC);
                router.InsertAfter(-1, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertRouteWhenMarkerIsNotFound()
            {
                var expected = new List<IRoute> { routeA };

                router.Register(routeA);
                router.InsertAfter(routeB, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertRouteListWhenMarkerIsNotFound()
            {
                var expected = new List<IRoute> { routeA };

                router.Register(routeA);
                router.InsertAfter(routeB, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteAfterIndex()
            {
                var expected = new List<IRoute> { routeA, routeC, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(0, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteListAfterIndex()
            {
                var expected = new List<IRoute> { routeA, route1, route2, route3, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(0, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteAfterMarker()
            {
                var expected = new List<IRoute> { routeA, routeC, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(routeA, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteListAfterMarker()
            {
                var expected = new List<IRoute> { routeA, route1, route2, route3, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertAfter(routeA, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void ThrowsExceptionWhenIndexIsOutOfRange()
            {
                router.Register(routeA);
                Should.Throw<ArgumentOutOfRangeException>(() => router.InsertAfter(5, routeB));
            }
        }

        public class InsertBeforeMethod
        {
            private readonly Route routeA;
            private readonly Route routeB;
            private readonly Route routeC;
            private readonly Route route1;
            private readonly Route route2;
            private readonly Route route3;
            private readonly List<IRoute> routes;
            private readonly Router router;

            public InsertBeforeMethod()
            {
                routeA = new Route(context => context);
                routeB = new Route(context => context);
                routeC = new Route(context => context);
                route1 = new Route(context => context);
                route2 = new Route(context => context);
                route3 = new Route(context => context);

                routes = new List<IRoute> { route1, route2, route3 };

                router = new Router();
            }

            [Fact]
            public void DoesNotInsertDuplicate()
            {
                var expected = new List<IRoute> { routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(1, routeB);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertWhenIndexIsLessThanZero()
            {
                var expected = new List<IRoute> { routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(-1, routeC);
                router.InsertBefore(-1, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertRouteWhenMarkerIsNotFound()
            {
                var expected = new List<IRoute> { routeA };

                router.Register(routeA);
                router.InsertBefore(routeB, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void DoesNotInsertRouteListWhenMarkerIsNotFound()
            {
                var expected = new List<IRoute> { routeA };

                router.Register(routeA);
                router.InsertBefore(routeB, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteBeforeIndex()
            {
                var expected = new List<IRoute> { routeA, routeC, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(1, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteListBeforeIndex()
            {
                var expected = new List<IRoute> { routeA, route1, route2, route3, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(1, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteBeforeMarker()
            {
                var expected = new List<IRoute> { routeA, routeC, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(routeB, routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteListBeforeMarker()
            {
                var expected = new List<IRoute> { routeA, route1, route2, route3, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertBefore(routeB, routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void ThrowsExceptionWhenIndexIsOutOfRange()
            {
                router.Register(routeA);
                Should.Throw<ArgumentOutOfRangeException>(() => router.InsertBefore(5, routeB));
            }
        }

        public class InsertFirstMethod
        {
            private readonly Route routeA;
            private readonly Route routeB;
            private readonly Route routeC;
            private readonly Route route1;
            private readonly Route route2;
            private readonly Route route3;
            private readonly List<IRoute> routes;
            private readonly Router router;

            public InsertFirstMethod()
            {
                routeA = new Route(context => context);
                routeB = new Route(context => context);
                routeC = new Route(context => context);
                route1 = new Route(context => context);
                route2 = new Route(context => context);
                route3 = new Route(context => context);

                routes = new List<IRoute> { route1, route2, route3 };

                router = new Router();
            }

            [Fact]
            public void DoesNotInsertDuplicate()
            {
                var expected = new List<IRoute> { routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertFirst(routeB);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteFirst()
            {
                var expected = new List<IRoute> { routeC, routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertFirst(routeC);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }

            [Fact]
            public void InsertRouteListFirst()
            {
                var expected = new List<IRoute> { route1, route2, route3, routeA, routeB };

                router.Register(routeA);
                router.Register(routeB);
                router.InsertFirst(routes);

                router.RoutingTable.SequenceEqual(expected).ShouldBeTrue();
            }
        }

        public class LoggerProperty
        {
            [Fact]
            public void SetToNullSetsNullLogger()
            {
                var router = new Router { Logger = new InMemoryLogger() };
                router.Logger.ShouldBeOfType<InMemoryLogger>();

                router.Logger = null;

                router.Logger.ShouldBeOfType<NullLogger>();
            }

            [Fact]
            public void PropogatesToScanner()
            {
                var scanner = new RouteScanner();
                scanner.Logger.ShouldBeOfType<NullLogger>();

                var router = new Router { Scanner = scanner };
                router.Logger.ShouldBeOfType<NullLogger>();

                router.Logger = new InMemoryLogger();

                router.Logger.ShouldBeOfType<InMemoryLogger>();
                scanner.Logger.ShouldBeOfType<InMemoryLogger>();

                router.Scanner = null;
                Should.NotThrow(() => router.Logger = NullLogger.GetInstance());
            }
        }

        public class RegisterMethod
        {
            [Fact]
            public void RegistersRoute()
            {
                var route = new Route(context => context);
                var router = new Router();

                router.Register(route);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                var route = new Route(func);
                var router = new Router();

                router.Register(func);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAndHttpMethodAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const HttpMethod verb = HttpMethod.GET;

                var route = new Route(func, verb);
                var router = new Router();

                router.Register(func, verb);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionAndPathInfoAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const string pathinfo = "/path";

                var route = new Route(func, pathinfo);
                var router = new Router();

                router.Register(func, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersFunctionHttpMethodAndPathInfoAsRoute()
            {
                Func<IHttpContext, IHttpContext> func = context => context;
                const HttpMethod verb = HttpMethod.GET;
                const string pathinfo = "/path";

                var route = new Route(func, verb, pathinfo);
                var router = new Router();

                router.Register(func, verb, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethod_as_route()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                var route = new Route(method);
                var router = new Router();

                router.Register(method);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodAndHttpMethodAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const HttpMethod verb = HttpMethod.GET;

                var route = new Route(method, verb);
                var router = new Router();

                router.Register(method, verb);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodAndPathInfoAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const string pathinfo = "/path";

                var route = new Route(method, pathinfo);
                var router = new Router();

                router.Register(method, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersMethodHttpMethodAndPathInfoAsRoute()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                const HttpMethod verb = HttpMethod.GET;
                const string pathinfo = "/path";

                var route = new Route(method, verb, pathinfo);
                var router = new Router();

                router.Register(method, verb, pathinfo);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route).ShouldBe(true);
            }

            [Fact]
            public void RegistersType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(MethodsToRegister));

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void RegistersGenericType()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register<MethodsToRegister>();

                router.RoutingTable.Count.ShouldBe(1);
            }

            [Fact]
            public void RegistersAssembly()
            {
                var router = new Router();
                router.RoutingTable.ShouldBeEmpty();

                router.Register(typeof(DefaultRouter).Assembly);

                router.RoutingTable.Count.ShouldBe(8);
            }

            [Fact]
            public void PreventsDuplicateRouteRegistrations()
            {
                var method = typeof(MethodsToRegister).GetMethod("Method");
                var route1 = new Route(method);
                var route2 = new Route(method);
                var router = new Router();

                router.Register(route1);
                router.Register(route2);

                router.RoutingTable.Count.ShouldBe(1);
                router.RoutingTable[0].Equals(route1).ShouldBe(true);
                router.RoutingTable[0].Equals(route2).ShouldBe(true);
            }
        }

        public class RouteMethod
        {
            [Fact]
            public void ReturnsWithoutExceptionWhenStateIsNull()
            {
                var router = new Router();
                router.Route(null);
            }

            [Fact]
            public void ReturnsWithoutExceptionWhenStateIsNotContext()
            {
                var router = new Router();
                router.Route("Not Context");
            }

            [Fact]
            public void CallsSendFileOnGet()
            {
                var folder = Substitute.For<PublicFolder>();
                var server = Substitute.For<IRestServer>();
                server.PublicFolder.Returns(folder);

                var context = Mocks.HttpContext();
                context.Server.Returns(server);

                var route = new Route(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }, HttpMethod.GET);

                var router = new Router();
                router.Register(route);

                router.Route(context);

                folder.ReceivedWithAnyArgs().SendFile(context);
            }

            [Fact]
            public void DoesNotCallSendFileOnNonGet()
            {
                var folder = Substitute.For<PublicFolder>();
                var server = Substitute.For<IRestServer>();
                server.PublicFolder.Returns(folder);

                var context = Mocks.HttpContext();
                context.Server.Returns(server);
                context.Request.HttpMethod.Returns(HttpMethod.POST);

                var route = new Route(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    return ctx;
                }, HttpMethod.POST);

                var router = new Router();
                router.Register(route);

                router.Route(context);

                folder.DidNotReceive().SendFile(context);
            }

            [Fact]
            public void IteratestOverAllPublicFolders()
            {
                var folders = new List<IPublicFolder> { Substitute.For<IPublicFolder>(), Substitute.For<IPublicFolder>() };
                var server = Substitute.For<IRestServer>();
                server.PublicFolders.Returns(folders);

                var context = Mocks.HttpContext();
                context.Request.HttpMethod.ShouldBe(HttpMethod.GET);
                context.Server.Returns(server);

                folders[1].When(x => x.SendFile(context)).Do(x => context.WasRespondedTo.Returns(true));

                var router = new Router();
                router.Route(context);

                folders[0].ReceivedWithAnyArgs().SendFile(context);
                folders[1].ReceivedWithAnyArgs().SendFile(context);
            }

            [Fact]
            public void SendResponseNotCalledIfRequestAlreadyRespondedTo()
            {
                var exception = new Exception("Generic Exception");
                var context = Mocks.HttpContext();

                var router = new Router();
                router.Register(ctx =>
                {
                    context.WasRespondedTo.Returns(true);
                    throw exception;
                });

                router.Route(context);

                context.Response.DidNotReceive().SendResponse(HttpStatusCode.InternalServerError, exception);
            }

            [Fact]
            public void ThrowsExcpetionWhenExceptionThrownAndExceptionThrowingIsEnabled()
            {
                var server = Substitute.For<IRestServer>();
                server.EnableThrowingExceptions.Returns(true);

                var context = Mocks.HttpContext();
                context.Server.Returns(server);

                var router = new Router();
                router.Register(ctx => { throw new Exception("Generic Exception"); });

                Should.Throw<Exception>(() => router.Route(context));
            }

            [Fact]
            public void SendsNotFoundResponseWhenFileNotFoundExceptionThrown()
            {
                const string fileRequested = "/some/file/on/server";
                var message = $"File {fileRequested} was not found";

                var folder = Substitute.For<IPublicFolder>();

                var server = Substitute.For<IRestServer>();
                server.PublicFolders.Returns(new List<IPublicFolder> { folder });

                var context = Mocks.HttpContext();
                context.Request.PathInfo.Returns(fileRequested);
                context.Server.Returns(server);

                folder.When(_ => _.SendFile(context)).Do(info => { throw new FileNotFoundException(context); });

                var router = new Router();
                router.Route(context);

                context.Response.Received().SendResponse(HttpStatusCode.NotFound, message);
            }

            [Fact]
            public void SendsNotFoundResponseWhenRouteNotFoundExceptionThrown()
            {
                const string pathinfo = "resource/parameter";
                var message = $"Route Not Found For {HttpMethod.POST} {pathinfo}";

                var context = Mocks.HttpContext();
                context.Request.HttpMethod.Returns(HttpMethod.POST);
                context.Request.PathInfo.Returns(pathinfo);

                var router = new Router();
                router.Route(context);

                context.Response.Received().SendResponse(HttpStatusCode.NotFound, message);
            }

            [Fact]
            public void SendsNotImplementedResponseWhenNotImplementedExceptionThrown()
            {
                const string message = "this has not been implemented";

                var context = Mocks.HttpContext();

                var router = new Router().Register(ctx => { throw new NotImplementedException(message); });
                router.Route(context);

                context.Response.Received().SendResponse(HttpStatusCode.NotImplemented, message);
            }

            [Fact]
            public void SendsInternalServerErrorResponseWhenExceptionThrown()
            {
                var exception = new Exception("Generic Exception");

                var context = Mocks.HttpContext();

                var router = new Router().Register(ctx => { throw exception; });
                router.Route(context);

                context.Response.Received().SendResponse(HttpStatusCode.InternalServerError, exception);
            }

            [Fact]
            public void ThrowsRouteNotFoundExceptionWhenRouteListIsNull()
            {
                Should.Throw<RouteNotFoundException>(() => new Router().Route(Mocks.HttpContext(), null));
            }

            [Fact]
            public void ThrowsRouteNotFoundExceptionWhenRouteListIsEmpty()
            {
                Should.Throw<RouteNotFoundException>(() => new Router().Route(Mocks.HttpContext(), new List<IRoute>()));
            }

            [Fact]
            public void ThrowsRouteNotFoundExceptionWhenNoRouteSendsResponse()
            {
                Should.Throw<RouteNotFoundException>(() => new Router().Route(Mocks.HttpContext(), new List<IRoute> {new Route(context => context)}));
            }

            [Fact]
            public void RoutingStopsAfterResponseIsSentByDefault()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var routes = new List<IRoute>
                {
                    new Route(ctx =>
                    {
                        context.WasRespondedTo.Returns(true);
                        return ctx;
                    }),
                    new Route(ctx =>
                    {
                        executed = true;
                        return ctx;
                    })
                };

                new Router().Route(context, routes);
                executed.ShouldBeFalse();
            }

            [Fact]
            public void RoutingContinuesAfterResponseIsSentWhenContinueRoutingAfterResponseSentIsTrue()
            {
                var executed = false;
                var context = Mocks.HttpContext();

                var routes = new List<IRoute>
                {
                    new Route(ctx =>
                    {
                        context.WasRespondedTo.Returns(true);
                        return ctx;
                    }),
                    new Route(ctx =>
                    {
                        executed = true;
                        return ctx;
                    })
                };

                new Router {ContinueRoutingAfterResponseSent = true}.Route(context, routes);
                executed.ShouldBeTrue();
            }
        }

        public class RoutesForMethod
        {
            [Fact]
            public void ReturnsRoutesForGet()
            {
                var router = new Router().Import<RouterToImport>();
                var context = Mocks.HttpContext(new Dictionary<string, object> {{"HttpMethod", HttpMethod.GET}, {"PathInfo", "/user/list"}});

                var routes = router.RoutesFor(context);

                router.RoutingTable.Count.ShouldBe(8);
                routes.Count.ShouldBe(1);
            }

            [Fact]
            public void ReturnsRoutesForPost()
            {
                var router = new Router().Import<RouterToImport>();
                var context = Mocks.HttpContext(new Dictionary<string, object> { { "HttpMethod", HttpMethod.POST }, { "PathInfo", "/user" } });

                var routes = router.RoutesFor(context);

                router.RoutingTable.Count.ShouldBe(8);
                routes.Count.ShouldBe(1);
            }
        }

        public class ScanAssembliesMethod
        {
            [Fact]
            public void AddsRoutesToRouter()
            {
                var scanner = Substitute.For<IRouteScanner>();
                scanner.Scan().Returns(new List<IRoute>());

                var router = new Router { Scanner = scanner };

                router.ScanAssemblies();

                scanner.Received().Scan();
            }
        }
    }

    public abstract class AbstractRouter : Router
    {
        /* This class intentionally left blank */
        /* This is an IRouter but it's abstract*/
    }

    public class NotARouter
    {
        /* This class intentionally left blank */
        /* This is not an IRouter */
    }

    public class NoParameterlessConstructor : IRouter
    {
        /* This is an implementation of IRouter */
        /* But it lacks a parameterless constructor */

        private readonly string _parameter;

        public NoParameterlessConstructor(string parameter)
        {
            _parameter = parameter;
        }

        public Func<IHttpContext, IHttpContext> After { get; set; }
        public Func<IHttpContext, IHttpContext> Before { get; set; }
        public event RoutingEventHandler AfterRouting;
        public event RoutingEventHandler BeforeRouting;
        public bool ContinueRoutingAfterResponseSent { get; set; }
        public bool SendExceptionMessages { get; set; }
        public IRouteScanner Scanner { get; }
        public IList<IRoute> RoutingTable { get; }
        public string Scope { get; }
        public IRouter Import(IRouter router)
        {
            throw new NotImplementedException();
        }

        public IRouter Import(Type type)
        {
            throw new NotImplementedException();
        }

        public IRouter Import<T>() where T : IRouter, new()
        {
            throw new NotImplementedException();
        }

        public IRouter InsertAfter(int index, IRoute route)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertAfter(IRoute afterThisRoute, IRoute insertThisRoute)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertAfter(int index, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertAfter(IRoute afterThisRoute, IList<IRoute> insertTheseRoutes)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertBefore(int index, IRoute route)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertBefore(IRoute beforeThisRoute, IRoute insertThisRoute)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertBefore(int index, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertBefore(IRoute beforeThisRoute, IList<IRoute> insertTheseRoutes)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertFirst(IRoute route)
        {
            throw new NotImplementedException();
        }

        public IRouter InsertFirst(IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        public IGrapevineLogger Logger { get; set; }
        public IRouter Register(IRoute route)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(MethodInfo method, string pathInfo)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(MethodInfo method, HttpMethod httpMethod)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(MethodInfo method, HttpMethod httpMethod, string pathInfo)
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Type type)
        {
            throw new NotImplementedException();
        }

        public IRouter Register<T>()
        {
            throw new NotImplementedException();
        }

        public IRouter Register(Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public IRouter ScanAssemblies()
        {
            throw new NotImplementedException();
        }

        public void Route(object state)
        {
            throw new NotImplementedException();
        }

        public void Route(IHttpContext context, IList<IRoute> routing)
        {
            throw new NotImplementedException();
        }

        public IList<IRoute> RoutesFor(IHttpContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class MethodsToRegister
    {
        [RestRoute]
        public IHttpContext Method(IHttpContext context) { return context; }
    }

    public class RouterToImport : Router
    {
        public RouterToImport()
        {
            AppendRoutingTable(Scanner.ScanAssembly(typeof(DefaultRouter).Assembly));
        }
    }

    public static class RouterExtensions
    {
        internal static void AddRouteToTable(this Router router, IRoute route)
        {
            var memberInfo = router.GetType();
            var method = memberInfo?.GetMethod("AppendRoutingTable", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(IRoute) }, null);

            try
            {
                method.Invoke(router, new object[] { route });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }
    }
}
