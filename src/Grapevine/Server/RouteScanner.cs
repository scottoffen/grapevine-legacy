using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Server.Attributes;
using Grapevine.Util;
using Grapevine.Util.Loggers;

namespace Grapevine.Server
{
    public interface IRouteScanner
    {
        void Exclude(string nameSpace);

        void Exclude(Type type);

        void Exclude<T>();

        void Include(string nameSpace);

        void Include(Type type);

        void Include<T>();

        IGrapevineLogger Logger { get; set; }

        void SetScope(string scope);

        IList<IRoute> Scan();

        IList<IRoute> ScanAssembly(Assembly assembly);

        IList<IRoute> ScanType(Type type);

        IList<IRoute> ScanMethod(MethodInfo method, string basePath);
    }

    public sealed class RouteScanner : IRouteScanner
    {
        private readonly List<string> _excludedNamespaces;
        private readonly List<string> _includedNamespaces;
        private readonly List<Type> _excludedTypes;
        private readonly List<Type> _includedTypes;
        private string _scope = string.Empty;

        public IGrapevineLogger Logger { get; set; }

        internal RouteScanner()
        {
            Logger = NullLogger.GetInstance();

            _excludedNamespaces = new List<string>();
            _includedNamespaces = new List<string>();
            _excludedTypes = new List<Type>();
            _includedTypes = new List<Type>();
        }

        public void Exclude(string nameSpace)
        {
            if (!_excludedNamespaces.Contains(nameSpace)) _excludedNamespaces.Add(nameSpace);
        }

        public void Exclude(Type type)
        {
            if (!_excludedTypes.Contains(type)) _excludedTypes.Add(type);
        }

        public void Exclude<T>()
        {
            Exclude(typeof(T));
        }

        public void Include(string nameSpace)
        {
            if (!_includedNamespaces.Contains(nameSpace)) _includedNamespaces.Add(nameSpace);
        }

        public void Include(Type type)
        {
            if (!_includedTypes.Contains(type)) _includedTypes.Add(type);
        }

        public void Include<T>()
        {
            Include(typeof(T));
        }

        private bool IsExcluded(string nameSpace)
        {
            return _excludedNamespaces.Contains(nameSpace);
        }

        private bool IsExcluded(Type type)
        {
            if (!_excludedTypes.Contains(type)) return false;
            Logger.Trace($"Excluding type {type.Name} due to exclusion rules");
            return true;
        }

        private bool IsIncluded(string nameSpace)
        {
            return !_includedNamespaces.Any() || _includedNamespaces.Contains(nameSpace);
        }

        private bool IsIncluded(Type type)
        {
            if (!_includedTypes.Any() || _includedTypes.Contains(type)) return true;
            Logger.Trace($"Excluding type {type.Name} due to inclusion rules");
            return false;
        }

        private bool IsInScope(Type type)
        {
            if (!type.IsRestResource() || string.IsNullOrWhiteSpace(_scope) ||
                _scope.Equals(type.GetRestResource().Scope)) return true;

            Logger.Trace($"Excluding type {type.Name} due to scoping differences");
            return false;
        }

        public void SetScope(string scope)
        {
            _scope = scope;
        }

        public IList<IRoute> Scan()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            return ScanAssembly(assembly);
        }

        /// <summary>
        /// Generates a list of routes for all RestResource classes found in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        public IList<IRoute> ScanAssembly(Assembly assembly)
        {
            var routes = new List<IRoute>();

            Logger.Trace($"Generating routes for assembly {assembly.GetName().Name}");

            foreach (var type in assembly.GetTypes().Where(t => t.IsRestResource()))
            {
                if (IsExcluded(type) || !IsIncluded(type)) continue;
                routes.AddRange(ScanType(type));
            }

            return routes;
        }

        /// <summary>
        /// Generates a list of routes for all RestRoute attributed methods found in the class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        public IList<IRoute> ScanType(Type type)
        {
            var routes = new List<IRoute>();
            if (IsExcluded(type.Namespace) || !IsIncluded(type.Namespace) || !IsInScope(type)) return routes;

            Logger.Trace($"Generating routes from type {type.Name}");

            var basepath = type.IsRestResource() ? type.GetRestResource().BasePath : string.Empty;
            foreach (var method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                routes.AddRange(ScanMethod(method, basepath));
            }

            return routes;
        }

        /// <summary>
        /// Generates a list of routes for the RestRoute attributed MethodInfo provided and the basePath applied to the PathInfo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="basePath"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        public IList<IRoute> ScanMethod(MethodInfo method, string basePath)
        {
            var routes = new List<IRoute>();
            var basepath = SanitizeBasePath(basePath);

            foreach (var attribute in method.GetCustomAttributes(true).Where(a => a is RestRoute).Cast<RestRoute>())
            {
                var pathinfo = attribute.PathInfo;
                var prefix = string.Empty;

                if (pathinfo.StartsWith("^"))
                {
                    prefix = "^";
                    pathinfo = pathinfo.TrimStart('^');
                }

                if (!string.IsNullOrEmpty(pathinfo) && !pathinfo.StartsWith("/")) pathinfo = $"/{pathinfo}";

                var route = new Route(method, attribute.HttpMethod, $"{prefix}{basepath}{pathinfo}");
                Logger.Trace($"Generated route {route}");
                routes.Add(route);
            }

            return routes;
        }

        private static string SanitizeBasePath(string basePath)
        {
            var basepath = basePath?.Trim().TrimEnd('/') ?? string.Empty;
            if (!basepath.StartsWith("/")) basepath = $"/{basepath}";
            return basepath;
        }
    }
}
