using Grapevine.Server;

namespace Grapevine.TestAssembly
{
    public class DefaultRouter
    {
        public static IRouter GetInstance()
        {
            return new Router();
        }
    }
}
