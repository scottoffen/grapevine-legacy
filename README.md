![](https://raw.githubusercontent.com/sukona/Grapevine/master/grapevine.png)

*The best solutions are the simplest to implement*. Embedding a REST/HTTP server in your application should be **simple**. Consuming REST resources from inside your application should be **simple**. If what you've been using *doesn't feel simple*, try **Grapevine**. It doesn't get any simpler than this.

### Introduction ###

Grapevine is a .NET class library focused on solving two problems:

1. Easily embedding a REST/HTTP servers in your application
2. Easily consume REST resources in your application

The focus is on simplicity, and Grapevine is intended for use in applications for which being a REST or HTTP client or server is not the primary function or purpose of the application.

### Features ###

- Grapevine can serve both **static files and dynamic resources**

- Grapevine can both **produce and consume** REST services

- Grapevine has **minimal configuration** requirements

- Grapevine allows you to **map specific methods to HTTP Method and URL patterns**

- Grapevine supports using **regular expressions**

- Grapevine streamlines connecting and communicating with REST servers using **simple patterns and placeholders**

- Grapevine can **listen on multiple ports**, and scope REST resources to those ports

### Limitations ###

- If an application needs to scale and/or balance load across multiple servers, it would likely benefit from the features offered by an [enterprise service bus](http://en.wikipedia.org/wiki/Enterprise_service_bus) (ESB). While Grapevine would not likely be a good fit for an ESB, it can certainly facilitate communication with them.

- Grapevine is **not** intended to be a drop-in replacement for [Microsoft IIS](http://www.iis.net/) or [Apache HTTP Server](http://httpd.apache.org/). Nor is it a full-featured application server (like [Tomcat](http://en.wikipedia.org/wiki/Apache_Tomcat)) or framework (like [Spring](http://en.wikipedia.org/wiki/Spring_Framework)). Instead, Grapevine aims to be **embedded in your application**, where using one of those would be impossible, or just plain overkill.

- Grapevine does not do any script parsing (**PHP**, **Perl**, **Python**, **Ruby**, etc.) by default - but feel free to fork this project and hack away! I'm pretty sure it could be done, I just haven't encountered a need for it (yet).

- You will likely be required to [open a port in your firewall](http://www.lmgtfy.com/?q=how+to+open+a+port+on+windows) for remote computers to be able to send requests to your application. Grapevine will not [automatically](http://msdn.microsoft.com/en-us/library/aa366418%28VS.85%29.aspx) do that for you.  You might want to do that during the [installation of your application](http://www.codeproject.com/Articles/14906/Open-Windows-Firewall-During-Installation).

### Support ###

If you find you are having problems and need help check out our [support options](https://github.com/sukona/Grapevine/blob/master/SUPPORT.md).

> "Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
