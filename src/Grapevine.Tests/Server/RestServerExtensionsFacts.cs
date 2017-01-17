using System.Threading;
using Grapevine.Server;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RestServerExtensionsFacts
    {
        [Fact]
        public void StopsServer()
        {
            var stopped = new ManualResetEvent(false);
            var port = PortFinder.FindNextLocalOpenPort(1234);

            using (var server = new RestServer { Connections = 1, Port = port })
            {
                server.OnAfterStop += () => { stopped.Set(); };

                server.Start();
                server.IsListening.ShouldBeTrue();
                server.ListenerPrefix.ShouldBe($"http://localhost:{port}/");

                server.ThreadSafeStop();
                stopped.WaitOne(300, false);

                server.IsListening.ShouldBeFalse();
            }
        }
    }
}
