using Xunit;
using Shouldly;
using Grapevine.Server;

namespace Grapevine.Test.Server
{
    public class RestServerTester
    {
        [Fact]
        public void first_test()
        {
            var server = new RestServer();
            server.ShouldNotBeNull();
        }
    }
}
