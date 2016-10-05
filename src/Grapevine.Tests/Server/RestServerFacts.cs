using System;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
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
                    server.PublicFolder.ShouldBeOfType<PublicFolder>();
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
                    _.PublicFolder.DefaultFileName = index;
                }))
                {
                    server.Port.ShouldBe(port);
                    server.PublicFolder.DefaultFileName.ShouldBe(index);
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

        public class ConnectionsProperty
        {
            [Fact]
            public void ThrowsExceptionWhenChangingWhileListenerIsListening()
            {
                var listener = Substitute.For<IHttpListener>();
                listener.IsListening.Returns(true);

                using (var server = new RestServer(listener))
                {
                    Should.Throw<ServerStateException>(() => server.Connections = 5);
                    listener.IsListening.Returns(false);
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
                //var listener = Substitute.For<IHttpListener>();
                //listener.IsListening.Returns(true);
                //var server = Substitute.For<RestServer>(listener);

                //server.Dispose();

                //server.Received().Stop();
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
            //[Fact]
            //public void server_start_does_not_start_when_islistening_is_true()
            //{
            //    var listener = Substitute.For<IHttpListener>();
            //    listener.IsListening.Returns(true);

            //    using (var server = new RestServer(listener))
            //    {
            //        server.Start();
            //        listener.DidNotReceive().Start();
            //    }
            //}

            //[Fact]
            //public void server_start_does_not_start_when_isstarting_is_true()
            //{
            //    var listener = Substitute.For<IHttpListener>();

            //    using (var server = new RestServer(listener))
            //    {
            //        server.SetIsStarting(true);
            //        server.Start();
            //        listener.DidNotReceive().Start();
            //    }
            //}

            //[Fact]
            //public void server_start_throws_exception_when_server_is_stopping()
            //{
            //    using (var server = new RestServer())
            //    {
            //        server.SetIsStopping(true);
            //        Should.Throw<UnableToStartHostException>(() => server.Start());
            //    }
            //}

            //[Fact]
            //public void server_start_throws_exception_when_exception_occurs_while_starting()
            //{
            //    var listener = Substitute.For<IHttpListener>();
            //    listener.When(_ => _.Start()).Do(_ => { throw new Exception(); });

            //    using (var server = new RestServer(listener))
            //    {
            //        Should.Throw<UnableToStartHostException>(() => server.Start());
            //        server.GetIsStarting().ShouldBeFalse();
            //    }
            //}

            //[Fact]
            //public void server_start_executes_on_before_start()
            //{
            //    //var invoked = false;

            //    //var listener = MockRepository.Mock<IHttpListener>();

            //    //listener.Stub(l => l.IsListening).Returns(() => invoked);
            //    //listener.Stub(l => l.Prefixes).Return(prefixes);

            //    //var server = new RestServer(listener)
            //    //{
            //    //    Connections = 1,
            //    //    Port = PortFinder.FindNextLocalOpenPort(),
            //    //    OnBeforeStart = () => { invoked = true; }
            //    //};

            //    //server.Start();

            //    //invoked.ShouldBeTrue();
            //}

            //[Fact]
            //public void server_start_executes_on_after_start()
            //{
            //    var invoked = false;
            //    var listener = Substitute.For<IHttpListener>();
            //    listener.When(_ => _.Start()).Do(_ => { listener.IsListening.Returns(true); });

            //    using (var server = new RestServer(listener))
            //    {
            //        server.Connections = 1;
            //        server.OnAfterStart = () => invoked = true;
            //        server.Start();
            //        server.GetIsStarting().ShouldBeFalse();
            //        invoked.ShouldBeTrue();
            //    }
            //}
        }

        public class StopMethod
        {
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
