using Grapevine.Interfaces.Server;
using Grapevine.Server;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RestClusterFacts
    {
        [Fact]
        public void Indexer()
        {
            var cluster = new RestCluster();
            cluster.ShouldNotBeNull();
            cluster.Count.ShouldBe(0);

            using (var server = new RestServer(Substitute.For<IHttpListener>()))
            {
                cluster["first"] = server;
                cluster.Count.ShouldBe(1);
                cluster["first"].ShouldBe(server);
            }
        }

        public class AddMethod
        {
            [Fact]
            public void AddWithLabel()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();
                server.ListenerPrefix.Returns("blah");

                cluster.Add("stub", server);

                cluster.Count.ShouldBe(1);
                cluster["stub"].ShouldNotBeNull();
                cluster["blah"].ShouldBeNull();

                server.DidNotReceive().Start();
            }

            [Fact]
            public void AddWithOutLabel()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();
                server.ListenerPrefix.Returns("blah");

                cluster.Add(server);

                cluster.Count.ShouldBe(1);
                cluster["blah"].ShouldNotBeNull();

                server.DidNotReceive().Start();
            }

            [Fact]
            public void AddStartsWhenClusterIsStarted()
            {
                var cluster = new RestCluster();
                cluster.StartAll();

                var server = Substitute.For<IRestServer>();
                cluster.Add("some", server);

                server.Received().Start();
            }

            [Fact]
            public void AddInvokesDelegatesWhenClusterIsStarted()
            {
                var before = false;
                var after = false;

                var cluster = new RestCluster();
                cluster.StartAll();

                cluster.OnBeforeStartEach = srvr => { before = true; };
                cluster.OnAfterStartEach = srvr => { after = true; };

                var server = Substitute.For<IRestServer>();
                cluster.Add("some", server);

                before.ShouldBeTrue();
                after.ShouldBeTrue();
            }
        }

        public class GetMethod
        {
            [Fact]
            public void ReturnsNullWhenLabelNotFound()
            {
                var cluster = new RestCluster();
                cluster.Get("x").ShouldBeNull();
            }

            [Fact]
            public void ReturnsServerWhenFound()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();

                cluster.Add("x", server);

                cluster.Get("x").ShouldBe(server);
            }
        }

        public class RemoveMethod
        {
            [Fact]
            public void RemoveReturnsWhenLabelNotFound()
            {
                var before = false;

                var cluster = new RestCluster();
                cluster.StartAll();

                cluster.OnBeforeStopEach = srvr => { before = true; };

                var server = Substitute.For<IRestServer>();
                cluster.Add("some", server);

                cluster.Remove("non-existent-label");
                    
                before.ShouldBeFalse();
            }

            [Fact]
            public void DoesNotStopIfClusterHasNotStarted()
            {
                var cluster = new RestCluster();

                var server = Substitute.For<IRestServer>();
                cluster.Add("x", server);

                cluster.Remove("x");
                server.DidNotReceive().Stop();
            }

            [Fact]
            public void StopsServerWhenClusterHasStarted()
            {
                var cluster = new RestCluster();
                cluster.StartAll();

                var server = Substitute.For<IRestServer>();
                cluster.Add("x", server);
                cluster.Remove("x");

                server.Received().Stop();
            }

            [Fact]
            public void InvokesDelegatesWhenClusterHasStarted()
            {
                var before = false;
                var after = false;

                var cluster = new RestCluster();
                cluster.StartAll();

                cluster.OnBeforeStopEach = srvr => { before = true; };
                cluster.OnAfterStopEach = srvr => { after = true; };

                var server = Substitute.For<IRestServer>();
                cluster.Add("x", server);

                cluster.Remove("x");

                before.ShouldBeTrue();
                after.ShouldBeTrue();
            }

        }

        public class StartAllMethod
        {
            [Fact]
            public void InvokesDelegates()
            {
                var beforeAll = false;
                var beforeEach = false;
                var afterAll = false;
                var afterEach = false;

                var cluster = new RestCluster();
                cluster.OnBeforeStartAll = () => { beforeAll = true; };
                cluster.OnBeforeStartEach = srvr => { beforeEach = true; };
                cluster.OnAfterStartAll = () => { afterAll = true; };
                cluster.OnAfterStartEach = srvr => { afterEach = true; };

                cluster.Add(Substitute.For<IRestServer>());

                beforeAll.ShouldBeFalse();
                beforeEach.ShouldBeFalse();
                afterAll.ShouldBeFalse();
                afterEach.ShouldBeFalse();

                cluster.StartAll();

                beforeAll.ShouldBeTrue();
                beforeEach.ShouldBeTrue();
                afterAll.ShouldBeTrue();
                afterEach.ShouldBeTrue();
            }

            [Fact]
            public void StartsServerWhenClusterStarts()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();

                cluster.Add(server);
                cluster.StartAll();

                server.Received().Start();
            }

            [Fact]
            public void DoesNotStartsServerUntilClusterStarts()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();

                cluster.Add(server);

                server.DidNotReceive().Start();
            }

            [Fact]
            public void DoesNotStartWhenAlreadyStarted()
            {
                var started = false;
                var cluster = new RestCluster();
                cluster.OnBeforeStartAll = () => { started = !started; };

                cluster.StartAll();
                cluster.StartAll();

                started.ShouldBeTrue();
            }
        }

        public class StopAllMethod
        {
            [Fact]
            public void InvokesDelegates()
            {
                var beforeAll = false;
                var beforeEach = false;
                var afterAll = false;
                var afterEach = false;

                var server = Substitute.For<IRestServer>();
                server.IsListening.Returns(true);

                var cluster = new RestCluster();
                cluster.OnBeforeStopAll = () => { beforeAll = true; };
                cluster.OnBeforeStopEach = srvr => { beforeEach = true; };
                cluster.OnAfterStopAll = () => { afterAll = true; };
                cluster.OnAfterStopEach = srvr => { afterEach = true; };

                cluster.Add("x", server);
                cluster.StartAll();

                beforeAll.ShouldBeFalse();
                beforeEach.ShouldBeFalse();
                afterAll.ShouldBeFalse();
                afterEach.ShouldBeFalse();

                cluster.StopAll();                

                beforeAll.ShouldBeTrue();
                beforeEach.ShouldBeTrue();
                afterAll.ShouldBeTrue();
                afterEach.ShouldBeTrue();
            }

            [Fact]
            public void StopsServerWhenClusterStops()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();
                server.IsListening.Returns(true);

                cluster.Add(server);
                cluster.StartAll();
                cluster.StopAll();

                server.Received().Stop();
            }

            [Fact]
            public void DoesNotStopServerWhenClusterHasAlreadyStopped()
            {
                var cluster = new RestCluster();
                var server = Substitute.For<IRestServer>();

                cluster.Add(server);

                server.DidNotReceive().Start();
            }

            [Fact]
            public void DoesNotStopWhenAlreadyStopped()
            {
                var stopped = false;
                var cluster = new RestCluster();
                cluster.OnBeforeStopAll = () => { stopped = !stopped; };

                cluster.StartAll();
                cluster.StopAll();
                cluster.StopAll();

                stopped.ShouldBeTrue();
            }
        }
    }
}
