using System;
using System.Net;
using System.Security.Policy;
using Xunit;
using Shouldly;
using Grapevine.Server;
using Grapevine.Server.Exceptions;
using Grapevine.Server.Interfaces;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Rhino.Mocks;

namespace Grapevine.Tests.Server
{
    public class RestServerTester
    {
        [Fact]
        public void server_default_configuration()
        {
            var server = new RestServer();

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

        [Fact]
        public void server_on_stop_and_stop_synonyms()
        {
            Action action = () => { var x = 2; };

            var server = new RestServer();

            server.OnAfterStart = action;
            server.OnStart.ShouldBe(action);

            server.OnStart = null;
            server.OnAfterStart.ShouldBeNull();

            server.OnAfterStop = action;
            server.OnStop.ShouldBe(action);

            server.OnStop = null;
            server.OnAfterStop.ShouldBeNull();
        }

        [Fact]
        public void server_configures_using_for_generic_type()
        {
            var server = RestServer.For<CustomSettings>();
            server.Port.ShouldBe("5555");
        }

        [Fact]
        public void server_configures_using_action()
        {
            const string port = "5555";
            const string index = "default.htm";

            var server = RestServer.For(_ =>
            {
                 _.Port = port;
                _.PublicFolder.DefaultFileName = index;
            });

            server.Port.ShouldBe(port);
            server.PublicFolder.DefaultFileName.ShouldBe(index);
        }

        [Fact]
        public void server_listenerprefix_begins_with_https_when_usehttps_is_true()
        {
            var server = new RestServer();
            server.ShouldNotBeNull();

            server.UseHttps = true;

            server.UseHttps.ShouldBeTrue();
            server.ListenerPrefix.StartsWith("https").ShouldBeTrue();
        }

        [Fact]
        public void server_listenerprefix_begins_with_http_when_usehttps_is_false()
        {
            var server = new RestServer();
            server.ShouldNotBeNull();

            server.UseHttps.ShouldBeFalse();
            server.ListenerPrefix.StartsWith("http").ShouldBeTrue();
            server.ListenerPrefix.StartsWith("https").ShouldBeFalse();
        }

        [Fact]
        public void server_starts_and_stops()
        {
            //var server = new RestServer();

            //server.Start();
            //server.IsListening.ShouldBeTrue();

            //server.Stop();
            //server.IsListening.ShouldBeFalse();
        }

        [Fact]
        public void server_set_connections_throws_exception_when_value_assigned_while_islistening_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.IsListening).Return(true);
            var server = new RestServer(listener);
            Should.Throw<ServerStateException>(() => server.Connections = 2);
        }

        [Fact]
        public void server_set_host_throws_exception_when_value_assigned_while_islistening_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.IsListening).Return(true);
            var server = new RestServer(listener);
            Should.Throw<ServerStateException>(() => server.Host = "+");
        }

        [Fact]
        public void server_set_host_to_all_zeros_sets_host_to_plus()
        {
            var server = new RestServer {Host = "0.0.0.0"};
            server.Host.ShouldBe("+");
        }

        [Fact]
        public void server_islistening_returns_false_when_listener_is_null()
        {
            IHttpListener listener = null;
            var server = new RestServer(listener);
            server.IsListening.ShouldBeFalse();
        }

        [Fact]
        public void server_set_logger_to_null_set_null_logger()
        {
            var server = new RestServer();
            server.LogToConsole();
            server.Logger.ShouldBeOfType<ConsoleLogger>();

            server.Logger = null;

            server.Logger.ShouldBeOfType<NullLogger>();
        }

        [Fact]
        public void server_set_logger_sets_router_logger()
        {
            var server = new RestServer();
            server.Router.ShouldNotBeNull();
            server.Logger.ShouldBeOfType<NullLogger>();
            server.Router.Logger.ShouldBeOfType<NullLogger>();

            server.LogToConsole();

            server.Logger.ShouldBeOfType<ConsoleLogger>();
            server.Router.Logger.ShouldBeOfType<ConsoleLogger>();
        }

        [Fact]
        public void server_set_logger_does_not_set_router_logger_when_router_is_null()
        {
            var server = new RestServer();
            server.Logger.ShouldBeOfType<NullLogger>();
            server.Router = null;

            Should.NotThrow(() => server.LogToConsole());

            server.Logger.ShouldBeOfType<ConsoleLogger>();
        }

        [Fact]
        public void server_set_port_throws_exception_when_value_assigned_while_islistening_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.IsListening).Return(true);
            var server = new RestServer(listener);
            Should.Throw<ServerStateException>(() => server.Port = "5555");
        }

        [Fact]
        public void server_set_protocol_throws_exception_when_value_assigned_while_islistening_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.IsListening).Return(true);
            var server = new RestServer(listener);
            Should.Throw<ServerStateException>(() => server.UseHttps = true);
        }

        [Fact]
        public void server_start_does_not_start_when_islistening_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.IsListening).Return(true);
            var server = new RestServer(listener);

            server.Start();

            listener.AssertWasNotCalled(l => l.Start());
        }

        [Fact]
        public void server_start_does_not_start_when_isstarting_is_true()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            var server = new RestServer(listener);
            server.SetIsStarting(true);

            server.Start();

            listener.AssertWasNotCalled(l => l.Start());
        }

        [Fact]
        public void server_start_throws_exception_when_server_is_stopping()
        {
            var server = new RestServer();
            server.SetIsStopping(true);
            Should.Throw<UnableToStartHostException>(() => server.Start());
        }

        [Fact]
        public void server_start_throws_exception_when_exception_occurs_while_starting()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            listener.Stub(l => l.Start()).Throws<Exception>();
            var server = new RestServer(listener);

            Should.Throw<UnableToStartHostException>(() => server.Start());
            server.GetIsStarting().ShouldBeFalse();
        }

        [Fact]
        public void server_start_executes_on_before_start()
        {
            //var invoked = false;

            //var listener = MockRepository.Mock<IHttpListener>();

            //listener.Stub(l => l.IsListening).Returns(() => invoked);
            //listener.Stub(l => l.Prefixes).Return(prefixes);

            //var server = new RestServer(listener)
            //{
            //    Connections = 1,
            //    Port = PortFinder.FindNextLocalOpenPort(),
            //    OnBeforeStart = () => { invoked = true; }
            //};

            //server.Start();

            //invoked.ShouldBeTrue();
        }

        [Fact]
        public void server_start_executes_on_after_start()
        {
            var invoked = false;
            var server = new RestServer
            {
                OnBeforeStart = () =>
                {
                    invoked = true;
                    throw new Exception();
                }
            };

            Should.Throw<UnableToStartHostException>(() => server.Start());
            invoked.ShouldBeTrue();
            server.GetIsStarting().ShouldBeFalse();
        }








        [Fact]
        public void server_dispose_calls_listener_close()
        {
            var listener = MockRepository.Mock<IHttpListener>();
            var server = new RestServer(listener);

            server.Dispose();

            listener.AssertWasCalled(l => l.Close());
        }
    }
}
