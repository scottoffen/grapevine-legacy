using Grapevine.Server;

namespace Grapevine.Tests.Server.Helpers
{
    class RestServerTesterHelper
    {
    }

    public class CustomSettings : ServerSettings
    {
        public CustomSettings()
        {
            Port = "5555";
            OnBeforeStart = () =>
            {
                UseHttps = true;
            };
        }
    }
}
