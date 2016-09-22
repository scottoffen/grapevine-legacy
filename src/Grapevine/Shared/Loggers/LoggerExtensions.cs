using Grapevine.Interfaces.Shared;

namespace Grapevine.Shared.Loggers
{
    public static class LoggerExtensions
    {
        public static void BeginRouting (this IGrapevineLogger logger, string message)
        {
            logger.Info($"Routing Request  : {message}");
        }

        public static void EndRouting(this IGrapevineLogger logger, string message)
        {
            logger.Trace($"Routing Complete : {message}");
        }

        public static void RouteInvoked(this IGrapevineLogger logger, string message)
        {
            logger.Trace($"Route Invoked    : {message}");
        }
    }
}