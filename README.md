Grapevine
=========

![](https://raw.github.com/scottoffen/Grapevine/master/grapevine.png)

Grapevine provides an abstract class for quickly and easily creating multithreaded .NET HTTP endpoints using the ubiquitous [HttpListener](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistener(v=vs.100)) class and custom [attributes](http://msdn.microsoft.com/en-us/library/sw480ze8.aspx).  Grapevine makes it simple to serve up file and REST services equally well, providing all the functionality needed to create a robust backend.

##Features##
Grapevine is:

- Flexible : Grapevine just listens, you provide the responses.  And don't worry! If Grapevine can't find a response, it looks for a file, and if there is no file, it handles returning the errors.

- Fast : Grapevine accepts requests and spins it off to be handled by another thread.  As a result, there is no blocking I/O, because the server is always ready to respond to incoming requests.

- No Distortion: The [message context](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistenercontext(v=vs.110).aspx) is passed to your responder methods - you get all of the data all of the time so you can decide how to respond.

- Spontaneous: Grapevine runs only as many threads as you specify, and those threads focus on sending requests to the appropriate responders, so there is never any difficulty in passing messages around.


##Usage##
Grapevine provides the [HttpResponder](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/HttpResponder.cs) abstract class and the [Responder](https://github.com/scottoffen/Grapevine/blob/master/Grapevine/Responder.cs) custom attribute.  Simply create a class that extends HttpResponder, and annotate the appropriate responder methods with the Responder attribute.

Attribute values default to Method = "GET" and PathInfo = "/", so for a catch-all method you don't need to define anything.

###Example###
An example of a simple REST server that responds to GET requests on http://localhost:1234/foo/5678

    using Grapevine;
    using System.Net;

    ...
        class RestServer : HttpResponder
        {
            [Responder(Method = "GET", PathInfo = @"/foo/\d+")]
            public void HandleSomething(HttpListenerContext context)
            {
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "Success";
                this.SendResponse(context, "SUCCESS");
            }
        }

In your main thread, spin up your server like so:

    var server = new RestServer();
    server.Start();

    while (server.IsListening)
    {
        Thread.Sleep(500);
    }

See the cookbook for more examples.


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

"Dark Blue Grapes In Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
