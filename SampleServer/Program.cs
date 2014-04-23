using System;
using System.Net;
using System.Threading;
using Grapevine;

namespace SampleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server  = new SampleServer();
            var counter = 0;
            var max     = 5;

            server.Start();
            while (max > counter)
            {
                counter++;

                var client = new RestClient(server.BaseUrl);

                var request = new RestRequest("/foo/{id}");
                request.AddParameter("id", "1234");
                request.SetContentType(ContentType.TXT);

                var response = client.Execute(request);

                Console.WriteLine(counter + " : " + response.StatusCode + " : " + response.ElapsedTime + " : " + response.Content);
                Console.WriteLine();

                Thread.Sleep(100);
            }
            server.Stop();

            Console.WriteLine("Press Any Key to Continue...");
            Console.ReadLine();
        }
    }
}
