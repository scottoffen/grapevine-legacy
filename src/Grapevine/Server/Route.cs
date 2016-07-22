using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Util;
using System.Text.RegularExpressions;

namespace Grapevine.Server
{
    public interface IRoute
    {
        /// <summary>
        /// Gets or sets an optional description for the route
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the route is enabled
        /// </summary>
        bool Enabled { get; }

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
        /// Enables the route, allowing it to be invoked during routing
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables the route, preventing it from being invoked during routing
        /// </summary>
        void Disable();

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
        public bool Enabled { get; protected set; }
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
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            Function = ConvertMethodToFunc(methodInfo);
            if (methodInfo.ReflectedType != null) Name = $"{methodInfo.ReflectedType.FullName}.{methodInfo.Name}";
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

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public IHttpContext Invoke(IHttpContext context)
        {
            if (context.WasRespondedTo) return context;
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
        protected ReadOnlyDictionary<string, string> ParseParams(string pathinfo)
        {
            var parsed = new Dictionary<string, string>();
            var idx = 0;

            foreach (Match match in PathInfoPattern.Matches(pathinfo))
            {
                if (match.Success)
                {
                    var key = PatternKeys.Count > 0 && PatternKeys.Count > idx ? PatternKeys[idx] : $"p{idx}";
                    parsed.Add(key, match.Groups[1].Value);
                }
                idx++;
            }

            return new ReadOnlyDictionary<string, string>(parsed);
        }

        /// <summary>
        /// Returns a generic delegate for the given MethodInfo
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        protected Func<IHttpContext, IHttpContext> ConvertMethodToFunc(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.ReturnType != typeof(IHttpContext)) throw new RouteMethodArgumentException($"{methodInfo.Name}: Return type must be of type {typeof(IHttpContext).Name}");

            var args = methodInfo.GetParameters();
            if (args.Length != 1) throw new RouteMethodArgumentException($"{methodInfo.Name}: must have one and only one argument to be used as a {typeof(RestRoute).Name}");
            if (args[0].ParameterType != typeof(IHttpContext)) throw new RouteMethodArgumentException($"{methodInfo.Name}: argument must be of type {typeof(IHttpContext).Name}");

            Func<IHttpContext, IHttpContext> function;

            if (methodInfo.IsStatic)
            {
                function = context => (IHttpContext)methodInfo.Invoke(null, new object[] { context });
            }
            else
            {
                function = context =>
                {
                    var instance = methodInfo.ReflectedType != null ? Activator.CreateInstance(methodInfo.ReflectedType) : null;
                    return (IHttpContext)methodInfo.Invoke(instance, new object[] { context });
                };
            }

            return function;
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
}