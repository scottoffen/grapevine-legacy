using System.Threading;

namespace SampleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new RestServer();

            server.Start();
            while (server.IsListening)
            {
                Thread.Sleep(100);
            }
            server.Stop();
        }
    }
}
