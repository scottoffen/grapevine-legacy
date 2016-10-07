using System.Security.Policy;
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
        }

        public class GetMethod
        {
        }

        public class RemoveMethod
        {
        }

        public class StartAllMethod
        {
        }

        public class StopAllMethod
        {
        }


    }
}
