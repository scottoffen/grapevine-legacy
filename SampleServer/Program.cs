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
            server.Host = "*";
            server.Port = "4567";
            server.MaxThreads = 100;
            server.WebRoot = "";

            server.Start();
            while (server.IsListening)
            {
                Thread.Sleep(500);
            }

            Environment.Exit(0);
        }
    }
}
