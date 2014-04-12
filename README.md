Grapevine
=========

![](https://raw.github.com/scottoffen/Grapevine/master/grapevine.png)

Grapevine provides a framework for quickly and easily creating multithreaded .NET HTTP endpoints using the ubiquitous [HttpListener](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistener(v=vs.100)) class and custom [attributes](http://msdn.microsoft.com/en-us/library/sw480ze8.aspx).  Grapevine makes it simple to serve up files and REST services equally well, providing all the functionality needed to create a robust backend for small- and medium-sized applications.

##Features##
[Grapevine](http://en.wikipedia.org/wiki/Grapevine_(gossip)#Features_of_Grapevine_Communication) is:

- Flexible : Grapevine just listens, you provide the responses. You can even have one method handle multiple request types. If no handler exists for the request, it looks for a file at the path specified.  If there is no file, it handles returning the errors.  You only have to worry about the happy path!

- Fast : Grapevine accepts incoming http requests and spins them off to be handled by another thread.  As a result, there is no blocking I/O; the server is always ready to respond to incoming requests.

- Constant : The [message context](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistenercontext(v=vs.110).aspx) is passed to your handler methods - you get all of the data all of the time so you can decide how best to respond.

- Spontaneous : Grapevine searches your class for the best handler, no need to "register" new ones.  You can add files to the webroot to be served on-the-fly - no need to restart the server.  You can even write a custom handler to shut down your server remotely!

##Usage##
Grapevine provides the [HttpResponder](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/HttpResponder.cs) abstract class and the [Responder](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/Responder.cs) custom attribute.  Simply create a class that extends HttpResponder, and annotate the appropriate handler methods with the Responder attribute.  **No methods to implement!**

Attribute values default to Method = `HttpMethod.GET` and PathInfo = `"/"`, so for a catch-all method you don't need to define anything.

###Example###
An example of a simple REST server that responds to GET requests on `http://localhost:1234/foo/5678`.

    using System.Net;
    using Grapevine;
    
    namespace SampleServer
    {
        class RestServer : HttpResponder
        {
            [Responder(Method = HttpMethod.GET, PathInfo = @"^/foo/\d+$")]
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

See the [**cookbook**](https://github.com/scottoffen/Grapevine/wiki) for more examples, including how to change the host, port, number of threads and webroot directory.

###Limitations###
- Grapevine is **not** intended to be a drop-in replacement for [Microsoft IIS](http://www.iis.net/) or [Apache HTTP Server](http://httpd.apache.org/).  Instead, Grapevine aims to be embedded in your application, where using one of those would be impossible, or just plain overkill.

- Grapevine does not support **ASP.NET** nor do any script parsing (**PHP**, **Perl**, **Python**, **Ruby**, etc.) by default - but feel free to fork this project and hack away at it to your hearts content.

- A single instance will only listen on one host/port combination (unless you define the host as "*").

##License##
Copyright 2011-2014 Scott Offen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at [apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

"Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
