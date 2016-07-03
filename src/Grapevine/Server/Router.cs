using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    /// <summary>
    /// Provides a mechanism to register routes and invoke them according to the produced routing table
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        /// Gets the scope used when scanning assemblies for routes
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// Adds the <c>Type</c> to the list of excluded types when scanning assemblies for routes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRouter Exclude<T>();

        /// <summary>
        /// Adds the <c>Type</c> to the list of excluded types when scanning assemblies for routes
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        IRouter Exclude(Type T);

        /// <summary>
        /// Adds the namespace to the list of excluded namespaces when scanning assemblies for routes
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        IRouter ExcludeNameSpace(string nameSpace);

        /// <summary>
        /// Gets the Exclusions that represents the types and namespaces to be exluded when scanning assemblies for routes
        /// </summary>
        IExclusions Exclusions { get; }

        /// <summary>
        /// Gets a list of registered routes in the order they were registered
        /// </summary>
        IList<IRoute> RoutingTable { get; }

        /// <summary>
        /// Adds the route to the routing table
        /// </summary>
        /// <param name="route"></param>
        /// <returns>IRouter</returns>
        IRouter Register(IRoute route);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IRouter</returns>
        IRouter Register(Func<IHttpContext, IHttpContext> func);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="func"></param>
        /// <param name="pathInfo"></param>
        /// <returns>IRouter</returns>
        IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="func"></param>
        /// <param name="httpMethod"></param>
        /// <returns>IRouter</returns>
        IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="func"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        /// <returns>IRouter</returns>
        IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="method"></param>
        /// <returns>IRouter</returns>
        IRouter Register(MethodInfo method);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="method"></param>
        /// <param name="pathInfo"></param>
        /// <returns>IRouter</returns>
        IRouter Register(MethodInfo method, string pathInfo);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="method"></param>
        /// <param name="httpMethod"></param>
        /// <returns>IRouter</returns>
        IRouter Register(MethodInfo method, HttpMethod httpMethod);

        /// <summary>
        /// Creates a new route and adds it to the routing table
        /// </summary>
        /// <param name="method"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        /// <returns>IRouter</returns>
        IRouter Register(MethodInfo method, HttpMethod httpMethod, string pathInfo);

        /// <summary>
        /// Adds all RestRoutes in the specified type to the routing table
        /// </summary>
        /// <param name="type"></param>
        /// <returns>IRouter</returns>
        IRouter Register(Type type);

        /// <summary>
        /// Adds all RestRoutes in the specified type to the routing table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IRouter</returns>
        IRouter Register<T>();

        /// <summary>
        /// Adds all RestRoutes found in all RestResources in the current assembly to the routing table
        /// </summary>
        /// <returns>IRouter</returns>
        IRouter RegisterAssembly();

        /// <summary>
        /// Adds the routes in router parameter to the end of the current routing table
        /// </summary>
        /// <param name="router"></param>
        /// <returns>IRouter</returns>
        IRouter Import(IRouter router);

        /// <summary>
        /// Adds the routes from the router type parameter to the end of the current routing table
        /// </summary>
        /// <param name="type"></param>
        /// <returns>IRouter</returns>
        IRouter Import(Type type);

        /// <summary>
        /// Adds the routes from the router type parameter to the end of the current routing table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IRouter</returns>
        IRouter Import<T>() where T : IRouter;

        /// <summary>
        /// Gets a list of enabled registered routes that match the IHttpContext provided
        /// </summary>
        /// <param name="context"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> RouteFor(IHttpContext context);

        /// <summary>
        /// Routes the IHttpContext through all enabled registered routes that match the IHttpConext provided; returns true if at least one route is invoked
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Route(IHttpContext context);

        /// <summary>
        /// Routes the IHttpContext through the list of routes provided; returns true if at least one route is invoked
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        bool Route(IHttpContext context, IList<IRoute> routing);
    }

    public class Router : IRouter
    {
        private readonly Exclusions _exclusions;
        private readonly IList<IRoute> _routingTable;

        public string Scope { get; protected set; }

        /// <summary>
        /// Returns a new Router object
        /// </summary>
        public Router()
        {
            _exclusions = new Exclusions();
            _routingTable = new List<IRoute>();
        }

        /// <summary>
        /// Returns a new Router object with the Scope property set to the parameter supplied
        /// </summary>
        /// <param name="scope"></param>
        public Router(string scope):this()
        {
            Scope = scope;
        }

        /// <summary>
        /// Returns a new Router object configured by the provided Action and with the Scope property set to the parameter supplied
        /// </summary>
        /// <param name="config"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Router For(Action<IRouter> config, string scope = null)
        {
            var router = new Router(scope);
            config(router);
            return router;
        }

        public IRouter Exclude(Type type)
        {
            if (!_exclusions.Types.Contains(type)) _exclusions.Types.Add(type);
            return this;
        }

        public IRouter Exclude<T>()
        {
            return Exclude(typeof(T));
        }

        public IRouter ExcludeNameSpace(string nameSpace)
        {
            if (!_exclusions.NameSpaces.Contains(nameSpace)) _exclusions.NameSpaces.Add(nameSpace);
            return this;
        }

        public IRouter Register(IRoute route)
        {
            AddToRoutingTable(route);
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func)
        {
            AddToRoutingTable(new Route(func));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo)
        {
            AddToRoutingTable(new Route(func, pathInfo));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod)
        {
            AddToRoutingTable(new Route(func, httpMethod));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo)
        {
            AddToRoutingTable(new Route(func, httpMethod, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo)
        {
            AddToRoutingTable(new Route(methodInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, string pathInfo)
        {
            AddToRoutingTable(new Route(methodInfo, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod)
        {
            AddToRoutingTable(new Route(methodInfo, httpMethod));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod, string pathInfo)
        {
            AddToRoutingTable(new Route(methodInfo, httpMethod, pathInfo));
            return this;
        }

        public IRouter Register(Type type)
        {
            AddRangeToGlobalStack(GenerateRoutes(type));
            return this;
        }

        public IRouter Register<T>()
        {
            return Register(typeof(T));
        }

        public IRouter RegisterAssembly()
        {
            AddRangeToGlobalStack(GenerateRoutes(Assembly.GetCallingAssembly()));
            return this;
        }

        public IRouter Import(IRouter router)
        {
            AddRangeToGlobalStack(router.RoutingTable);
            return this;
        }

        public IRouter Import(Type type)
        {
            if (type.IsNot<IRouter>()) throw new ArgumentException($"Cannot Import: {type.FullName} does not implement {typeof(IRouter).FullName}");
            if (type.IsAbstract) throw new ArgumentException($"Cannot Import: {type.FullName} is an abstract class");
            if (!type.IsClass) throw new ArgumentException($"Cannot Import: {type.FullName} type is not a class");
            return Import((IRouter) Activator.CreateInstance(type));
        }

        public IRouter Import<T>() where T : IRouter
        {
            return Import(typeof (T));
        }

        public IList<IRoute> RouteFor(IHttpContext context)
        {
            return _routingTable.Where(r => r.Matches(context) && r.Enabled).ToList();
        }

        public IList<IRoute> RoutingTable => _routingTable.ToList().AsReadOnly();

        public IExclusions Exclusions => _exclusions.AsReadOnly();

        public bool Route(IHttpContext context)
        {
            return Route(context, RouteFor(context));
        }

        public bool Route(IHttpContext context, IList<IRoute> routing)
        {
            LogBeginRequestRouting(context, routing.Count);

            if (routing == null || !routing.Any()) throw new RouteNotFound(context);
            if (context.WasRespondedTo()) return true;

            var routeContext = context;
            var routeCounter = 0;

            foreach (var route in routing)
            {
                if (!route.Enabled) continue;
                routeCounter++;

                // Stopwatch starts here
                routeContext = route.Invoke(routeContext);
                // Stopwatch stops here

                LogRouteInvoked(context, route, routeCounter);
                if (routeContext.WasRespondedTo()) break;
            }

            LogEndRequestRouting(context, routing.Count, routeCounter);
            return routeContext.WasRespondedTo();
        }

        private static void LogBeginRequestRouting(IHttpContext context, int routes)
        {
            context.Server.Logger.Info($"Request {context.Request.Id}:{context.Request.Name} has {routes} routes");
        }

        private static void LogEndRequestRouting(IHttpContext context, int routes, int routesHit)
        {
            context.Server.Logger.Trace($"Request {context.Request.Id}:{context.Request.Name} invoked {routes}/{routesHit} routes");
        }

        private static void LogRouteInvoked(IHttpContext context, IRoute route, int routeIndex)
        {
            context.Server.Logger.Trace($"{routeIndex} Request {context.Request.Id}:{context.Request.Name} hit {route.Name}");
        }

        /// <summary>
        /// Generates a list of routes for the RestRoute attributed MethodInfo provided and the basePath applied to the PathInfo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="basePath"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        internal IList<IRoute> GenerateRoutes(MethodInfo method, string basePath)
        {
            var routes = new List<IRoute>();

            var basepath = string.IsNullOrWhiteSpace(basePath)
                ? string.Empty
                : basePath;

            if (basepath != string.Empty && basepath.EndsWith("/"))
                basepath = basepath.TrimEnd('/');

            if (basepath != string.Empty && !basepath.StartsWith("/"))
                basepath = $"/{basepath}";

            foreach (var attribute in method.GetCustomAttributes(true).Where(a => a is RestRoute).Cast<RestRoute>())
            {
                var pathinfo = attribute.PathInfo;
                var prefix = string.Empty;

                if (pathinfo.StartsWith("^"))
                {
                    prefix = "^";
                    pathinfo = pathinfo.TrimStart('^');
                }

                if (!pathinfo.StartsWith("/")) pathinfo = $"/{pathinfo}";

                routes.Add(new Route(method, attribute.HttpMethod, $"{prefix}{basepath}{pathinfo}"));
            }

            return routes;
        }

        /// <summary>
        /// Generates a list of routes for all RestRoute attributed methods found in RestResource
        /// </summary>
        /// <param name="type"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        internal IList<IRoute> GenerateRoutes(Type type)
        {
            var routes = new List<IRoute>();
            var basepath = string.Empty;

            if (type.IsRestResource())
            {
                if (!string.IsNullOrWhiteSpace(Scope) && !Scope.Equals(type.RestResource().Scope)) return routes;
                basepath = type.RestResource().BasePath;
            }

            foreach (var method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                routes.AddRange(GenerateRoutes(method, basepath));
            }

            return routes;
        }

        /// <summary>
        /// Generates a list of routes for all RestResource types found in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        internal IList<IRoute> GenerateRoutes(Assembly assembly)
        {
            var routes = new List<IRoute>();

            foreach (var type in assembly.GetTypes().Where(t => t.IsRestResource()))
            {
                if (Exclusions.IsExcluded(type)) continue;
                routes.AddRange(GenerateRoutes(type));
            }

            return routes;
        }

        /// <summary>
        /// Adds the route to the routing table excluding duplicates
        /// </summary>
        /// <param name="route"></param>
        private void AddToRoutingTable(IRoute route)
        {
            if (route.Function == null) throw new ArgumentNullException(nameof(route));
            if (!_routingTable.Contains(route)) _routingTable.Add(route);
        }

        /// <summary>
        /// Adds the routes to the routing table excluding duplicates
        /// </summary>
        /// <param name="routes"></param>
        private void AddRangeToGlobalStack(IEnumerable<IRoute> routes)
        {
            routes.ToList().ForEach(AddToRoutingTable);
        }
    }

    /// <summary>
    /// Representation of the <c>Types</c> and namespaces to exclude when scanning assemblies for Routes
    /// </summary>
    public interface IExclusions
    {
        /// <summary>
        /// Gets the list of namespaces to exclude when scanning assemblies for Routes
        /// </summary>
        IList<string> NameSpaces { get; }

        /// <summary>
        /// Gets the list of <c>Types</c> to excluded when scanning assemblies for Routes
        /// </summary>
        IList<Type> Types { get; }

        /// <summary>
        /// Gets a read only representation of the IExclusion instance
        /// </summary>
        IExclusions AsReadOnly();

        /// <summary>
        /// Gets a value that indicates whether the supplied Type is excluded by Type or namespace
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsExcluded(Type type);
    }

    public class Exclusions : IExclusions
    {
        public IList<string> NameSpaces { get; internal set; }
        public IList<Type> Types { get; internal set; }

        internal Exclusions()
        {
            NameSpaces = new List<string>();
            Types = new List<Type>();
        }

        public IExclusions AsReadOnly()
        {
            return new Exclusions
            {
                NameSpaces = NameSpaces.ToList().AsReadOnly(),
                Types = Types.ToList().AsReadOnly()
            };
        }

        public bool IsExcluded(Type type)
        {
            return Types.Contains(type) || NameSpaces.Contains(type.Namespace);
        }
    }
}
