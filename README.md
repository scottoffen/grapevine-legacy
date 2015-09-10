Grapevine 3.0.4
===============

[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/scottoffen/Grapevine.svg)](http://isitmaintained.com/project/scottoffen/Grapevine "Average time to resolve an issue") [![Percentage of issues still open](http://isitmaintained.com/badge/open/scottoffen/Grapevine.svg)](http://isitmaintained.com/project/scottoffen/Grapevine "Percentage of issues still open")

*The best solutions are the simplest to implement*. Embedding a REST/HTTP server in your application should be **simple**. Consuming REST resources from inside your application should be **simple**. If what you've been using *doesn't feel simple*, try **Grapevine**. It doesn't get any simpler than this.

>For a version of Grapevine with JSON support baked in, try new [Grapevine Plus](https://github.com/scottoffen/GrapevinePlus)!

![](https://raw.github.com/scottoffen/Grapevine/master/grapevine.png)

>Grapevine is a .NET 4.0 class library for embedding REST/HTTP servers **and/or** clients inside any application.
>
>**Server** - Utilizing the ubiquitous [`HttpListener`](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistener(v=vs.100)) class, Grapevine allows you to map HTTP Methods and URL patterns (using regular expressions) to specific methods.
>
>**Client** - Streamline connecting and communicating with REST servers using simple patterns and placeholders.
>
>*Version 3.0 is not backwards compatible with earlier versions.*

## Use Case for Grapevine ##

Grapevine is designed for use in an application for which being a REST or HTTP client or server is not the primary function or purpose of the application, but rather a standardized means of communication with the application.

For example, a Widows Forms application or Windows Service would be the "primary" means of communication with an application, and having a Grapevine `RESTServer` (or several) listening on a particular protocol/host/port combination would be a secondary means of communication - even if Grapevine is used to expose the majority of the functionality.

>If an application needs to scale or do load balancing, it would likely benefit from the features offered by an [enterprise service bus](http://en.wikipedia.org/wiki/Enterprise_service_bus) (ESB). While Grapevine would not likely be a good fit for an ESB, it can certainly facilitate communication with them!

Having a REST client in the same package means you can both produce and consume REST services - such as building an application that actively communicates with other applications like it on the network by initiating the conversation, not just waiting around to for something else to initiate a conversation with it.

## Features ##

- Embed a REST server in your application. Add attributes to your classes and methods to define resources and routes for managing traffic based on HTTP method and path info (using regular expressions). The [message context](http://msdn.microsoft.com/en-us/library/vstudio/system.net.httplistenercontext(v=vs.110).aspx) is passed to your route every time, and each resource has a reference to the server that spawned it.

- Manage multiple REST servers simultaneously and easily with a `RESTCluster`. Scope your resources to one, many or all REST servers.

- Serve up static files (HTML, CSS, JavaScript, images, etc.) with virtually no configuration. Each server can have a unique location to serve files from, or they can all share a location.

- Embed REST clients to interact with remote RESTful APIs. Initiate exchanges as well as respond them.

- Write messages out to a common event log for your entire application using `EventLogger`. [`404 Not Found`](http://en.wikipedia.org/wiki/HTTP_404) and [`500 Internal Server Error`](http://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_Server_Error) responses are handled automatically if a route or file cannot be found or throws an unhandled exception, respectively. Have complete control over the response returned to the client.

## Limitations ##

- Grapevine is **not** intended to be a drop-in replacement for [Microsoft IIS](http://www.iis.net/) or [Apache HTTP Server](http://httpd.apache.org/). Nor is it a full-featured application server (like [Tomcat](http://en.wikipedia.org/wiki/Apache_Tomcat)) or framework (like [Spring](http://en.wikipedia.org/wiki/Spring_Framework)). Instead, Grapevine aims to be **embedded in your application**, where using one of those would be impossible, or just plain overkill.

- Grapevine does not do any script parsing (**PHP**, **Perl**, **Python**, **Ruby**, etc.) by default - but feel free to fork this project and hack away! I'm pretty sure it could be done, I just haven't encountered a need for it (yet).

- You will likely be required to [open a port in your firewall](http://www.lmgtfy.com/?q=how+to+open+a+port+on+windows) for remote computers to be able to send requests to your application. Grapevine will not [automatically](http://msdn.microsoft.com/en-us/library/aa366418%28VS.85%29.aspx) do that for you.  You might want to do that during the [installation of your application](http://www.codeproject.com/Articles/14906/Open-Windows-Firewall-During-Installation).

## Contact Me ##
Feel free to [contact me with feedback](mailto:github@scottoffen.com) if Grapevine has been useful for you or your company. If you find you are having problems and need help check out my [support options](https://github.com/scottoffen/Grapevine/blob/master/SUPPORT.md).

## License ##
Copyright 2014 Scott Offen

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at [apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, **WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND**, either express or implied. See the License for the specific language governing permissions and limitations under the License.

"Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
