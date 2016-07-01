using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    public interface IRouter
    {
        string Scope { get; }

        IRouter Exclude<T>();
        IRouter Exclude(Type T);
        IRouter ExcludeNameSpace(string nameSpace);

        IExclusions Exclusions { get; }
        IList<IRoute> RoutingTable { get; }

        IRouter Register(IRoute route);
        IRouter Register(Func<IHttpContext, IHttpContext> func);
        IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo);
        IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod);
        IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo);
        IRouter Register(MethodInfo method);
        IRouter Register(MethodInfo method, string pathInfo);
        IRouter Register(MethodInfo method, HttpMethod httpMethod);
        IRouter Register(MethodInfo method, HttpMethod httpMethod, string pathInfo);
        IRouter Register(Type type);
        IRouter Register<T>();
        IRouter RegisterAssembly();

        IRouter Import(IRouter router);
        IRouter Import(Type type);
        IRouter Import<T>() where T : IRouter;

        IList<IRoute> RouteFor(IHttpContext context);

        bool Route(IHttpContext context);
        bool Route(IHttpContext context, IList<IRoute> routing);
    }

    public class Router : IRouter
    {
        private readonly Exclusions _exclusions;
        private readonly IList<IRoute> _routingTable;

        public string Scope { get; protected set; }

        public Router()
        {
            _exclusions = new Exclusions();
            _routingTable = new List<IRoute>();
        }

        public Router(string scope):this()
        {
            Scope = scope;
        }

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
            AddToGlobalStack(route);
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func)
        {
            AddToGlobalStack(new Route(func));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, string pathInfo)
        {
            AddToGlobalStack(new Route(func, pathInfo));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod)
        {
            AddToGlobalStack(new Route(func, httpMethod));
            return this;
        }

        public IRouter Register(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo)
        {
            AddToGlobalStack(new Route(func, httpMethod, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo)
        {
            AddToGlobalStack(new Route(methodInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, string pathInfo)
        {
            AddToGlobalStack(new Route(methodInfo, pathInfo));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod)
        {
            AddToGlobalStack(new Route(methodInfo, httpMethod));
            return this;
        }

        public IRouter Register(MethodInfo methodInfo, HttpMethod httpMethod, string pathInfo)
        {
            AddToGlobalStack(new Route(methodInfo, httpMethod, pathInfo));
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
            if (routing == null || !routing.Any()) throw new RouteNotFound(context);
            if (context.WasRespondedTo()) return true;

            var routeContext = context;

            foreach (var route in routing)
            {
                if (!route.Enabled) continue;
                routeContext = route.Invoke(routeContext);
                if (routeContext.WasRespondedTo()) break;
            }

            return routeContext.WasRespondedTo();
        }

        internal IList<IRoute> GenerateRoutes(MethodInfo method, string baseUrl)
        {
            var routes = new List<IRoute>();

            var baseurl = string.IsNullOrWhiteSpace(baseUrl)
                ? string.Empty
                : baseUrl;

            if (baseurl != string.Empty && baseurl.EndsWith("/"))
                baseurl = baseurl.TrimEnd('/');

            if (baseurl != string.Empty && !baseurl.StartsWith("/"))
                baseurl = $"/{baseurl}";

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

                routes.Add(new Route(method, attribute.HttpMethod, $"{prefix}{baseurl}{pathinfo}"));
            }

            return routes;
        }

        internal IList<IRoute> GenerateRoutes(Type type)
        {
            var routes = new List<IRoute>();
            var baseurl = string.Empty;

            if (type.IsRestResource())
            {
                if (!string.IsNullOrWhiteSpace(Scope) && !Scope.Equals(type.RestResource().Scope)) return routes;
                baseurl = type.RestResource().BaseUrl;
            }

            foreach (var method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                routes.AddRange(GenerateRoutes(method, baseurl));
            }

            return routes;
        }

        internal IList<IRoute> GenerateRoutes(Assembly assembly)
        {
            var routes = new List<IRoute>();

            foreach (var type in assembly.GetTypes().Where(t => t.IsRestResource()))
            {
                if (Exclusions.Types.Contains(type)
                    || Exclusions.NameSpaces.Contains(type.Namespace))
                    continue;
                routes.AddRange(GenerateRoutes(type));
            }

            return routes;
        }

        private void AddToGlobalStack(IRoute route)
        {
            if (route.Function == null) throw new ArgumentNullException();
            if (!_routingTable.Contains(route)) _routingTable.Add(route);
        }

        private void AddRangeToGlobalStack(IEnumerable<IRoute> routes)
        {
            routes.ToList().ForEach(AddToGlobalStack);
        }
    }

    public interface IExclusions
    {
        IList<string> NameSpaces { get; }
        IList<Type> Types { get; }
        IExclusions AsReadOnly();
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
    }
}
