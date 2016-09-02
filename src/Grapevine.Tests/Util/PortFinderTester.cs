using System;
using System.Linq;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class PortFinderTester
    {
        [Fact]
        public void finder_default_start_and_end_indicies()
        {
            var finder = new PortFinder();
            finder.StartIndex.ShouldBe(PortFinder.FirstPort);
            finder.EndIndex.ShouldBe(PortFinder.LastPort);
        }

        [Fact]
        public void finder_default_end_index()
        {
            var finder = new PortFinder(200);
            finder.StartIndex.ShouldBe(200);
            finder.EndIndex.ShouldBe(PortFinder.LastPort);
        }

        [Fact]
        public void finder_throws_exception_if_start_index_is_less_than_first_port()
        {
            var finder = new PortFinder();
            Should.Throw<ArgumentOutOfRangeException>(() => finder.StartIndex = PortFinder.FirstPort - 1);
        }

        [Fact]
        public void finder_throws_exception_if_end_index_is_greater_than_first_port()
        {
            var finder = new PortFinder();
            Should.Throw<ArgumentOutOfRangeException>(() => finder.EndIndex = PortFinder.LastPort + 1);
        }

        [Fact]
        public void finder_finds_open_port_using_start_and_end_indicies()
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
        public void static_finder_throws_exception_on_index_out_of_range()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort - 1));
            Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.LastPort + 1));
            Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort - 1, PortFinder.LastPort));
            Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort, PortFinder.LastPort + 1));
        }

        [Fact]
        public void static_finder_finds_port_when_start_and_end_index_are_inverted()
        {
            const int start = 4000;
            const int end = 5000;

            var port = int.Parse(PortFinder.FindNextLocalOpenPort(end, start));

            port.ShouldBeGreaterThanOrEqualTo(start);
            port.ShouldBeLessThanOrEqualTo(end);
        }

        [Fact]
        public void static_finder_finds_open_port()
        {
            var port = PortFinder.FindNextLocalOpenPort();
            port.ShouldNotBeNull();
        }

        [Fact]
        public void static_finder_finds_open_port_at_or_above_start_index()
        {
            var port = PortFinder.FindNextLocalOpenPort();
            port.ShouldNotBeNull();
        }

        [Fact]
        public void static_finder_finds_open_port_using_start()
        {
            var start = int.Parse(PortFinder.FindNextLocalOpenPort()) + 1;
            var port = PortFinder.FindNextLocalOpenPort(start);

            port.ShouldNotBeNull();
            int.Parse(port).ShouldBeGreaterThanOrEqualTo(start);
        }

        [Fact]
        public void static_finder_finds_open_port_using_start_and_end()
        {
            const int start = 5000;
            const int end = 10000;

            var port = int.Parse(PortFinder.FindNextLocalOpenPort(start, end));

            port.ShouldBeGreaterThanOrEqualTo(start);
            port.ShouldBeLessThanOrEqualTo(end);
        }

        [Fact]
        public void static_finder_does_not_find_open_port()
        {
            var port = PortFinder.PortsInUse.First();
            PortFinder.FindNextLocalOpenPort(port, port).ShouldBe("0");
        }

        [Fact]
        public void static_finder_contains_list_of_used_ports()
        {
            PortFinder.PortsInUse.Count.ShouldBeGreaterThan(0);
        }
    }
}
