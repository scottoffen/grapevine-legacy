![](https://raw.githubusercontent.com/sukona/Grapevine/master/img/grapevine.png)

[![Build status](https://ci.appveyor.com/api/projects/status/wnn2r520922eex78/branch/master?svg=true&retina=true&passingText=master%20-%20passing&pendingText=master%20-%20pending&failingText=master%20-%20failing)](https://ci.appveyor.com/project/scottoffen/grapevine/branch/master)
[![Join the chat at https://gitter.im/sukona/Grapevine](https://badges.gitter.im/sukona/Grapevine.svg)](https://gitter.im/sukona/Grapevine?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)


*The best solutions are the simplest to implement*. Embedding a REST/HTTP server in your application should be **simple**. Consuming REST resources from inside your application should be **simple**. If what you've been using *doesn't feel simple*, try **Grapevine**. It doesn't get any simpler than this.

> I'm **always** open to [flattry](https://flattr.com/submit/auto?fid=x79jw7&url=https%3A%2F%2Fgithub.com%2Fsukona%2FGrapevine)

### Introduction ###

Grapevine is a .NET class library focused on solving two problems:

1. Easily embedding a REST/HTTP servers in your application
2. Easily consume REST resources in your application

The focus is on simplicity, and while Grapevine is intended for use in applications for which being a REST or HTTP client or server is not the primary function or purpose of the application, it can just as easily be used to serve up production-ready website and web applications.

### Features ###

- Grapevine has **no dependency** on IIS or `System.Web`.

- Grapevine can serve both **static files and dynamic resources**

- Grapevine can both **produce and consume** REST services

- Grapevine has **minimal configuration** requirements

- Grapevine allows you to **map specific methods to HTTP verbs and URL patterns**

- Grapevine supports using **regular expressions**

- Grapevine streamlines connecting and communicating with REST servers using **simple patterns and placeholders**

- Grapevine can **listen on multiple ports**, and scope REST resources to those ports

### Limitations ###

- Grapevine does not do any script parsing (CGI scripts or HTML templating engines) by default - but feel free to fork this project and hack away! I'm pretty sure it could be done, I just haven't encountered a need for it (yet).

- You will likely be required to [open a port in your firewall](http://www.dummies.com/how-to/content/how-to-open-a-port-in-the-windows-7-firewall.html) for remote computers to be able to send requests to your application. Grapevine will not (yet) automatically do that for you, but it's on our roadmap.

### Support ###

If you find you are having problems and need help check out our [support options](https://github.com/sukona/Grapevine/blob/master/SUPPORT.md).

> "Grapes In Dark Blue Cloud" Icon courtesy of [aha-soft](http://www.aha-soft.com/free-icons/free-dark-blue-cloud-icons/).
