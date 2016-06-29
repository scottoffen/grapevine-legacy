using System.Security.Policy;
using System.Threading;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Util
{
    public class PortFinderTester
    {
        [Fact]
        public void finder_finds_open_port()
        {
            var port = PortFinder.FindNextLocalOpenPort();
            port.ShouldNotBeNull();
        }

        [Fact]
        public void finder_finds_open_port_using_start()
        {
            var start = int.Parse(PortFinder.FindNextLocalOpenPort()) + 1;
            var port = PortFinder.FindNextLocalOpenPort(start);

            port.ShouldNotBeNull();
            int.Parse(port).ShouldBeGreaterThanOrEqualTo(start);
        }

        [Fact]
        public void finder_finds_open_port_using_start_and_end()
        {
            var start = 5000;
            var end = 10000;
            var port = PortFinder.FindNextLocalOpenPort(start, end);

            port.ShouldNotBeNull();
            var iport = int.Parse(port);
            iport.ShouldBeGreaterThanOrEqualTo(start);
            iport.ShouldBeLessThanOrEqualTo(end);
        }

        [Fact]
        public void finder_finds_open_port_using_inverted_start_and_end()
        {
            var start = 10000;
            var end = 5000;
            var port = PortFinder.FindNextLocalOpenPort(start, end);

            port.ShouldNotBeNull();
            var iport = int.Parse(port);
            iport.ShouldBeGreaterThanOrEqualTo(end);
            iport.ShouldBeLessThanOrEqualTo(start);
        }

        [Fact]
        public void finder_instance_finds_open_port()
        {
            var start = 5000;
            var end = 6000;

            var finder = new PortFinder(start, end);
            var port = finder.Run();

            port.ShouldNotBeNull();
            var iport = int.Parse(port);
            iport.ShouldBeGreaterThanOrEqualTo(start);
            iport.ShouldBeLessThanOrEqualTo(end);
        }

        [Fact]
        public void finder_instance_finds_open_port_async()
        {
            var start = 5000;
            var end = 6000;
            var port = string.Empty;
            var nf = false;

            var finder = new PortFinder(start, end);
            finder.PortFound += str => { port = str; };
            finder.PortNotFound += () => { nf = true; };

            finder.RunAsync();
            while (nf == false && string.IsNullOrEmpty(port))
            {
                Thread.Sleep(300);
            }

            if (nf == false)
            {
                port.ShouldNotBeNullOrWhiteSpace();
                var iport = int.Parse(port);
                iport.ShouldBeGreaterThanOrEqualTo(start);
                iport.ShouldBeLessThanOrEqualTo(end);
            }
        }
    }
}
