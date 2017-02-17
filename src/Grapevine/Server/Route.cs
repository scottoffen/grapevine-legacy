using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Shared;
using System.Text.RegularExpressions;
using Grapevine.Server.Attributes;
using Grapevine.Interfaces.Server;

namespace Grapevine.Server
{
    /// <summary>
    /// Represents code to be executed when a request is recieved and the conditions under which it should execute
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Gets or sets an optional description for the route
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the route is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the generic delegate that will be run when the route is invoked
        /// </summary>
        Func<IHttpContext, IHttpContext> Function { get; }

        /// <summary>
        /// Gets the HttpMethod that this route responds to; defaults to HttpMethod.ALL
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// Gets a unique name for function that will be invoked in the route, internally assigned
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the PathInfo that this method responds to
        /// </summary>
        string PathInfo { get; }

        /// <summary>
        /// Get the PathInfo regular expression used to match this method to requests
        /// </summary>
        Regex PathInfoPattern { get; }

        /// <summary>
        /// Returns the result of the route delegate being executed on the IHttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <returns>IHttpContext</returns>
        IHttpContext Invoke(IHttpContext context);

        /// <summary>
        /// Gets a value indicating whether the route matches the given IHttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <returns>bool</returns>
        bool Matches(IHttpContext context);
    }

    public class Route : IRoute
    {
        /// <summary>
        /// The pattern keys specified in the PathInfo
        /// </summary>
        protected readonly List<string> PatternKeys;

        public string Description { get; set; }
        public bool Enabled { get; set; }
        public Func<IHttpContext, IHttpContext> Function { get; }
        public HttpMethod HttpMethod { get; }
        public string Name { get; }
        public string PathInfo { get; }
        public Regex PathInfoPattern { get; }

        /// <summary>
        /// Creates a route from the given MethodInfo; defaults to HttpMethod.All and an empty PathInfo
        /// </summary>
        /// <param name="methodInfo"></param>
        public Route(MethodInfo methodInfo) : this(methodInfo, HttpMethod.ALL, string.Empty) { }

        /// <summary>
        /// Creates a route from the given MethodInfo and PathInfo; defaults to HttpMethod.All
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="pathInfo"></param>
        public Route(MethodInfo methodInfo, string pathInfo) : this(methodInfo, HttpMethod.ALL, pathInfo) { }

        /// <summary>
        /// Creates a route from the given MethodInfo and HttpMethod; defaults to an empty PathInfo
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="httpMethod"></param>
        public Route(MethodInfo methodInfo, HttpMethod httpMethod) : this(methodInfo, httpMethod, string.Empty) { }

        /// <summary>
        /// Creates a route from the given MethodInfo, HttpMethod and PathInfo
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        public Route(MethodInfo methodInfo, HttpMethod httpMethod, string pathInfo) : this(httpMethod, pathInfo)
        {
            Function = ConvertMethodToFunc(methodInfo);
            Name = $"{methodInfo.ReflectedType.FullName}.{methodInfo.Name}";
            Description = $"{HttpMethod} {PathInfo} > {Name}";
        }

        /// <summary>
        /// Creates a route from the given generic delegate; defaults to HttpMethod.All and an empty PathInfo
        /// </summary>
        /// <param name="func"></param>
        public Route(Func<IHttpContext, IHttpContext> func) : this(func, HttpMethod.ALL, string.Empty) { }

        /// <summary>
        /// Creates a route from the given generic delegate and PathInfo; defaults to HttpMethod.All
        /// </summary>
        /// <param name="func"></param>
        /// <param name="pathInfo"></param>
        public Route(Func<IHttpContext, IHttpContext> func, string pathInfo) : this(func, HttpMethod.ALL, pathInfo) { }

        /// <summary>
        /// Creates a route from the given generic delegate and HttpMethod; defaults to an empty PathInfo
        /// </summary>
        /// <param name="func"></param>
        /// <param name="httpMethod"></param>
        public Route(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod) : this(func, httpMethod, string.Empty) { }

        /// <summary>
        /// Creates a route from the given generic delegate, HttpMethod and PathInfo
        /// </summary>
        /// <param name="function"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        public Route(Func<IHttpContext, IHttpContext> function, HttpMethod httpMethod, string pathInfo) : this(httpMethod, pathInfo)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            Function = function;
            Name = $"{Function.Method.ReflectedType}.{Function.Method.Name}";
            Description = $"{HttpMethod} {PathInfo} > {Name}";
        }

        /// <summary>
        /// Fluent interface for route creation
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static FluentRouteCreator.RouteToExpression For(HttpMethod httpMethod)
        {
            return new FluentRouteCreator.RouteToExpression(httpMethod);
        }

        /// <summary>
        /// Base constructor for a route
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        private Route(HttpMethod httpMethod, string pathInfo)
        {
            Enabled = true;
            HttpMethod = httpMethod;
            PathInfo = !string.IsNullOrWhiteSpace(pathInfo) ? pathInfo : string.Empty;
            PatternKeys = PatternParser.GeneratePatternKeys(PathInfo);
            PathInfoPattern = PatternParser.GenerateRegEx(PathInfo);
        }

        public IHttpContext Invoke(IHttpContext context)
        {
            if (!Enabled) return context;
            context.Request.PathParameters = ParseParams(context.Request.PathInfo);
            return Function(context);
        }

        public bool Matches(IHttpContext context)
        {
            return HttpMethod.IsEquivalent(context.Request.HttpMethod) && PathInfoPattern.IsMatch(context.Request.PathInfo);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Route)) return false;
            var route = (Route)obj;

            if (Name != route.Name) return false;

            if (!HttpMethod.IsEquivalent(route.HttpMethod)) return false;

            return PathInfoPattern.IsMatch(route.PathInfo) || route.PathInfoPattern.IsMatch(PathInfo);
        }

        public override string ToString()
        {
            return $"{HttpMethod} {PathInfo} > {Name}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Gets a dictionary of parameter values from the PathInfo of the request
        /// </summary>
        /// <param name="pathinfo"></param>
        /// <returns></returns>
        protected Dictionary<string, string> ParseParams(string pathinfo)
        {
            var parsed = new Dictionary<string, string>();
            var idx = 0;

            var matches = PathInfoPattern.Matches(pathinfo)[0].Groups;
            for (int i = 1; i < matches.Count; i++)
            {
                var key = PatternKeys.Count > 0 && PatternKeys.Count > idx ? PatternKeys[idx] : $"p{idx}";
                parsed.Add(key, matches[i].Value);
                idx++;
            }

            return parsed;
        }

        /// <summary>
        /// Returns a generic delegate for the given MethodInfo if it is rest route eligible
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Func<IHttpContext, IHttpContext> ConvertMethodToFunc(MethodInfo method)
        {
            method.IsRestRouteEligible(true); // throws an aggregate exception if the method is not eligible

            // Static method
            if (method.IsStatic || method.ReflectedType == null)
            {
                return context => (IHttpContext) method.Invoke(null, new object[] {context});
            }

            // Generates new instance every time
            return context =>
            {
                var instance = Activator.CreateInstance(method.ReflectedType);
                var disposed = false;
                try
                {
                    var ctx = (IHttpContext) method.Invoke(instance, new object[] {context});
                    disposed = instance.TryDisposing();
                    return ctx;
                }
                finally
                {
                    if (!disposed) instance.TryDisposing();
                }
            };

            // TODO: Create and use a cached instance, like in earlier versions of Grapevine?
        }
    }

    /// <summary>
    /// Fluent interface for route creation
    /// </summary>
    public class FluentRouteCreator
    {
        /// <summary>
        /// Fluent interface for specifying the PathInfo and MethodInfo or generic delegate for a new Route
        /// </summary>
        public class RouteToExpression : IRouteUseExpression
        {
            private readonly HttpMethod _httpMethod;
            private string _pathInfo;

            public RouteToExpression(HttpMethod httpMethod)
            {
                _httpMethod = httpMethod;
            }

            /// <summary>
            /// Provide the PathInfo for the new Route
            /// </summary>
            /// <param name="pathinfo"></param>
            /// <returns></returns>
            public IRouteUseExpression To (string pathinfo)
            {
                _pathInfo = pathinfo;
                return this;
            }

            public Route Use(MethodInfo methodInfo)
            {
                return new Route(methodInfo, _httpMethod);
            }

            public Route Use(Func<IHttpContext, IHttpContext> function)
            {
                return new Route(function, _httpMethod);
            }

            Route IRouteUseExpression.Use(MethodInfo methodInfo)
            {
                return new Route(methodInfo, _httpMethod, _pathInfo);
            }

            Route IRouteUseExpression.Use(Func<IHttpContext, IHttpContext> function)
            {
                return new Route(function, _httpMethod, _pathInfo);
            }
        }

        /// <summary>
        /// Creates a new route for the specified values
        /// </summary>
        public interface IRouteUseExpression
        {
            /// <summary>
            /// Provide a MethodInfo for the new Route
            /// </summary>
            /// <param name="methodInfo"></param>
            /// <returns></returns>
            Route Use(MethodInfo methodInfo);

            /// <summary>
            /// Provide a generic delegate for the new Route
            /// </summary>
            /// <param name="function"></param>
            /// <returns></returns>
            Route Use(Func<IHttpContext, IHttpContext> function);
        }
    }

    public static class RouteInterfaceExtensions
    {
        /// <summary>
        /// Enables the route, allowing it to be invoked during routing
        /// </summary>
        public static void Enable(this IRoute route)
        {
            route.Enabled = true;
        }

        /// <summary>
        /// Disables the route, preventing it from being invoked during routing
        /// </summary>
        public static void Disable(this IRoute route)
        {
            route.Enabled = false;
        }
    }
}
