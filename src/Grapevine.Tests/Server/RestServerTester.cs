using Xunit;
using Shouldly;
using Grapevine.Server;

namespace Grapevine.Tests.Server
{
    public class RestServerTester
    {
        [Fact]
        public void server_usehttps_is_true_then_listenerprefix_begins_with_https()
        {
            var server = new RestServer();
            server.ShouldNotBeNull();

            server.UseHttps = true;

            server.UseHttps.ShouldBeTrue();
            server.ListenerPrefix.StartsWith("https").ShouldBeTrue();
        }

        [Fact]
        public void server_usehttps_is_false_then_listenerprefix_begins_with_http()
        {
            var server = new RestServer();
            server.ShouldNotBeNull();

            server.UseHttps.ShouldBeFalse();
            server.ListenerPrefix.StartsWith("http").ShouldBeTrue();
            server.ListenerPrefix.StartsWith("https").ShouldBeFalse();
        }
    }
}
