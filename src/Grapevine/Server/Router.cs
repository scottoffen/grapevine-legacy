using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;

namespace Grapevine.Server
{
    /// <summary>
    /// Provides a mechanism to register routes and invoke them according to the produced routing table
    /// </summary>
    public interface IRouter
    {
        Func<IHttpContext, IHttpContext> After { get; set; }

        Func<IHttpContext, IHttpContext> Before { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate whether request routing should continue even after a response has been sent.
        /// </summary>
        bool ContinueRoutingAfterResponseSent { get; set; }

        /// <summary>
        /// Scan the assembly for routes based on inclusion and exclusion rules
        /// </summary>
        IRouteScanner Scanner { get; }

        /// <summary>
        /// Gets a list of registered routes in the order they were registered
        /// </summary>
        IList<IRoute> RoutingTable { get; }

        /// <summary>
        /// Gets the scope used when scanning assemblies for routes
        /// </summary>
        string Scope { get; }

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
        /// Gets or sets the internal logger
        /// </summary>
        IGrapevineLogger Logger { get; set; }

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
        /// Adds all RestRoutes found in all RestResources in the specified assembly to the routing table
        /// </summary>
        /// <returns>IRouter</returns>
        IRouter Register(Assembly assembly);

        /// <summary>
        /// Adds all RestRoutes returned from RouteScanner.Scan() to the routing table
        /// </summary>
        /// <returns>IRouter</returns>
        IRouter ScanAssemblies();

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

        /// <summary>
        /// Gets a list of enabled registered routes that match the IHttpContext provided
        /// </summary>
        /// <param name="context"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> RouteFor(IHttpContext context);
    }

    public class Router : IRouter
    {
        private readonly IList<IRoute> _routingTable;
        private IGrapevineLogger _logger;

        public Func<IHttpContext, IHttpContext> After { get; set; }
        public Func<IHttpContext, IHttpContext> Before { get; set; }

        public bool ContinueRoutingAfterResponseSent { get; set; }
        public string Scope { get; protected set; }

        public IRouteScanner Scanner { get; protected internal set; }

        /// <summary>
        /// Returns a new Router object
        /// </summary>
        public Router()
        {
            _routingTable = new List<IRoute>();
            Logger = NullLogger.GetInstance();
            Scanner = new RouteScanner();
            Scope = string.Empty;
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

        public IGrapevineLogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value ?? NullLogger.GetInstance();
                if (Scanner != null) Scanner.Logger = _logger;
            }
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
            AddToRoutingTable(Scanner.ScanType(type));
            return this;
        }

        public IRouter Register<T>()
        {
            return Register(typeof(T));
        }

        public IRouter Register(Assembly assembly)
        {
            AddToRoutingTable(Scanner.ScanAssembly(assembly));
            return this;
        }

        public IRouter ScanAssemblies()
        {
            AddToRoutingTable(Scanner.Scan());
            return this;
        }

        public IRouter Import(IRouter router)
        {
            AddToRoutingTable(router.RoutingTable);
            return this;
        }

        public IRouter Import(Type type)
        {
            if (!type.IsClass) throw new ArgumentException($"Cannot Import: {type.FullName} type is not a class");
            if (type.IsAbstract) throw new ArgumentException($"Cannot Import: {type.FullName} is an abstract class");
            if (!type.Implements<IRouter>()) throw new ArgumentException($"Cannot Import: {type.FullName} does not implement {typeof(IRouter).FullName}");
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

        public bool Route(IHttpContext context)
        {
            return Route(context, RouteFor(context));
        }

        public bool Route(IHttpContext context, IList<IRoute> routing)
        {
            if (routing == null || !routing.Any()) throw new RouteNotFoundException(context);
            if (context.WasRespondedTo) return true;

            var routeContext = context;
            var totalRoutes = routing.Count;
            var routeCounter = 0;

            Logger.BeginRouting($"{context.Request.Id} - {context.Request.Name} has {totalRoutes} routes");

            if (Before != null) routeContext = Before.Invoke(routeContext);

            try
            {
                foreach (var route in routing.Where(route => route.Enabled))
                {
                    routeCounter++;
                    routeContext = route.Invoke(routeContext);

                    Logger.RouteInvoked($"{context.Request.Id} - {routeCounter}/{totalRoutes} {route.Name}");
                    if (ContinueRoutingAfterResponseSent) continue;
                    if (routeContext.WasRespondedTo) break;
                }
            }
            finally
            {
                if (After != null) routeContext = After.Invoke(routeContext);
                Logger.EndRouting($"{context.Request.Id} - {routeCounter} of {totalRoutes} routes invoked");
            }

            return routeContext.WasRespondedTo;
        }

        /// <summary>
        /// Adds the route to the routing table excluding duplicates
        /// </summary>
        /// <param name="route"></param>
        protected void AddToRoutingTable(IRoute route)
        {
            if (route.Function == null) throw new ArgumentNullException(nameof(route));
            if (!_routingTable.Contains(route)) _routingTable.Add(route);
        }

        /// <summary>
        /// Adds the routes to the routing table excluding duplicates
        /// </summary>
        /// <param name="routes"></param>
        protected void AddToRoutingTable(IEnumerable<IRoute> routes)
        {
            routes.ToList().ForEach(AddToRoutingTable);
        }
    }
}