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
            var server  = new ExtendedServer();
            server.Start();

            var client  = new RestClient(server.BaseUrl);
            RestResponse response; 
            RestRequest[] requests = new RestRequest[]
            {
                new RestRequest("/foo/{id}", ContentType.TXT),
                new RestRequest(HttpMethod.POST, "/foo/bar", ContentType.TXT)
            };

            foreach (RestRequest request in requests)
            {
                switch (request.Method)
                {
                    case HttpMethod.GET:
                        request.AddParameter("id", "d");
                        break;
                    case HttpMethod.POST:
                        request.Payload = "Payload is optional";
                        break;
                }

                response = client.Execute(request);

                Console.WriteLine(response.StatusCode + " : " + response.ElapsedTime + " : " + response.Content);
                Console.WriteLine();
            }

            server.Stop();

            Console.WriteLine("Press Any Key to Continue...");
            Console.ReadLine();
        }
    }
}
