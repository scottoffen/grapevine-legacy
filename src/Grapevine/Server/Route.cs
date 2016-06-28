using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Util;
using System.Text.RegularExpressions;

namespace Grapevine.Server
{
    public interface IRoute
    {
        string Name { get; }
        Func<IHttpContext, IHttpContext> Function { get; }
        HttpMethod HttpMethod { get; }
        string PathInfo { get; }
        Regex PathInfoPattern { get; }

        bool Matches(IHttpContext context);
        IHttpContext Invoke(IHttpContext context);
    }

    public class Route : IRoute
    {
        protected readonly List<string> PatternKeys;

        public string Name { get; protected set; }
        public Func<IHttpContext, IHttpContext> Function { get; protected set; }
        public HttpMethod HttpMethod { get; protected set; }
        public string PathInfo { get; protected set; }
        public Regex PathInfoPattern { get; protected set; }

        public static FluentRouteCreator.ToExpression For(HttpMethod httpMethod)
        {
            return new FluentRouteCreator.ToExpression(httpMethod);
        }

        public Route(MethodInfo methodInfo) : this(methodInfo, HttpMethod.ALL, string.Empty) { }

        public Route(MethodInfo methodInfo, string pathInfo) : this(methodInfo, HttpMethod.ALL, pathInfo) { }

        public Route(MethodInfo methodInfo, HttpMethod httpMethod) : this(methodInfo, httpMethod, string.Empty) { }

        public Route(MethodInfo methodInfo, HttpMethod httpMethod, string pathInfo):this(httpMethod, pathInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            Function = ConvertMethodToFunc(methodInfo);
            if (methodInfo.ReflectedType != null) Name = $"{methodInfo.ReflectedType.FullName}.{methodInfo.Name}";
        }

        public Route(Func<IHttpContext, IHttpContext> func) : this(func, HttpMethod.ALL, string.Empty) { }

        public Route(Func<IHttpContext, IHttpContext> func, string pathInfo):this(func, HttpMethod.ALL, pathInfo) { }

        public Route(Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod) : this(func, httpMethod, string.Empty) { }

        public Route(Func<IHttpContext, IHttpContext> function, HttpMethod httpMethod, string pathInfo):this(httpMethod, pathInfo)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            Function = function;
            Name = $"{Function.Method.ReflectedType}.{Function.Method.Name}";
        }

        private Route(HttpMethod httpMethod, string pathInfo)
        {
            HttpMethod = httpMethod;
            PathInfo = (pathInfo != null) ? (!string.IsNullOrWhiteSpace(pathInfo)) ? pathInfo : string.Empty : string.Empty;
            PatternKeys = PatternParser.GeneratePatternKeys(PathInfo);
            PathInfoPattern = PatternParser.GenerateRegEx(PathInfo);
        }

        public bool Matches(IHttpContext context)
        {
            return HttpMethod.IsEquivalent(context.Request.HttpMethod) && PathInfoPattern.IsMatch(context.Request.PathInfo);
        }

        public IHttpContext Invoke(IHttpContext context)
        {
            if (context.WasRespondedTo()) return context;
            context.Request.PathParameters = new ReadOnlyDictionary<string, string>(ParseParams(context.Request.PathInfo));
            return Function(context);
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

        protected Dictionary<string, string> ParseParams(string pathinfo)
        {
            var parsed = new Dictionary<string, string>();
            var idx = 0;

            foreach (Match match in PathInfoPattern.Matches(pathinfo))
            {
                if (match.Success)
                {
                    var pname = (PatternKeys.Count > 0 && PatternKeys.Count > idx) ? PatternKeys[idx] : $"p{idx}";
                    parsed.Add(pname, match.Groups[1].Value);
                }
                idx++;
            }

            return parsed;
        }

        private Func<IHttpContext, IHttpContext> ConvertMethodToFunc(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.ReturnType != typeof(IHttpContext)) throw new RouteMethodArgumentException($"{methodInfo.Name}: Return type must be of type {typeof(IHttpContext).Name}");

            var pars = methodInfo.GetParameters();
            if (pars.Length != 1) throw new RouteMethodArgumentException($"{methodInfo.Name}: must have one and only one argument if it to be used as a {typeof(RestRoute).Name}");
            if (pars[0].ParameterType != typeof(IHttpContext)) throw new RouteMethodArgumentException($"{methodInfo.Name}: first argument must be of type {typeof(IHttpContext).Name}");

            var func = new Func<IHttpContext, IHttpContext>(context =>
            {
                var instance = methodInfo.IsStatic ? null
                    : (methodInfo.ReflectedType != null) ? Activator.CreateInstance(methodInfo.ReflectedType) : null;
                return (IHttpContext)methodInfo.Invoke(instance, new object[] { context });
            });

            return func;
        }
    }

    public class FluentRouteCreator
    {
        public class ToExpression : IRouteUseExpression
        {
            private readonly HttpMethod _httpMethod;
            private string _pathInfo;

            public ToExpression(HttpMethod httpMethod)
            {
                _httpMethod = httpMethod;
            }

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

        public interface IRouteUseExpression
        {
            Route Use(MethodInfo methodInfo);
            Route Use(Func<IHttpContext, IHttpContext> function);
        }
    }
}
