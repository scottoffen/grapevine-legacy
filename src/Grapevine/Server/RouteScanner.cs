using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Interfaces.Shared;
using Grapevine.Server.Attributes;
using Grapevine.Shared.Loggers;

namespace Grapevine.Server
{
    public interface IRouteScanner
    {
        /// <summary>
        /// Set a rule to exclude types in the specified namespace when auto-scanning for routes
        /// </summary>
        /// <param name="nameSpace"></param>
        [Obsolete("This will be removed in the next release")]
        void Exclude(string nameSpace);

        /// <summary>
        /// Set a rule to exclude the specified type when auto-scanning for routes
        /// </summary>
        /// <param name="type"></param>
        void Exclude(Type type);

        /// <summary>
        /// Set a rule to exclude the generic type when auto-scanning for routes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Exclude<T>();

        /// <summary>
        /// Set a rule to exclude the specified assembly when auto-scanning for routes
        /// </summary>
        /// <param name="assembly"></param>
        void Exclude(Assembly assembly);

        /// <summary>
        /// Set a rule to include types in the specified namespace when auto-scanning for routes
        /// </summary>
        /// <param name="nameSpace"></param>
        [Obsolete("This will be removed in the next release")]
        void Include(string nameSpace);

        /// <summary>
        /// Set a rule to include the specified type when auto-scanning for routes
        /// </summary>
        /// <param name="type"></param>
        void Include(Type type);

        /// <summary>
        /// Set a rule to include the generic type when auto-scanning for routes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Include<T>();

        /// <summary>
        /// Set a rule to include the specified assembly when auto-scanning for routes
        /// </summary>
        /// <param name="assembly"></param>
        void Include(Assembly assembly);

        /// <summary>
        /// Gets or sets the logger
        /// </summary>
        IGrapevineLogger Logger { get; set; }

        /// <summary>
        /// Set a rule to include only RestResources with the specified scope when auto-scanning for routes
        /// </summary>
        /// <param name="scope"></param>
        void SetScope(string scope);

        /// <summary>
        /// Generates a list of routes for all RestResource classes found in all assemblies in the current AppDomain
        /// </summary>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> Scan();

        /// <summary>
        /// Generates a list of routes for all RestResource classes found in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanAssembly(Assembly assembly);

        /// <summary>
        /// Generates a list of routes for all RestResource classes found in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="basePath"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanAssembly(Assembly assembly, string basePath);

        /// <summary>
        /// Generates a list of routes for all RestRoute attributed methods found in the class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanType(Type type);

        /// <summary>
        /// Generates a list of routes for all RestRoute attributed methods found in the class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basePath"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanType(Type type, string basePath);

        /// <summary>
        /// Generates a list of routes for the RestRoute attributed MethodInfo provided and the basePath applied to the PathInfo
        /// </summary>
        /// <param name="method"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanMethod(MethodInfo method);

        /// <summary>
        /// Generates a list of routes for the RestRoute attributed MethodInfo provided and the basePath applied to the PathInfo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="basePath"></param>
        /// <returns>IList&lt;IRoute&gt;</returns>
        IList<IRoute> ScanMethod(MethodInfo method, string basePath);
    }

    public sealed class RouteScanner : IRouteScanner
    {
        private readonly List<string> _excludedNamespaces;
        private readonly List<string> _includedNamespaces;
        private readonly List<Type> _excludedTypes;
        private readonly List<Type> _includedTypes;
        private readonly List<Assembly> _excludedAssemblies;
        private readonly List<Assembly> _includedAssemblies;
        private string _scope = string.Empty;

        public IGrapevineLogger Logger { get; set; }

        public static readonly List<Assembly> Assemblies;

        static RouteScanner()
        {
            Assemblies = new List<Assembly>();
            foreach (
                var assembly in
                    AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => !a.GlobalAssemblyCache && a.GetName().Name != "Grapevine" && !a.GetName().Name.StartsWith("vshost"))
                        .OrderBy(a => a.FullName))
            {Assemblies.Add(assembly);}
        }

        internal RouteScanner()
        {
            Logger = NullLogger.GetInstance();

            _excludedNamespaces = new List<string>();
            _includedNamespaces = new List<string>();

            _excludedTypes = new List<Type>();
            _includedTypes = new List<Type>();

            _excludedAssemblies = new List<Assembly>();
            _includedAssemblies = new List<Assembly>();
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

        public void Exclude(Assembly assembly)
        {
            if (!_excludedAssemblies.Contains(assembly)) _excludedAssemblies.Add(assembly);
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

        public void Include(Assembly assembly)
        {
            if (!_includedAssemblies.Contains(assembly)) _includedAssemblies.Add(assembly);
        }

        private bool IsExcluded(string nameSpace)
        {
            return _excludedNamespaces.Contains(nameSpace);
        }

        private bool IsExcluded(Type type)
        {
            if (!_excludedTypes.Contains(type) && !IsExcluded(type.Namespace)) return false;
            Logger.Trace($"Excluding type {type.Name} due to exclusion rules");
            return true;
        }

        private bool IsExcluded(Assembly assembly)
        {
            if (!_excludedAssemblies.Contains(assembly)) return false;
            Logger.Trace($"Excluding assembly {assembly.GetName().Name} due to exclusion rules");
            return true;
        }

        private bool IsIncluded(Type type)
        {
            if (!_includedTypes.Any() || _includedTypes.Contains(type)) return true;
            Logger.Trace($"Excluding type {type.Name} due to inclusion rules");
            return false;
        }

        private bool IsIncluded(Assembly assembly)
        {
            if (!_includedAssemblies.Any() || _includedAssemblies.Contains(assembly)) return true;
            Logger.Trace($"Excluding assembly {assembly.GetName().Name} due to inclusion rules");
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
            var routes = new List<IRoute>();

            Logger.Trace("Scanning resources for routes...");

            foreach (var assembly in Assemblies)
            {
                if (IsExcluded(assembly) || !IsIncluded(assembly)) continue;
                routes.AddRange(ScanAssembly(assembly));
            }

            return routes;
        }

        public IList<IRoute> ScanAssembly(Assembly assembly)
        {
            return ScanAssembly(assembly, string.Empty);
        }

        public IList<IRoute> ScanAssembly(Assembly assembly, string basePath)
        {
            var routes = new List<IRoute>();

            Logger.Trace($"Generating routes for assembly {assembly.GetName().Name}");

            foreach (var type in assembly.GetTypes().Where(t => t.IsRestResource()).OrderBy(m => m.Name))
            {
                if (IsExcluded(type) || !IsIncluded(type)) continue;
                routes.AddRange(ScanType(type, basePath));
            }

            return routes;
        }

        public IList<IRoute> ScanType(Type type)
        {
            return ScanType(type, string.Empty);
        }

        public IList<IRoute> ScanType(Type type, string basePath)
        {
            var routes = new List<IRoute>();
            if (type.IsAbstract || !type.IsClass || IsExcluded(type) || !IsIncluded(type) || !IsInScope(type)) return routes;

            Logger.Trace($"Generating routes from type {type.Name}");

            var basepath = GenerateBasePath(basePath, type);
            foreach (var method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                routes.AddRange(ScanMethod(method, basepath));
            }

            return routes;
        }

        public IList<IRoute> ScanMethod(MethodInfo method)
        {
            return ScanMethod(method, string.Empty);
        }

        public IList<IRoute> ScanMethod(MethodInfo method, string basePath)
        {
            var routes = new List<IRoute>();
            var basepath = SanitizeBasePath(basePath);

            foreach (var attribute in method.GetCustomAttributes(true).Where(a => a is RestRoute).Cast<RestRoute>())
            {
                var pathinfo = GeneratePathInfo(attribute.PathInfo, basepath);
                var route = new Route(method, attribute.HttpMethod, pathinfo);
                Logger.Trace($"Generated route {route}");
                routes.Add(route);
            }

            return routes;
        }

        private static string GeneratePathInfo(string pathInfo, string basePath)
        {
            var pathinfo = pathInfo;
            var prefix = string.Empty;

            if (pathinfo.StartsWith("^"))
            {
                prefix = "^";
                pathinfo = pathinfo.TrimStart('^');
            }

            if (!string.IsNullOrEmpty(pathinfo) && !pathinfo.StartsWith("/")) pathinfo = $"/{pathinfo}";
            if (!string.IsNullOrEmpty(basePath) && !basePath.StartsWith("/")) basePath = $"/{basePath}";

            return $"{prefix}{basePath}{pathinfo}";
        }

        private static string GenerateBasePath(string basePath, Type type)
        {
            var bpArgument = basePath ?? string.Empty;
            var bpOnResource = type.IsRestResource() ? type.GetRestResource().BasePath : string.Empty;

            if (string.IsNullOrWhiteSpace(bpArgument)) return bpOnResource;
            if (string.IsNullOrWhiteSpace(bpOnResource)) return bpArgument;

            bpArgument = bpArgument.TrimEnd('/');
            bpOnResource = bpOnResource.TrimStart('/');

            return $"{bpArgument}/{bpOnResource}";
        }

        private static string SanitizeBasePath(string basePath)
        {
            var basepath = basePath?.Trim().TrimEnd('/').TrimStart('/') ?? string.Empty;
            return string.IsNullOrWhiteSpace(basepath) ? basepath : $"/{basepath}";
        }
    }
}
