using Grapevine.Shared.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared.Loggers
{
    public class LoggerExtensionsFacts
    {
        public static string MessageText = "Test Message";

        [Fact]
        public void BeginRoutingMessageLogged()
        {
            var expected = $"Routing Request  : {MessageText}";
            var logger = new InMemoryLogger();

            logger.BeginRouting(MessageText);
            logger.Logs[0].Message.ShouldBe(expected);
        }

        [Fact]
        public void RoutingCompleteMessageLogged()
        {
            var expected = $"Routing Complete : {MessageText}";
            var logger = new InMemoryLogger();

            logger.EndRouting(MessageText);
            logger.Logs[0].Message.ShouldBe(expected);
        }

        [Fact]
        public void RouteInvokedMessageLogged()
        {
            var expected = $"Route Invoked    : {MessageText}";
            var logger = new InMemoryLogger();

            logger.RouteInvoked(MessageText);
            logger.Logs[0].Message.ShouldBe(expected);
        }
    }
}
