Grapevine REST/HTTP Server
==========================

![](https://raw.github.com/scottoffen/Grapevine/master/grapevine.png)

**Current Version: 2.0**

Grapevine provides a framework for quickly and easily creating multithreaded .NET HTTP endpoints using the ubiquitous [HttpListener](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistener(v=vs.100)) class and custom [attributes](http://msdn.microsoft.com/en-us/library/sw480ze8.aspx).  Grapevine makes it equally simple to serve up static files and REST services.

###Use Case for Grapevine###
Like the [informal means of communication](http://en.wikipedia.org/wiki/Grapevine_(gossip)) its name alludes to, Grapevine is designed for use in application for which being an HTTP server or responding to REST requests is not the primary function or purpose of the application, but rather as a secondary means of communication with the application.

For example, a Widows Forms application or Windows Service would be the primary means of communication, and having an object (or several) that extends Grapevine listening on a particular port would be your secondary means of communication - even if you plan on using Grapevine to expose the majority of your functionality.

###Install Grapevine via NuGet###
Grapevine is available to install via [NuGet](https://www.nuget.org/packages/Grapevine/):

    > Install-Package Grapevine

###Dependencies###
Grapevine has a dependency on [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) v. 5.0.8 or higher.  While it has been successfully tested with version 6.0.2, any future changes to Json.NET could introduce error or incompatibilities in Grapevine.  If you experience any issues with a newer release of Json.NET, please [log an issue](https://github.com/scottoffen/Grapevine/issues?state=open) (make sure one doesn't already exist first, of course).

##Features##
- Flexible : Grapevine just listens, you provide the responses. You can even have one method handle multiple request types. If no handler exists for the request, it looks for a file at the path specified.  If there is no file, it handles returning the errors.  You only have to worry about the happy path!

- Fast : Grapevine accepts incoming http requests and spins them off to be handled by another thread.  As a result, there is no blocking I/O; the server is always ready to respond to incoming requests.

- Constant : The [message context](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistenercontext(v=vs.110).aspx) is passed to your handler methods - you get all of the data all of the time so you can decide how best to respond.

- Spontaneous : Grapevine searches your class for the best handler, no need to "register" new ones.  You can add files to the webroot to be served on-the-fly - no need to restart the server.  You can even write a custom handler to shut down your server remotely!

##Usage##
Grapevine provides the [HttpHandler](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/HttpHandler.cs) abstract class and the [Handler](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/Handler.cs) custom attribute.  Simply create a class that extends HttpHandler, and annotate the appropriate handler methods with the Handler attribute.  **No methods to implement!**

Attribute values default to Method = `HttpMethod.GET` and PathInfo = `"/"`, so for a catch-all method you don't need to define anything.

###Rest Server Example###
An example of a simple REST server that responds to GET requests on `http://localhost:1234/foo/5678`.

    using System.Net;
    using Grapevine;
    
    namespace SampleServer
    {
        class RestServer : HttpHandler
        {
            [Handler(Method = HttpMethod.GET, PathInfo = @"^/foo/\d+$")]
            public void HandleFoo(HttpListenerContext context)
            {
    			// code to handle foo goes here
                this.SendResponse(context, "Foo was handled successfully");
            }
        }
    }

In your main thread, spin up your server like so:

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

###Rest Client Example###
You can also communicate with REST servers (or test your own) using the include `Grapevine.REST` namespace:

    using System;
	using System.Threading;
	using Grapevine.REST;
	
	namespace SampleServer
	{
	    class Program
	    {
	        static void Main(string[] args)
	        {
	            var server  = new RestServer();
	            var counter = 0;
	            var max     = 5;
	
	            server.Start();
	            while (max > counter)
	            {
	                counter++;
	
	                var request = new RestRequest("/foo/{id}");
	                request.AddParameter("id", "1234");
	                request.SetContentType(RequestContentType.TEXT);
	
	                var client = new RestClient(server.BaseUrl);
	                var response = client.Execute(request);
	
	                Console.WriteLine(counter + " : " + response.StatusCode + " : " + response.ElapsedTime);
	                Console.WriteLine();
	
	                Thread.Sleep(100);
	            }
	            server.Stop();
	
	            Console.WriteLine("Press Any Key to Continue...");
	            Console.ReadLine();
	        }
	    }
	}

###Checkout the Cookbook###
See the [**cookbook**](https://github.com/scottoffen/Grapevine/wiki) for more examples, including how to change the host, port, number of threads and webroot directory.

##Limitations##
- Grapevine is **not** intended to be a drop-in replacement for [Microsoft IIS](http://www.iis.net/) or [Apache HTTP Server](http://httpd.apache.org/).  Instead, Grapevine aims to be embedded in your application, where using one of those would be impossible, or just plain overkill.

- Grapevine does not support **ASP.NET** nor does it do any script parsing (**PHP**, **Perl**, **Python**, **Ruby**, etc.) by default - but feel free to fork this project and hack away at it to your hearts content.

- A single instance will only listen on one host/port combination (unless you define the host as "*").

- You will likely be required to [open a port in your firewall](http://www.lmgtfy.com/?q=how+to+open+a+port+on+windows) for remote computers to be able to send requests to your application. Grapevine will not [automatically](http://msdn.microsoft.com/en-us/library/aa366418%28VS.85%29.aspx) do that for you.  You might want to do that during the [installation of your application](http://www.codeproject.com/Articles/14906/Open-Windows-Firewall-During-Installation).

##License##
Copyright 2014 Scott Offen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at [apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

"Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
