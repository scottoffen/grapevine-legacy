using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;

namespace Grapevine.Server
{
    /// <summary>
    /// Delegate for the <see cref="IRouter.BeforeRouting"/> and <see cref="IRouter.AfterRouting"/> events
    /// </summary>
    /// <param name="context">The <see cref="IHttpContext"/> that is being routed.</param>
    public delegate void RoutingEventHandler(IHttpContext context);

    /// <summary>
    /// Provides a mechanism to register routes and invoke them according to the produced routing table
    /// </summary>
    public interface IRouter
    {
        #region Deprecated

        /// <summary>
        /// Gets or sets a function to be executed prior to any routes being executed
        /// </summary>
        [Obsolete(
             "The After delegate has been replace with the AfterRouting event and will be removed in the next version.")
        ]
        Func<IHttpContext, IHttpContext> After { get; set; }

        /// <summary>
        /// Gets or sets a function to be executed after route execution has completed
        /// </summary>
        [Obsolete(
             "The Before delegate has been replace with the BeforeRouting event and will be removed in the next version."
         )]
        Func<IHttpContext, IHttpContext> Before { get; set; }

        #endregion

        /// <summary>
        /// Raised after a request has completed invoking matching routes
        /// </summary>
        event RoutingEventHandler AfterRouting;

        /// <summary>
        /// Raised prior to sending any request though matching routes
        /// </summary>
        event RoutingEventHandler BeforeRouting;

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
        IRouter Import<T>() where T : IRouter, new();

        /// <summary>
        /// Inserts the route into the routing table after the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        IRouter InsertAfter(int index, IRoute route);

        /// <summary>
        /// Inserts the route into the routing table after the specified route
        /// </summary>
        /// <param name="afterThisRoute"></param>
        /// <param name="insertThisRoute"></param>
        /// <returns></returns>
        IRouter InsertAfter(IRoute afterThisRoute, IRoute insertThisRoute);

        /// <summary>
        /// Inserts the routes into the routing table after the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        IRouter InsertAfter(int index, IList<IRoute> routes);

        /// <summary>
        /// Inserts the routes into the routing table after the specified route
        /// </summary>
        /// <param name="afterThisRoute"></param>
        /// <param name="insertTheseRoutes"></param>
        /// <returns></returns>
        IRouter InsertAfter(IRoute afterThisRoute, IList<IRoute> insertTheseRoutes);

        /// <summary>
        /// Inserts the route into the routing table before the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        IRouter InsertBefore(int index, IRoute route);

        /// <summary>
        /// Inserts the route into the routing table before the specified route
        /// </summary>
        /// <param name="beforeThisRoute"></param>
        /// <param name="insertThisRoute"></param>
        /// <returns></returns>
        IRouter InsertBefore(IRoute beforeThisRoute, IRoute insertThisRoute);

        /// <summary>
        /// Inserts the routes into the routing table before the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        IRouter InsertBefore(int index, IList<IRoute> routes);

        /// <summary>
        /// Inserts the route into the routing table before the specified route
        /// </summary>
        /// <param name="beforeThisRoute"></param>
        /// <param name="insertTheseRoutes"></param>
        /// <returns></returns>
        IRouter InsertBefore(IRoute beforeThisRoute, IList<IRoute> insertTheseRoutes);

        /// <summary>
        /// Inserts the route on the top of the routing table
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        IRouter InsertFirst(IRoute route);

        /// <summary>
        /// Inserts the routes to the top of the routing
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        IRouter InsertFirst(IList<IRoute> routes);

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
        /// Routes the IHttpContext through all enabled registered routes that match the IHttpConext provided
        /// </summary>
        /// <param name="state"></param>
        void Route(object state);

        /// <summary>
        /// Routes the IHttpContext through the list of routes provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routing"></param>
        void Route(IHttpContext context, IList<IRoute> routing);

        /// <summary>
        /// Gets a list of enabled registered routes that match the IHttpContext provided
        /// </summary>
        /// <param name="context"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> RoutesFor(IHttpContext context);
    }

    public class Router : IRouter
    {
        #region Deprecated

        public Func<IHttpContext, IHttpContext> After { get; set; }
        public Func<IHttpContext, IHttpContext> Before { get; set; }
        private IGrapevineLogger _logger;

        #endregion

        protected internal readonly IList<IRoute> RegisteredRoutes;
        protected internal ConcurrentDictionary<string, IList<IRoute>> RouteCache;

        public event RoutingEventHandler AfterRouting;
        public event RoutingEventHandler BeforeRouting;

        public bool ContinueRoutingAfterResponseSent { get; set; }
        public string Scope { get; }
        public IRouteScanner Scanner { get; protected internal set; }
        public IList<IRoute> RoutingTable => RegisteredRoutes.ToList().AsReadOnly();

        /// <summary>
        /// Creates a new Router object
        /// </summary>
        public Router()
        {
            RegisteredRoutes = new List<IRoute>();
            RouteCache = new ConcurrentDictionary<string, IList<IRoute>>();
            Logger = NullLogger.GetInstance();
            Scanner = new RouteScanner();
            Scope = string.Empty;
        }

        /// <summary>
        /// Creates a new Router object with the Scope property set to the parameter supplied
        /// </summary>
        /// <param name="scope"></param>
        public Router(string scope) : this()
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

        public IRouter Import(IRouter router)
        {
            AppendRoutingTable(router.RoutingTable);
            return this;
        }

        public IRouter Import(Type type)
        {
            if (!type.IsClass) throw new ArgumentException($"Cannot Import: {type.FullName} type is not a class");
            if (type.IsAbstract) throw new ArgumentException($"Cannot Import: {type.FullName} is an abstract class");
            if (!type.Implements<IRouter>()) throw new ArgumentException($"Cannot Import: {type.FullName} does not implement {typeof(IRouter).FullName}");
            if (!type.HasParameterlessConstructor()) throw new ArgumentException($"Cannot Import: {type.FullName} does not have parameterless constructor");
            return Import((IRouter)Activator.CreateInstance(type));
        }

        public IRouter Import<T>() where T : IRouter, new()
        {
            return Import((IRouter)Activator.CreateInstance(typeof(T)));
        }

        public IRouter InsertAfter(int index, IRoute route)
        {
            if (index >= 0) InsertAt(index + 1, route);
            return this;
        }

        public IRouter InsertAfter(IRoute afterThisRoute, IRoute insertThisRoute)
        {
            if (RegisteredRoutes.Contains(afterThisRoute))
                InsertAt(RegisteredRoutes.IndexOf(afterThisRoute) + 1, insertThisRoute);
            return this;
        }

        public IRouter InsertAfter(int index, IList<IRoute> insertTheseRoutes)
        {
            var counter = index + 1;
            if (counter > 0) insertTheseRoutes.Aggregate(counter, InsertAt);
            return this;
        }

        public IRouter InsertAfter(IRoute afterThisRoute, IList<IRoute> insertTheseRoutes)
        {
            var counter = RegisteredRoutes.IndexOf(afterThisRoute) + 1;
            if (counter > 0) insertTheseRoutes.Aggregate(counter, InsertAt);
            return this;
        }

        public IRouter InsertBefore(int index, IRoute route)
        {
            if (index >= 0) InsertAt(index, route);
            return this;
        }

        public IRouter InsertBefore(IRoute beforeThisRoute, IRoute insertThisRoute)
        {
            if (RegisteredRoutes.Contains(beforeThisRoute))
                InsertAt(RegisteredRoutes.IndexOf(beforeThisRoute), insertThisRoute);
            return this;
        }

        public IRouter InsertBefore(int index, IList<IRoute> insertTheseRoutes)
        {
            if (index >= 0) insertTheseRoutes.Aggregate(index, InsertAt);
            return this;
        }

        public IRouter InsertBefore(IRoute beforeThisRoute, IList<IRoute> insertTheseRoutes)
        {
            var counter = RegisteredRoutes.IndexOf(beforeThisRoute);
            if (counter >= 0) insertTheseRoutes.Aggregate(counter, InsertAt);
            return this;
        }

        public IRouter InsertFirst(IRoute route)
        {
            InsertAt(0, route);
            return this;
        }

        public IRouter InsertFirst(IList<IRoute> routes)
        {
            const int counter = 0;
            routes.Aggregate(counter, InsertAt);
            return this;
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
            AppendRoutingTable(route);
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func)
        {
            AppendRoutingTable(new Route(func));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo)
        {
            AppendRoutingTable(new Route(func, pathInfo));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod)
        {
            AppendRoutingTable(new Route(func, httpMethod));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo)
        {
            AppendRoutingTable(new Route(func, httpMethod, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo)
        {
            AppendRoutingTable(new Route(methodInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, string pathInfo)
        {
            AppendRoutingTable(new Route(methodInfo, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod)
        {
            AppendRoutingTable(new Route(methodInfo, httpMethod));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod, string pathInfo)
        {
            AppendRoutingTable(new Route(methodInfo, httpMethod, pathInfo));
            return this;
        }

        public IRouter Register(Type type)
        {
            AppendRoutingTable(Scanner.ScanType(type));
            return this;
        }

        public IRouter Register<T>()
        {
            return Register(typeof(T));
        }

        public IRouter Register(Assembly assembly)
        {
            AppendRoutingTable(Scanner.ScanAssembly(assembly));
            return this;
        }

        public IRouter ScanAssemblies()
        {
            AppendRoutingTable(Scanner.Scan());
            return this;
        }

        public void Route(object state)
        {
            var context = state as IHttpContext;
            if (context == null) return;

            try
            {
                if (context.Request.HttpMethod == HttpMethod.GET)
                {
                    foreach (var folder in context.Server.PublicFolders)
                    {
                        folder.SendFile(context);
                        if (context.WasRespondedTo) return;
                    }
                }

                Route(context, RoutesFor(context));
            }
            catch (Exception e)
            {
                Logger.Log(new LogEvent { Exception = e, Level = LogLevel.Error, Message = e.Message });
                if (context.Server.EnableThrowingExceptions) throw;

                if (context.WasRespondedTo) return;

                if (e is NotFoundException)
                {
                    context.Response.SendResponse(HttpStatusCode.NotFound, e.Message);
                    return;
                }

                if (e is NotImplementedException)
                {
                    context.Response.SendResponse(HttpStatusCode.NotImplemented, e.Message);
                    return;
                }

                context.Response.SendResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        public void Route(IHttpContext context, IList<IRoute> routing)
        {
            if (routing == null || !routing.Any()) throw new RouteNotFoundException(context);
            var totalRoutes = routing.Count;
            var routeCounter = 0;

            Logger.BeginRouting($"{context.Request.Id} - {context.Request.Name} has {totalRoutes} routes");

            try
            {
                OnBeforeRouting(context);

                foreach (var route in routing.Where(route => route.Enabled))
                {
                    routeCounter++;
                    route.Invoke(context);

                    Logger.RouteInvoked($"{context.Request.Id} - {routeCounter}/{totalRoutes} {route.Name}");

                    if (ContinueRoutingAfterResponseSent) continue;
                    if (context.WasRespondedTo) break;
                }
            }
            finally
            {
                OnAfterRouting(context);
                Logger.EndRouting($"{context.Request.Id} - {routeCounter} of {totalRoutes} routes invoked");
            }

            if (!context.WasRespondedTo) throw new RouteNotFoundException(context);
        }

        public IList<IRoute> RoutesFor(IHttpContext context)
        {
            var key = $"{context.Request.HttpMethod}:{context.Request.PathInfo}";
            return RouteCache.GetOrAdd(key, RegisteredRoutes.Where(r => r.Matches(context) && r.Enabled).ToList());
        }

        /// <summary>
        /// Event handler for when the <see cref="BeforeRouting"/> event is raised
        /// </summary>
        /// <param name="context">The <see cref="IHttpContext"/> being routed</param>
        protected internal void OnBeforeRouting(IHttpContext context)
        {
            Before?.Invoke(context);
            BeforeRouting?.Invoke(context);
        }

        /// <summary>
        /// Event handler for when the <see cref="AfterRouting"/> event is raised
        /// </summary>
        /// <param name="context">The <see cref="IHttpContext"/> being routed</param>
        protected internal void OnAfterRouting(IHttpContext context)
        {
            After?.Invoke(context);
            if (AfterRouting == null) return;
            foreach (var action in AfterRouting.GetInvocationList().Reverse().Cast<RoutingEventHandler>())
            {
                action(context);
            }
        }

        /// <summary>
        /// Adds the route to the routing table excluding duplicates
        /// </summary>
        /// <param name="route"></param>
        protected internal void AppendRoutingTable(IRoute route)
        {
            if (route.Function == null) throw new ArgumentNullException(nameof(route));
            if (!RegisteredRoutes.Contains(route)) RegisteredRoutes.Add(route);
            if (!RouteCache.IsEmpty) RouteCache.Clear();
        }

        /// <summary>
        /// Adds the routes to the routing table excluding duplicates
        /// </summary>
        /// <param name="routes"></param>
        protected internal void AppendRoutingTable(IEnumerable<IRoute> routes)
        {
            routes.ToList().ForEach(AppendRoutingTable);
        }

        /// <summary>
        /// Inserts the specified route into the routing table at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns>Returns the index the next route can be inserted at.</returns>
        protected internal int InsertAt(int index, IRoute route)
        {
            if (index < 0 || RegisteredRoutes.Contains(route)) return index;
            RegisteredRoutes.Insert(index, route);
            return index + 1;
        }
    }
}