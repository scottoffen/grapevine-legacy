using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util.Loggers
{
    public class LoggerExtensionsTester
    {
        [Fact]
        public void logger_begin_routing_message_gets_logged()
        {
            var message = "Test Message";
            var expected = $"Routing Request  : {message}";
            var logger = new InMemoryLogger();

            logger.BeginRouting(message);
            logger.Logs[0].Message.ShouldBe(expected);
        }

        [Fact]
        public void logger_routing_complete_message_gets_logged()
        {
            var message = "Test Message";
            var expected = $"Routing Complete : {message}";
            var logger = new InMemoryLogger();

            logger.EndRouting(message);
            logger.Logs[0].Message.ShouldBe(expected);
        }

        [Fact]
        public void logger_route_invoked_message_gets_logged()
        {
            var message = "Test Message";
            var expected = $"Route Invoked    : {message}";
            var logger = new InMemoryLogger();

            logger.RouteInvoked(message);
            logger.Logs[0].Message.ShouldBe(expected);
        }
    }
}