using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Grapevine;

namespace SampleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an EventLog to log all error messages, and assign this log to a client or server to have all internal
            // exceptions logged to the event log.
            EventLog log = CreateOrGetEventLog();

            // Create an Extended Server
            var server  = new ExtendedServer();
            server.EventLog = log;
            server.Start();

            bool failGet = false;

            var client  = new RestClient(server.BaseUrl);
            client.EventLog = log;

            RestResponse response; 
            RestRequest[] requests = new RestRequest[]
            {
                new RestRequest("/foo/{id}/bobby", ContentType.TXT),
                new RestRequest("/foo/{id}", ContentType.TXT),
                new RestRequest(HttpMethod.POST, "/foo/bar", ContentType.TXT),
                new RestRequest(HttpMethod.DELETE, "/shutdown")
            };

            foreach (RestRequest request in requests)
            {
                switch (request.Method)
                {
                    case HttpMethod.GET:
                        request.AddParameter("id", (failGet) ? "123" : "xyz");
                        failGet = !failGet;
                        break;
                    case HttpMethod.POST:
                        request.Payload = "Payload is optional";
                        break;
                }

                response = client.Execute(request);

                Console.WriteLine(response.StatusCode + " : " + response.ElapsedTime + " : " + response.Content);
                Console.WriteLine();
            }

            // Use this loop to keep the main thread running until the server.IsListening flag
            // is set to false;
            //while (server.IsListening)
            //{
            //    Thread.Sleep(300);
            //}

            server.Stop();

            Console.WriteLine("Press Any Key to Continue...");
            Console.ReadLine();
        }

        public static EventLog CreateOrGetEventLog()
        {
            EventLog log = new EventLog();
            string source = "Sample Server";
            string logName = "General Log";

            // To check if a source exits and to create an event source in Windows Vista and later or Windows Server 2003, you must have administrative privileges.
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            log.Source = source;
            log.Log = logName;

            return log;
        }
    }
}
