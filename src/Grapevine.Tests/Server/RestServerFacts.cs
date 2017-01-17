using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Server;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RestServerFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultConfiguration()
            {
                using (var server = new RestServer())
                {
                    server.Connections.ShouldBe(50);
                    server.EnableThrowingExceptions.ShouldBeFalse();
                    server.Host.ShouldBe("localhost");
                    server.IsListening.ShouldBeFalse();
                    server.ListenerPrefix.ShouldBe("http://localhost:1234/");
                    server.Logger.ShouldBeOfType<NullLogger>();
                    server.OnAfterStart.ShouldBeNull();
                    server.OnAfterStop.ShouldBeNull();
                    server.OnBeforeStart.ShouldBeNull();
                    server.OnBeforeStop.ShouldBeNull();
                    server.OnStart.ShouldBeNull();
                    server.OnStop.ShouldBeNull();
                    server.Port.ShouldBe("1234");
                    server.PublicFolders.Any().ShouldBeFalse();
                    server.PublicFolder.ShouldNotBeNull();
                    server.PublicFolders.Any().ShouldBeTrue();
                    server.Router.ShouldBeOfType<Router>();
                    server.UseHttps.ShouldBeFalse();
                }
            }

            [Fact]
            public void ForAction()
            {
                const string port = "5555";
                const string index = "default.htm";

                using (var server = RestServer.For(_ =>
                {
                    _.Port = port;
                    _.PublicFolder = new PublicFolder { IndexFileName = index};
                }))
                {
                    server.Port.ShouldBe(port);
                    server.PublicFolder.IndexFileName.ShouldBe(index);
                }
            }

            [Fact]
            public void ForGenericType()
            {
                using (var server = RestServer.For<CustomSettings>())
                {
                    server.Port.ShouldBe("5555");
                }
            }

            [Fact]
            public void TestingMode()
            {
                var listener = Substitute.For<IHttpListener>();
                using (var server = new RestServer(listener))
                {
                    server.TestingMode.ShouldBeTrue();
                }
            }
        }

        public class HostProperty
        {
            [Fact]
            public void ThrowsExceptionWhenChangingWhileListenerIsListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    Should.Throw<ServerStateException>(() => server.Host = "something");
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void AllZerosSetsHostToPlusSign()
            {
                using (var server = new RestServer { Host = "0.0.0.0" })
                {
                    server.Host.ShouldBe("+");
                }
            }
        }

        public class IsListeningProperty
        {
            [Fact]
            public void ReturnsFalseWhenListenerIsNull()
            {
                IHttpListener listener = null;
                using (var server = new RestServer(listener))
                {
                    server.IsListening.ShouldBeFalse();
                }
            }
        }

        public class LoggerProperty
        {
            [Fact]
            public void NullSetNullLogger()
            {
                using (var server = new RestServer())
                {
                    server.LogToConsole();
                    server.Logger.ShouldBeOfType<ConsoleLogger>();

                    server.Logger = null;

                    server.Logger.ShouldBeOfType<NullLogger>();
                }
            }

            [Fact]
            public void SetsRouterLogger()
            {
                using (var server = new RestServer())
                {
                    server.Router.ShouldNotBeNull();
                    server.Logger.ShouldBeOfType<NullLogger>();
                    server.Router.Logger.ShouldBeOfType<NullLogger>();

                    server.LogToConsole();

                    server.Logger.ShouldBeOfType<ConsoleLogger>();
                    server.Router.Logger.ShouldBeOfType<ConsoleLogger>();
                }
            }

            [Fact]
            public void DoesNotSetRouterLoggerWhenRouterIsNull()
            {
                using (var server = new RestServer())
                {
                    server.Logger.ShouldBeOfType<NullLogger>();
                    server.Router = null;

                    Should.NotThrow(() => server.LogToConsole());

                    server.Logger.ShouldBeOfType<ConsoleLogger>();
                }
            }
        }

        public class OnStartPropery
        {
            [Fact]
            public void SynonymForOnAfterStart()
            {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                Action action = () => { var x = 2; };
#pragma warning restore CS0219 // Variable is assigned but its value is never used

                using (var server = new RestServer())
                {
                    server.OnAfterStart = action;
                    server.OnStart.ShouldBe(action);

                    server.OnStart = null;
                    server.OnAfterStart.ShouldBeNull();
                }
            }
        }

        public class OnStopProperty
        {
            [Fact]
            public void SynonymForOnAfterStop()
            {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                Action action = () => { var x = 2; };
#pragma warning restore CS0219 // Variable is assigned but its value is never used

                using (var server = new RestServer())
                {
                    server.OnAfterStop = action;
                    server.OnStop.ShouldBe(action);

                    server.OnStop = null;
                    server.OnAfterStop.ShouldBeNull();
                }
            }
        }

        public class PortProperty
        {
            [Fact]
            public void ThrowsExceptionWhenChangingWhileListenerIsListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                using (var server = new RestServer(listener))
                {
                    Should.Throw<ServerStateException>(() => server.Port = "5555");
                    listener.IsListening.Returns(false);
                }
            }
        }

        public class UseHttpsProperty
        {
            [Fact]
            public void ListenerPrefixBeginsWithHttpsWhenTrue()
            {
                using (var server = new RestServer())
                {
                    server.UseHttps = true;
                    server.UseHttps.ShouldBeTrue();
                    server.ListenerPrefix.StartsWith("https").ShouldBeTrue();
                }
            }

            [Fact]
            public void ListenerPrefixBeginsWithHttpsWhenFalse()
            {
                using (var server = new RestServer())
                {
                    server.UseHttps.ShouldBeFalse();
                    server.ListenerPrefix.StartsWith("http").ShouldBeTrue();
                    server.ListenerPrefix.StartsWith("https").ShouldBeFalse();
                }
            }

            [Fact]
            public void ThrowsExceptionWhenChangingWhileListenerIsListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                using (var server = new RestServer(listener))
                {
                    Should.Throw<ServerStateException>(() => server.UseHttps = true);
                    listener.IsListening.Returns(false);
                }
            }
        }

        public class CloneEventHandlersMethod
        {
            [Fact]
            public void ClonesBeforeStartHandlersInCorrectOrder()
            {
                var server1 = new RestServer();
                var server2 = new RestServer();
                var order = new List<string>();

                server1.BeforeStarting += rs => { order.Add("1"); };
                server1.BeforeStarting += rs => { order.Add("2"); };
                server1.CloneEventHandlers(server2);
                server2.GetType().GetMethod("OnBeforeStarting", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server2, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesAfterStartHandlersInCorrectOrder()
            {
                var server1 = new RestServer();
                var server2 = new RestServer();
                var order = new List<string>();

                server1.AfterStarting += rs => { order.Add("1"); };
                server1.AfterStarting += rs => { order.Add("2"); };
                server1.CloneEventHandlers(server2);
                server2.GetType().GetMethod("OnAfterStarting", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server2, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesBeforeStopHandlersInCorrectOrder()
            {
                var server1 = new RestServer();
                var server2 = new RestServer();
                var order = new List<string>();

                server1.BeforeStopping += rs => { order.Add("1"); };
                server1.BeforeStopping += rs => { order.Add("2"); };
                server1.CloneEventHandlers(server2);
                server2.GetType().GetMethod("OnBeforeStopping", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server2, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }

            [Fact]
            public void ClonesAfterStopHandlersInCorrectOrder()
            {
                var server1 = new RestServer();
                var server2 = new RestServer();
                var order = new List<string>();

                server1.AfterStopping += rs => { order.Add("1"); };
                server1.AfterStopping += rs => { order.Add("2"); };
                server1.CloneEventHandlers(server2);
                server2.GetType().GetMethod("OnAfterStopping", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(server2, null);

                order.Count.ShouldBe(2);
                order[0].ShouldBe("1");
                order[1].ShouldBe("2");
            }
        }

        public class EventHandlers
        {
            [Fact]
            public void BeforeStartingThrowsAggregateException()
            {
                var listener = Substitute.For<IHttpListener>();
                var order = new List<int>();

                using (var server = new RestServer(listener))
                {
                    server.BeforeStarting += restServer => { order.Add(1); throw new Exception("blah"); };
                    server.BeforeStarting += restServer => { order.Add(2); };
                    server.AfterStarting += restServer => { order.Add(3); };

                    try
                    {
                        server.Start();
                    }
                    catch (Exception e)
                    {
                        e.InnerException.ShouldNotBeNull();
                        e.InnerException?.GetType().ShouldBe(typeof(AggregateException));
                    }
                }

                order.Count.ShouldBe(2);
                order.Contains(3).ShouldBeFalse();
            }

            [Fact]
            public void AfterStartingThrowsAggregateException()
            {
                var listener = Substitute.For<IHttpListener>();
                var order = new List<int>();

                using (var server = new RestServer(listener))
                {
                    server.BeforeStarting += restServer => { order.Add(1); listener.IsListening.Returns(true); };
                    server.AfterStarting += restServer => { order.Add(2); throw new Exception("blah"); };
                    server.AfterStarting += restServer => { order.Add(3); };

                    try
                    {
                        server.Start();
                    }
                    catch (Exception e)
                    {
                        e.InnerException.ShouldNotBeNull();
                        e.InnerException?.GetType().ShouldBe(typeof(AggregateException));
                    }

                    listener.IsListening.Returns(false);
                }

                order.Count.ShouldBe(3);
            }

            [Fact]
            public void BeforeStoppingThrowsAggregateException()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                var order = new List<int>();

                using (var server = new RestServer(listener))
                {
                    server.BeforeStopping += restServer => { order.Add(1); throw new Exception("blah"); };
                    server.BeforeStopping += restServer => { order.Add(2); };
                    server.AfterStopping += restServer => { order.Add(3); };

                    try
                    {
                        server.Stop();
                    }
                    catch (Exception e)
                    {
                        e.InnerException.ShouldNotBeNull();
                        e.InnerException?.GetType().ShouldBe(typeof(AggregateException));
                        listener.IsListening.Returns(false);
                    }
                }

                order.Count.ShouldBe(2);
                order.Contains(3).ShouldBeFalse();
            }

            [Fact]
            public void AfterStoppingThrowsAggregateException()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                var order = new List<int>();

                using (var server = new RestServer(listener))
                {
                    server.BeforeStopping += restServer => { order.Add(1); listener.IsListening.Returns(false); };
                    server.AfterStopping += restServer => { order.Add(2); throw new Exception("blah"); };
                    server.AfterStopping += restServer => { order.Add(3); };

                    try
                    {
                        server.Stop();
                    }
                    catch (Exception e)
                    {
                        e.InnerException.ShouldNotBeNull();
                        e.InnerException?.GetType().ShouldBe(typeof(AggregateException));
                    }

                    listener.IsListening.Returns(false);
                }

                order.Count.ShouldBe(3);
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void CallsCloseOnListener()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(false);
                var server = new RestServer(listener);

                server.Dispose();

                listener.Received().Close();
            }

            [Fact]
            public void DoesNotCallCloseOnListenerWhenListenerIsNull()
            {
                IHttpListener listener = null;
                var server = new RestServer(listener);

                Should.NotThrow(() => server.Dispose());
            }

            [Fact]
            public void CallsStopWhenListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                var server = Substitute.For<RestServer>(listener);

                server.Dispose();

                server.Received().Stop();
            }

            [Fact]
            public void DoesNotCallsStopWhenNotListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(false);
                var server = Substitute.For<RestServer>(listener);

                server.Dispose();

                server.DidNotReceive().Stop();
            }
        }

        public class StartMethod
        {
            [Fact]
            public void AbortsWhenAlreadyStarted()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    server.Start();
                    listener.DidNotReceive().Start();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void AbortsWhenAlreadyStarting()
            {
                var listener = Substitute.For<IHttpListener>();

                using (var server = new RestServer(listener))
                {
                    server.SetIsStarting(true);
                    server.Start();
                    listener.DidNotReceive().Start();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void ThrowsExceptionWhenStopping()
            {
                using (var server = new RestServer())
                {
                    server.SetIsStopping(true);
                    Should.Throw<UnableToStartHostException>(() => server.Start());
                }
            }

            [Fact]
            public void ThrowsExceptionWhenExceptionOccursWhileStarting()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.When(_ => _.Start()).Do(_ => { throw new Exception(); });

                using (var server = new RestServer(listener))
                {
                    Should.Throw<UnableToStartHostException>(() => server.Start());
                    server.GetIsStarting().ShouldBeFalse();
                }
            }

            [Fact]
            public void OnBeforeStartExecuted()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();
                listener.When(l => l.Start()).Do(info => listener.IsListening.Returns(invoked));

                using (var server = new RestServer(listener) { OnBeforeStart = () => { invoked = true; } })
                {
                    server.Start();
                    invoked.ShouldBeTrue();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void ScansAssemblyCalledWhenRoutingTableEmpty()
            {
                var invoked = false;

                var listener = Substitute.For<IHttpListener>();
                listener.When(l => l.Start()).Do(info => listener.IsListening.Returns(invoked));

                var router = Substitute.For<IRouter>();
                router.RoutingTable.Returns(new List<IRoute>());

                using (var server = new RestServer(listener) { OnBeforeStart = () => { invoked = true; }, Router = router })
                {
                    server.Start();
                    router.Received().ScanAssemblies();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void ScansAssemblyNotCalledWhenRoutingTableEmpty()
            {
                var invoked = false;

                var listener = Substitute.For<IHttpListener>();
                listener.When(l => l.Start()).Do(info => listener.IsListening.Returns(invoked));

                var router = Substitute.For<IRouter>();
                router.RoutingTable.Returns(new List<IRoute> { new Route(context => context) });

                using (var server = new RestServer(listener) { OnBeforeStart = () => { invoked = true; }, Router = router })
                {
                    server.Start();
                    router.DidNotReceive().ScanAssemblies();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void OnAfterStartExecutesWhenListening()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(false);
                listener.When(l => l.Start()).Do(info => listener.IsListening.Returns(true));

                using (var server = new RestServer(listener) { OnAfterStart = () => { invoked = true; } })
                {
                    server.Start();
                    invoked.ShouldBeTrue();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void OnAfterStartNotExecutedWhenNotListening()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();

                using (var server = new RestServer(listener) { OnAfterStart = () => { invoked = true; } })
                {
                    server.Start();
                    invoked.ShouldBeFalse();
                }
            }
        }

        public class StopMethod
        {
            [Fact]
            public void AbortsWhenAlreadyStopped()
            {
                var listener = Substitute.For<IHttpListener>();

                using (var server = new RestServer(listener))
                {
                    server.Stop();
                    listener.DidNotReceive().Stop();
                }
            }

            [Fact]
            public void AbortsWhenAlreadyStopping()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    server.SetIsStopping(true);
                    server.Stop();
                    listener.DidNotReceive().Stop();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void ThrowsExceptionWhenStarting()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    server.SetIsStarting(true);
                    Should.Throw<UnableToStopHostException>(() => server.Stop());
                    listener.IsListening.Returns(false);
                    server.SetIsStarting(false);
                }
            }

            [Fact]
            public void ThrowsExceptionWhenExceptionOccursWhileStopping()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.When(_ => _.Stop()).Do(_ => { throw new Exception(); });
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    Should.Throw<UnableToStopHostException>(() => server.Stop());
                    server.GetIsStopping().ShouldBeFalse();
                    listener.IsListening.Returns(false);
                }
            }

            [Fact]
            public void OnBeforeStopExecuted()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                listener.When(l => l.Stop()).Do(info => listener.IsListening.Returns(false));

                using (var server = new RestServer(listener) { OnBeforeStop = () => { invoked = true; } })
                {
                    server.Stop();
                    invoked.ShouldBeTrue();
                }
            }

            [Fact]
            public void OnAfterStopExecutesWhenNotListening()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);
                listener.When(l => l.Stop()).Do(info => listener.IsListening.Returns(false));

                using (var server = new RestServer(listener) { OnAfterStop = () => { invoked = true; } })
                {
                    server.Stop();
                    invoked.ShouldBeTrue();
                }
            }

            [Fact]
            public void OnAfterStopNotExecutedWhenListening()
            {
                var invoked = false;
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener) { OnAfterStop = () => { invoked = true; } })
                {
                    server.Stop();
                    invoked.ShouldBeFalse();
                    listener.IsListening.Returns(false);
                }
            }
        }
    }

    public static class RestServerExtensions
    {
        internal static void SetIsStopping(this RestServer server, bool val)
        {
            var memberInfo = server.GetType();
            var field = memberInfo?.GetField("IsStopping", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(server, val);
        }

        internal static bool GetIsStopping(this RestServer server)
        {
            var memberInfo = server.GetType();
            var field = memberInfo?.GetField("IsStopping", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)field?.GetValue(server);
        }

        internal static void SetIsStarting(this RestServer server, bool val)
        {
            var memberInfo = server.GetType();
            var field = memberInfo?.GetField("IsStarting", BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(server, val);
        }

        internal static bool GetIsStarting(this RestServer server)
        {
            var memberInfo = server.GetType();
            var field = memberInfo?.GetField("IsStarting", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)field?.GetValue(server);
        }
    }

    public class CustomSettings : ServerSettings
    {
        public CustomSettings()
        {
            Port = "5555";
            OnBeforeStart = () =>
            {
                UseHttps = true;
            };
        }
    }
}
