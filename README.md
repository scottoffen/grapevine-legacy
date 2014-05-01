Grapevine REST/HTTP Server +Client
==================================

![](https://raw.github.com/scottoffen/Grapevine/master/grapevine.png)

**Current Version: 2.6**

Grapevine provides a framework to quickly and easily embed both REST Clients and multithreaded REST/HTTP servers into your applications using the ubiquitous [HttpListener](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistener(v=vs.100)) class.  Grapevine makes it equally simple to produce or consume REST services and serve up static files.

##Use Grapevine##
See the wiki for a [**Getting Started Guide**](https://github.com/scottoffen/Grapevine/wiki/Getting-Started-Guide) and [other usage examples](https://github.com/scottoffen/Grapevine/wiki).

###Install Grapevine via NuGet###
Grapevine is available to install via [NuGet](https://www.nuget.org/packages/Grapevine/):

    > Install-Package Grapevine

##Use Case for Grapevine##
Like the [informal means of communication](http://en.wikipedia.org/wiki/Grapevine_(gossip)) its name alludes to, Grapevine is designed for use in applications for which being a REST or HTTP client or server is not the primary function or purpose of the application, but rather a secondary means of communicating with the application.

For example, a Widows Forms application or Windows Service would be the "primary" means of communication with an application, and having an object (or several) that extends Grapevine listening on a particular port would be your secondary means of communication - even if you plan on using Grapevine to expose the majority of your functionality.

Having the REST client in the same package means you can use a single package to both produce and consume REST services - such as building an application that actively communicates with other applications like it on the network by initiating the conversation instead of waiting around to for something else to initiate an conversation with it.

##Features##
- Flexible : Grapevine includes all you need for a REST server, a simple HTTP server, and a REST client.

- Fast : Grapevine uses a non-blocking I/O model, so the server is always ready to respond to incoming requests.

- Constant : The [message context](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistenercontext(v=vs.110).aspx) is passed to your handler methods - you get all of the data all of the time so you can decide how best to respond.

- Spontaneous : Grapevine finds the best route defined in your class, no need to "register" new ones.  You can add files to the webroot to be served on-the-fly - no need to restart the server.  You can even write a custom handler to shut down your server remotely!

##Limitations##
- Grapevine is **not** intended to be a drop-in replacement for [Microsoft IIS](http://www.iis.net/) or [Apache HTTP Server](http://httpd.apache.org/).  Instead, Grapevine aims to be embedded in your application, where using one of those would be impossible, or just plain overkill.

- Grapevine does not support **ASP.NET** nor does it do any script parsing (**PHP**, **Perl**, **Python**, **Ruby**, etc.) by default - but feel free to fork this project and hack away! I'm pretty sure it could be done, I just haven't encountered a need for it (yet).

- A single instance of a class that extends RestServer will only listen on one host/port combination (unless you define the host as "*").

- You will likely be required to [open a port in your firewall](http://www.lmgtfy.com/?q=how+to+open+a+port+on+windows) for remote computers to be able to send requests to your application. Grapevine will not [automatically](http://msdn.microsoft.com/en-us/library/aa366418%28VS.85%29.aspx) do that for you.  You might want to do that during the [installation of your application](http://www.codeproject.com/Articles/14906/Open-Windows-Firewall-During-Installation).

##Contact Me##
I'd love to hear from anyone using Grapevine, if for no other reason than to know someone else is finding this package useful.  [Email me](mailto:github@scottoffen.com), or, if you are having problems, [open an issue](https://github.com/scottoffen/Grapevine/issues).

Thanks for checking out Grapevine! 

##License##
Copyright 2014 Scott Offen

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at [apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, **WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND**, either express or implied. See the License for the specific language governing permissions and
limitations under the License.

"Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
