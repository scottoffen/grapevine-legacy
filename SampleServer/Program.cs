using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Thread.Sleep(500);
            }
            server.Stop();
//            Environment.Exit(0);
        }
    }
}
