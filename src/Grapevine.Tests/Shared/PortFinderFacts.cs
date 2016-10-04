using System;
using System.Linq;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class PortFinderFacts
    {
        public class ConstructorMethods
        {
            [Fact]
            public void DefaultsStartAndEndIndicies()
            {
                var finder = new PortFinder();
                finder.StartIndex.ShouldBe(PortFinder.FirstPort);
                finder.EndIndex.ShouldBe(PortFinder.LastPort);
            }

            [Fact]
            public void DefaultsEndIndex()
            {
                var finder = new PortFinder(200);
                finder.StartIndex.ShouldBe(200);
                finder.EndIndex.ShouldBe(PortFinder.LastPort);
            }
        }

        public class StartIndexProperty
        {
            [Fact]
            public void ThrowsExceptionWhenStartIndexIsLessThanFirstPort()
            {
                var finder = new PortFinder();
                Should.Throw<ArgumentOutOfRangeException>(() => finder.StartIndex = PortFinder.FirstPort - 1);
            }
        }

        public class EndIndexProperty
        {
            [Fact]
            public void ThrowsExceptionWhenEndIndexIsGreaterThanLastPort()
            {
                var finder = new PortFinder();
                Should.Throw<ArgumentOutOfRangeException>(() => finder.EndIndex = PortFinder.LastPort + 1);
            }
        }

        public class RunMethod
        {
            [Fact]
            public void FindsOpenPortUsingStartAndEndIndicies()
            {
                const int start = 5000;
                const int end = 6000;

                var finder = new PortFinder(start, end);
                var port = finder.Run();

                port.ShouldNotBeNull();
                var iport = int.Parse(port);
                iport.ShouldBeGreaterThanOrEqualTo(start);
                iport.ShouldBeLessThanOrEqualTo(end);
            }
        }

        public class Statics
        {
            public class FindNextLocalOpenPortMethod
            {
                [Fact]
                public void ThrowsExceptionWhenIndexIsOutOfRange()
                {
                    Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort - 1));
                    Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.LastPort + 1));
                    Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort - 1, PortFinder.LastPort));
                    Should.Throw<ArgumentOutOfRangeException>(() => PortFinder.FindNextLocalOpenPort(PortFinder.FirstPort, PortFinder.LastPort + 1));
                }

                [Fact]
                public void WillInvertStartAndEndIndex()
                {
                    const int start = 4000;
                    const int end = 5000;

                    var port = int.Parse(PortFinder.FindNextLocalOpenPort(end, start));

                    port.ShouldBeGreaterThanOrEqualTo(start);
                    port.ShouldBeLessThanOrEqualTo(end);
                }

                [Fact]
                public void FindsOpenPort()
                {
                    var port = PortFinder.FindNextLocalOpenPort();
                    port.ShouldNotBeNull();
                }

                [Fact]
                public void FindsOpenPortInDefaultRange()
                {
                    var port = PortFinder.FindNextLocalOpenPort();
                    port.ShouldNotBeNull();
                }

                [Fact]
                public void FindsOpenPortUsingCustomStartIndex()
                {
                    var start = int.Parse(PortFinder.FindNextLocalOpenPort()) + 1;
                    var port = PortFinder.FindNextLocalOpenPort(start);

                    port.ShouldNotBeNull();
                    int.Parse(port).ShouldBeGreaterThanOrEqualTo(start);
                }

                [Fact]
                public void FindsOpenPortUsingCustomRange()
                {
                    const int start = 5000;
                    const int end = 10000;

                    var port = int.Parse(PortFinder.FindNextLocalOpenPort(start, end));

                    port.ShouldBeGreaterThanOrEqualTo(start);
                    port.ShouldBeLessThanOrEqualTo(end);
                }

                [Fact]
                public void ReturnsZeroWhenNoPortIsFoundInRange()
                {
                    var port = PortFinder.PortsInUse.First();
                    PortFinder.FindNextLocalOpenPort(port, port).ShouldBe("0");
                }
            }

            public class PortsInUseProperty
            {
                [Fact]
                public void ContainsListOfUsedPorts()
                {
                    PortFinder.PortsInUse.Count.ShouldBeGreaterThan(0);
                }
            }
        }
    }
}
