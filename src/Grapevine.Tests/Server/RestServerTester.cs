using System;
using Xunit;
using Shouldly;
using Grapevine.Server;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Grapevine.Util.Loggers;

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
            var server = new RestServer();

            server.Start();
            server.IsListening.ShouldBeTrue();

            server.Stop();
            server.IsListening.ShouldBeFalse();
        }
    }
}
