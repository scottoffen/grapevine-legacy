using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Server.Exceptions;
using Grapevine.TestAssembly;
using Grapevine.Tests.Server.Attributes.Helpers;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteScannerTester
    {
        private Assembly GetTestAssembly()
        {
            return typeof(DefaultRouter).Assembly;
        }

        [Fact]
        public void scanner_excludes_assemblies()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
            var scanner = new RouteScanner();
            scanner.ExcludedAssemblies().Count.ShouldBe(0);

            scanner.Exclude(assembly);

            scanner.ExcludedAssemblies().Count.ShouldBe(1);
            scanner.ExcludedAssemblies()[0].ShouldBe(assembly);
        }

        [Fact]
        public void scanner_excludes_generic_types()
        {
            var scanner = new RouteScanner();
            scanner.ExcludedTypes().Count.ShouldBe(0);

            scanner.Exclude<RouterTester>();

            scanner.ExcludedTypes().Count.ShouldBe(1);
            scanner.ExcludedTypes()[0].ShouldBe(typeof(RouterTester));
        }

        [Fact]
        public void scanner_excludes_types()
        {
            var scanner = new RouteScanner();
            scanner.ExcludedTypes().Count.ShouldBe(0);

            scanner.Exclude(typeof(RouterTester));

            scanner.ExcludedTypes().Count.ShouldBe(1);
            scanner.ExcludedTypes()[0].ShouldBe(typeof(RouterTester));
        }

        [Fact]
        public void scanner_excludes_namespaces()
        {
            const string ns = "Grapevine.Tests.Server";
            var scanner = new RouteScanner();
            scanner.ExcludedNamespaces().Count.ShouldBe(0);

            scanner.Exclude(ns);

            scanner.ExcludedNamespaces().Count.ShouldBe(1);
            scanner.ExcludedNamespaces()[0].ShouldBe(ns);
        }

        [Fact]
        public void scanner_excludes_skips_duplicate_assemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly1 = assemblies[0];
            var assembly2 = assemblies[1];

            var scanner = new RouteScanner();
            scanner.ExcludedAssemblies().Count.ShouldBe(0);

            scanner.Exclude(assembly1);

            scanner.ExcludedAssemblies().Count.ShouldBe(1);
            scanner.ExcludedAssemblies()[0].ShouldBe(assembly1);

            scanner.Exclude(assembly2);

            scanner.ExcludedAssemblies().Count.ShouldBe(2);
            scanner.ExcludedAssemblies()[0].ShouldBe(assembly1);
            scanner.ExcludedAssemblies()[1].ShouldBe(assembly2);

            scanner.Exclude(assembly1);

            scanner.ExcludedAssemblies().Count.ShouldBe(2);
            scanner.ExcludedAssemblies()[0].ShouldBe(assembly1);
            scanner.ExcludedAssemblies()[1].ShouldBe(assembly2);
        }

        [Fact]
        public void scanner_excludes_skips_duplicate_types()
        {
            var scanner = new RouteScanner();
            scanner.ExcludedTypes().Count.ShouldBe(0);

            scanner.Exclude<RouterTester>();

            scanner.ExcludedTypes().Count.ShouldBe(1);
            scanner.ExcludedTypes()[0].ShouldBe(typeof(RouterTester));

            scanner.Exclude(typeof(RouteTester));

            scanner.ExcludedTypes().Count.ShouldBe(2);
            scanner.ExcludedTypes()[0].ShouldBe(typeof(RouterTester));
            scanner.ExcludedTypes()[1].ShouldBe(typeof(RouteTester));

            scanner.Exclude(typeof(RouterTester));

            scanner.ExcludedTypes().Count.ShouldBe(2);
            scanner.ExcludedTypes()[0].ShouldBe(typeof(RouterTester));
            scanner.ExcludedTypes()[1].ShouldBe(typeof(RouteTester));
        }

        [Fact]
        public void scanner_excludes_skips_duplicate_namespaces()
        {
            const string ns1 = "Grapevine.Tests.Server";
            const string ns2 = "Grapevine.Tests.Client";

            var scanner = new RouteScanner();
            scanner.ExcludedNamespaces().Count.ShouldBe(0);

            scanner.Exclude(ns1);

            scanner.ExcludedNamespaces().Count.ShouldBe(1);
            scanner.ExcludedNamespaces()[0].ShouldBe(ns1);

            scanner.Exclude(ns2);

            scanner.ExcludedNamespaces().Count.ShouldBe(2);
            scanner.ExcludedNamespaces()[0].ShouldBe(ns1);
            scanner.ExcludedNamespaces()[1].ShouldBe(ns2);

            scanner.Exclude(ns1);

            scanner.ExcludedNamespaces().Count.ShouldBe(2);
            scanner.ExcludedNamespaces()[0].ShouldBe(ns1);
            scanner.ExcludedNamespaces()[1].ShouldBe(ns2);
        }

        [Fact]
        public void scanner_includes_assemblies()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
            var scanner = new RouteScanner();
            scanner.IncludedAssemblies().Count.ShouldBe(0);

            scanner.Include(assembly);

            scanner.IncludedAssemblies().Count.ShouldBe(1);
            scanner.IncludedAssemblies()[0].ShouldBe(assembly);
        }

        [Fact]
        public void scanner_includes_generic_types()
        {
            var scanner = new RouteScanner();
            scanner.IncludedTypes().Count.ShouldBe(0);

            scanner.Include<RouterTester>();

            scanner.IncludedTypes().Count.ShouldBe(1);
            scanner.IncludedTypes()[0].ShouldBe(typeof(RouterTester));
        }

        [Fact]
        public void scanner_includes_types()
        {
            var scanner = new RouteScanner();
            scanner.IncludedTypes().Count.ShouldBe(0);

            scanner.Include(typeof(RouterTester));

            scanner.IncludedTypes().Count.ShouldBe(1);
            scanner.IncludedTypes()[0].ShouldBe(typeof(RouterTester));
        }

        [Fact]
        public void scanner_includes_namespaces()
        {
            const string ns = "Grapevine.Tests.Server";
            var scanner = new RouteScanner();
            scanner.IncludedNamespaces().Count.ShouldBe(0);

            scanner.Include(ns);

            scanner.IncludedNamespaces().Count.ShouldBe(1);
            scanner.IncludedNamespaces()[0].ShouldBe(ns);
        }

        [Fact]
        public void scanner_includes_skips_duplicate_assemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly1 = assemblies[0];
            var assembly2 = assemblies[1];

            var scanner = new RouteScanner();
            scanner.IncludedAssemblies().Count.ShouldBe(0);

            scanner.Include(assembly1);

            scanner.IncludedAssemblies().Count.ShouldBe(1);
            scanner.IncludedAssemblies()[0].ShouldBe(assembly1);

            scanner.Include(assembly2);

            scanner.IncludedAssemblies().Count.ShouldBe(2);
            scanner.IncludedAssemblies()[0].ShouldBe(assembly1);
            scanner.IncludedAssemblies()[1].ShouldBe(assembly2);

            scanner.Include(assembly1);

            scanner.IncludedAssemblies().Count.ShouldBe(2);
            scanner.IncludedAssemblies()[0].ShouldBe(assembly1);
            scanner.IncludedAssemblies()[1].ShouldBe(assembly2);
        }

        [Fact]
        public void scanner_includes_skips_duplicate_types()
        {
            var scanner = new RouteScanner();
            scanner.IncludedTypes().Count.ShouldBe(0);

            scanner.Include<RouterTester>();

            scanner.IncludedTypes().Count.ShouldBe(1);
            scanner.IncludedTypes()[0].ShouldBe(typeof(RouterTester));

            scanner.Include(typeof(RouteTester));

            scanner.IncludedTypes().Count.ShouldBe(2);
            scanner.IncludedTypes()[0].ShouldBe(typeof(RouterTester));
            scanner.IncludedTypes()[1].ShouldBe(typeof(RouteTester));

            scanner.Include(typeof(RouterTester));

            scanner.IncludedTypes().Count.ShouldBe(2);
            scanner.IncludedTypes()[0].ShouldBe(typeof(RouterTester));
            scanner.IncludedTypes()[1].ShouldBe(typeof(RouteTester));
        }

        [Fact]
        public void scanner_includes_skips_duplicate_namespaces()
        {
            const string ns1 = "Grapevine.Tests.Server";
            const string ns2 = "Grapevine.Tests.Client";

            var scanner = new RouteScanner();
            scanner.IncludedNamespaces().Count.ShouldBe(0);

            scanner.Include(ns1);

            scanner.IncludedNamespaces().Count.ShouldBe(1);
            scanner.IncludedNamespaces()[0].ShouldBe(ns1);

            scanner.Include(ns2);

            scanner.IncludedNamespaces().Count.ShouldBe(2);
            scanner.IncludedNamespaces()[0].ShouldBe(ns1);
            scanner.IncludedNamespaces()[1].ShouldBe(ns2);

            scanner.Include(ns1);

            scanner.IncludedNamespaces().Count.ShouldBe(2);
            scanner.IncludedNamespaces()[0].ShouldBe(ns1);
            scanner.IncludedNamespaces()[1].ShouldBe(ns2);
        }

        [Fact]
        public void scanner_set_scope()
        {
            const string scope = "MyScope";
            var scanner = new RouteScanner();
            scanner.GetScope().Equals(string.Empty).ShouldBeTrue();

            scanner.SetScope(scope);

            scanner.GetScope().Equals(scope).ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_true_if_includes_is_empty()
        {
            var scanner = new RouteScanner();
            scanner.CheckIsIncluded(AppDomain.CurrentDomain.GetAssemblies().First()).ShouldBeTrue();
            scanner.CheckIsIncluded("Some.Random.Namespace").ShouldBeTrue();
            scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_true_if_assembly_is_in_includes()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
            var scanner = new RouteScanner();
            scanner.Include(assembly);
            scanner.CheckIsIncluded(assembly).ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_true_if_type_is_in_includes()
        {
            var scanner = new RouteScanner();
            scanner.Include(typeof(Route));
            scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_true_if_generic_type_is_in_includes()
        {
            var scanner = new RouteScanner();
            scanner.Include<Route>();
            scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_true_if_namespace_is_in_includes()
        {
            var scanner = new RouteScanner();
            scanner.Include("Some.Random.Namespace");
            scanner.CheckIsIncluded("Some.Random.Namespace").ShouldBeTrue();
        }

        [Fact]
        public void scanner_includes_is_false_if_assembly_is_not_in_includes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly1 = assemblies[0];
            var assembly2 = assemblies[2];
            var scanner = new RouteScanner();

            scanner.Include(assembly1);
            scanner.CheckIsIncluded(assembly2).ShouldBeFalse();
        }

        [Fact]
        public void scanner_includes_is_false_if_type_is_not_in_includes()
        {
            var scanner = new RouteScanner();
            scanner.Include<Route>();
            scanner.CheckIsIncluded(typeof(Router)).ShouldBeFalse();
        }

        [Fact]
        public void scanner_includes_is_false_if_namespace_is_not_in_includes()
        {
            var scanner = new RouteScanner();
            scanner.Include("Some.Other.Namespace");
            scanner.CheckIsIncluded("Some.Random.Namespace").ShouldBeFalse();
        }

        [Fact]
        public void scanner_excludes_is_false_if_excludes_is_empty()
        {
            var scanner = new RouteScanner();
            scanner.CheckIsExcluded(AppDomain.CurrentDomain.GetAssemblies().First()).ShouldBeFalse();
            scanner.CheckIsExcluded("Some.Other.Namespace").ShouldBeFalse();
            scanner.CheckIsExcluded(typeof(Route)).ShouldBeFalse();
        }

        [Fact]
        public void scanner_excludes_is_true_if_assembly_is_in_excludes()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
            var scanner = new RouteScanner();
            scanner.Exclude(assembly);
            scanner.CheckIsExcluded(assembly).ShouldBeTrue();
        }

        [Fact]
        public void scanner_excludes_is_true_if_type_is_in_excludes()
        {
            var scanner = new RouteScanner();
            scanner.Exclude(typeof(Route));
            scanner.CheckIsExcluded(typeof(Route)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_excludes_is_true_if_generic_type_is_in_excludes()
        {
            var scanner = new RouteScanner();
            scanner.Exclude<Route>();
            scanner.CheckIsExcluded(typeof(Route)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_excludes_is_true_if_namespace_is_in_excludes()
        {
            var scanner = new RouteScanner();
            scanner.Exclude("Some.Random.Namespace");
            scanner.CheckIsExcluded("Some.Random.Namespace").ShouldBeTrue();
        }

        [Fact]
        public void scanner_excludes_is_false_if_assembly_is_not_in_excludes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly1 = assemblies[0];
            var assembly2 = assemblies[2];
            var scanner = new RouteScanner();

            scanner.Exclude(assembly1);
            scanner.CheckIsExcluded(assembly2).ShouldBeFalse();
        }

        [Fact]
        public void scanner_excludes_is_false_if_type_is_not_in_excludes()
        {
            var scanner = new RouteScanner();
            scanner.Exclude<Route>();
            scanner.CheckIsExcluded(typeof(Router)).ShouldBeFalse();
        }

        [Fact]
        public void scanner_excludes_is_false_if_namespace_is_not_in_excludes()
        {
            var scanner = new RouteScanner();
            scanner.Exclude("Some.Random.Namespace");
            scanner.CheckIsExcluded("Some.Other.Namespace").ShouldBeFalse();
        }

        [Fact]
        public void scanner_isinscope_returns_true_when_type_is_not_rest_resource()
        {
            var scanner = new RouteScanner();
            scanner.CheckIsInScope(typeof(NotARestResource)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_isinscope_returns_true_when_scanner_scope_is_null_or_empty()
        {
            var scanner = new RouteScanner();
            scanner.CheckIsInScope(typeof(ClassInScopeA)).ShouldBeTrue();
            scanner.CheckIsInScope(typeof(ClassInScopeB)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_isinscope_returns_false_when_scanner_scope_does_not_match_type_scope()
        {
            var scanner = new RouteScanner();
            scanner.SetScope("ScopeA");
            scanner.CheckIsInScope(typeof(ClassInScopeB)).ShouldBeFalse();
        }

        [Fact]
        public void scanner_isinscope_logs_to_logger_when_returns_false()
        {
            var type = typeof(ClassInScopeB);
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner();
            scanner.SetScope("ScopeA");
            scanner.Logger = logger;

            logger.Logs.Count.ShouldBe(0);

            scanner.CheckIsInScope(type).ShouldBeFalse();

            logger.Logs.Count.ShouldBe(1);
            logger.Logs[0].Message.Equals($"Excluding type {type.Name} due to scoping differences").ShouldBeTrue();
        }

        [Fact]
        public void scanner_isinscope_returns_true_when_type_is_in_scope()
        {
            var scanner = new RouteScanner();
            scanner.SetScope("ScopeA");
            scanner.CheckIsInScope(typeof(ClassInScopeA)).ShouldBeTrue();
        }

        [Fact]
        public void scanner_scan_method_throws_exception_when_scanning_ineligible_method()
        {
            var scanner = new RouteScanner();
            var method = typeof(MethodsToScan).GetMethod("InvalidRoute");
            Should.Throw<InvalidRouteMethodExceptions>(() => scanner.ScanMethod(method));
        }

        [Fact]
        public void scanner_scan_method_returns_no_routes_when_method_has_no_attribute()
        {
            var scanner = new RouteScanner();
            var method = typeof(MethodsToScan).GetMethod("HasNoAttributes");

            var routes = scanner.ScanMethod(method);

            routes.ShouldNotBeNull();
            routes.ShouldBeEmpty();
        }

        [Fact]
        public void scanner_scan_method_returns_one_route_when_method_has_one_attribute()
        {
            var scanner = new RouteScanner();
            var method = typeof(MethodsToScan).GetMethod("HasOneAttribute");

            var routes = scanner.ScanMethod(method);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(1);

            var route = routes[0];
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe("/stuff");
        }

        [Fact]
        public void scanner_scan_method_returns_multiple_routes_when_method_has_multiple_attributes()
        {
            var scanner = new RouteScanner();
            var method = typeof(MethodsToScan).GetMethod("HasMultipleAttributes");

            var routes = scanner.ScanMethod(method);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(2);

            var route1 = routes[0];
            route1.HttpMethod.ShouldBe(HttpMethod.DELETE);
            route1.PathInfo.ShouldBe("/more/stuff");

            var route2 = routes[1];
            route2.HttpMethod.ShouldBe(HttpMethod.POST);
            route2.PathInfo.ShouldBe("/stuff/[id]");
        }

        [Fact]
        public void scanner_scan_method_applies_basepath_to_routes_returned()
        {
            var scanner = new RouteScanner();
            var method = typeof(MethodsToScan).GetMethod("HasOneAttribute");

            var routes1 = scanner.ScanMethod(method, "/dog");
            var routes2 = scanner.ScanMethod(method, "cat");

            routes1.ShouldNotBeNull();
            routes2.ShouldNotBeNull();
            routes1.Count.ShouldBe(1);
            routes2.Count.ShouldBe(1);


            routes1[0].HttpMethod.ShouldBe(HttpMethod.GET);
            routes1[0].PathInfo.ShouldBe("/dog/stuff");

            routes2[0].HttpMethod.ShouldBe(HttpMethod.GET);
            routes2[0].PathInfo.ShouldBe("/cat/stuff");
        }

        [Fact]
        public void scanner_scan_method_logs_message_for_generated_routes()
        {
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };
            var method = typeof(MethodsToScan).GetMethod("HasMultipleAttributes");

            var routes = scanner.ScanMethod(method);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(2);

            var route1 = routes[0];
            var route2 = routes[1];

            logger.Logs.Count.ShouldBe(2);
            logger.Logs[0].Message.ShouldBe($"Generated route {route1.HttpMethod} {route1.PathInfo} > {route1.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route2.HttpMethod} {route2.PathInfo} > {route2.Name}");
        }

        [Fact]
        public void scanner_scan_type_returns_empty_and_does_not_log_when_scanning_abstract_type()
        {
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner {Logger = logger};

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(typeof(AbstractToScan));

            routes.ShouldNotBeNull();
            routes.ShouldBeEmpty();
            logger.Logs.ShouldBeEmpty();
        }

        [Fact]
        public void scanner_scan_type_returns_empty_and_does_not_log_when_scanning_non_class_type()
        {
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(typeof(InterfaceToScan));

            routes.ShouldNotBeNull();
            routes.ShouldBeEmpty();
            logger.Logs.ShouldBeEmpty();
        }

        [Fact]
        public void scanner_scan_type_returns_routes_and_logs_without_attribute()
        {
            var type = typeof(MethodsToScan);
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(type);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(3);

            var route0 = routes[0];
            route0.HttpMethod.ShouldBe(HttpMethod.GET);
            route0.PathInfo.ShouldBe("/stuff");

            var route1 = routes[1];
            route1.HttpMethod.ShouldBe(HttpMethod.DELETE);
            route1.PathInfo.ShouldBe("/more/stuff");

            var route2 = routes[2];
            route2.HttpMethod.ShouldBe(HttpMethod.POST);
            route2.PathInfo.ShouldBe("/stuff/[id]");

            logger.Logs.Count.ShouldBe(4);
            logger.Logs[0].Message.ShouldBe($"Generating routes from type {type.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route0.HttpMethod} {route0.PathInfo} > {route0.Name}");
            logger.Logs[2].Message.ShouldBe($"Generated route {route1.HttpMethod} {route1.PathInfo} > {route1.Name}");
            logger.Logs[3].Message.ShouldBe($"Generated route {route2.HttpMethod} {route2.PathInfo} > {route2.Name}");
        }

        [Fact]
        public void scanner_scan_type_returns_routes_and_logs_with_attribute_without_basepath()
        {
            var type = typeof(TypeWithoutBasePath);
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(type);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(1);

            var route = routes[0];
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe("/stuff");

            logger.Logs.Count.ShouldBe(2);
            logger.Logs[0].Message.ShouldBe($"Generating routes from type {type.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route.HttpMethod} {route.PathInfo} > {route.Name}");
        }

        [Fact]
        public void scanner_scan_type_returns_routes_and_logs_with_attribute_with_basepath()
        {
            var type = typeof(TypeWithBasePath);
            var logger = new InMemoryLogger();
            var basepath = type.GetRestResource().BasePath;
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(type);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(1);

            var route = routes[0];
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe($"{basepath}/stuff");

            logger.Logs.Count.ShouldBe(2);
            logger.Logs[0].Message.ShouldBe($"Generating routes from type {type.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route.HttpMethod} {route.PathInfo} > {route.Name}");
        }

        [Fact]
        public void scanner_scan_type_returns_routes_and_logs_with_attribute_with_basepath_argument()
        {
            var type = typeof(TypeWithoutBasePath);
            var logger = new InMemoryLogger();
            var basepath = "/use_args";
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(type, basepath);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(1);

            var route = routes[0];
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe($"{basepath}/stuff");

            logger.Logs.Count.ShouldBe(2);
            logger.Logs[0].Message.ShouldBe($"Generating routes from type {type.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route.HttpMethod} {route.PathInfo} > {route.Name}");
        }

        [Fact]
        public void scanner_scan_type_returns_routes_and_logs_when_basepath_is_arguement_and_attribute()
        {
            var type = typeof(TypeWithBasePath);
            var logger = new InMemoryLogger();
            var basepath = "/override";
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanType(type, basepath);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(1);

            var route = routes[0];
            route.HttpMethod.ShouldBe(HttpMethod.GET);
            route.PathInfo.ShouldBe($"{basepath}/with/stuff");

            logger.Logs.Count.ShouldBe(2);
            logger.Logs[0].Message.ShouldBe($"Generating routes from type {type.Name}");
            logger.Logs[1].Message.ShouldBe($"Generated route {route.HttpMethod} {route.PathInfo} > {route.Name}");
        }

        [Fact]
        public void scanner_scan_assembly_returns_empty_when_no_routes_in_assembly()
        {
            var assembly = typeof(RouteScanner).Assembly;
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanAssembly(assembly);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(0);

            logger.Logs.Count.ShouldBe(1);
            logger.Logs[0].Message.ShouldBe($"Generating routes for assembly {assembly.GetName().Name}");
        }

        [Fact]
        public void scanner_scan_assembly_returns_routes_and_logs()
        {
            var assembly = GetTestAssembly();
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner {Logger = logger};

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanAssembly(assembly);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(8);

            routes[0].HttpMethod.ShouldBe(HttpMethod.GET);
            routes[0].PathInfo.ShouldBe("/todo/list");

            routes[4].HttpMethod.ShouldBe(HttpMethod.GET);
            routes[4].PathInfo.ShouldBe("/user/list");

            logger.Logs.Count.ShouldBe(11);
            logger.Logs[0].Message.ShouldBe($"Generating routes for assembly {assembly.GetName().Name}");
        }

        [Fact]
        public void scanner_scan_assembly_returns_routes_and_logs_with_baseurl_argument()
        {
            var assembly = GetTestAssembly();
            var baseurl = "/api";
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            var routes = scanner.ScanAssembly(assembly, baseurl);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(8);

            routes[0].HttpMethod.ShouldBe(HttpMethod.GET);
            routes[0].PathInfo.ShouldBe($"{baseurl}/todo/list");

            routes[4].HttpMethod.ShouldBe(HttpMethod.GET);
            routes[4].PathInfo.ShouldBe($"{baseurl}/user/list");

            logger.Logs.Count.ShouldBe(11);
            logger.Logs[0].Message.ShouldBe($"Generating routes for assembly {assembly.GetName().Name}");
        }

        [Fact]
        public void scanner_scan_assembly_returns_empty_if_assembly_is_excluded()
        {
            var assembly = GetTestAssembly();
            var type = typeof(ToDoListRoutes);
            var logger = new InMemoryLogger();
            var scanner = new RouteScanner { Logger = logger };

            logger.Logs.ShouldBeEmpty();

            scanner.Exclude(type);
            var routes = scanner.ScanAssembly(assembly);

            routes.ShouldNotBeNull();
            routes.Count.ShouldBe(4);

            routes[0].HttpMethod.ShouldBe(HttpMethod.GET);
            routes[0].PathInfo.ShouldBe("/user/list");

            logger.Logs.Count.ShouldBe(7);
            logger.Logs[1].Message.ShouldBe($"Excluding type {type.Name} due to exclusion rules");
        }

        [Fact]
        public void scanner_scan_returns_routes_and_logs()
        {
            ReflectionTypeLoadException exception = null;
            var attempts = 0;

            do
            {
                attempts++;
                exception = null;

                try
                {
                    var logger = new InMemoryLogger();
                    var scanner = new RouteScanner {Logger = logger};

                    logger.Logs.ShouldBeEmpty();

                    var routes = scanner.Scan();

                    routes.ShouldNotBeNull();
                    routes.ShouldNotBeEmpty();
                    routes.Count.ShouldBeGreaterThanOrEqualTo(2);
                    logger.Logs.ShouldNotBeEmpty();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exception = ex;
                    Thread.Sleep(417);
                }
            } while (exception != null || attempts > 5);

            if (exception != null) throw exception;
        }

        [Fact]
        public void scanner_scan_returns_routes_and_logs_with_inclusions()
        {
            ReflectionTypeLoadException exception = null;
            var attempts = 0;

            do
            {
                attempts++;
                exception = null;

                try
                {
                    var logger = new InMemoryLogger();
                    var scanner = new RouteScanner { Logger = logger };

                    scanner.Include(Assembly.GetExecutingAssembly());
                    logger.Logs.ShouldBeEmpty();

                    var routes = scanner.Scan();

                    routes.ShouldNotBeNull();
                    routes.Count.ShouldBe(2);
                    logger.Logs.ShouldNotBeEmpty();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exception = ex;
                    Thread.Sleep(231);
                }
            } while (exception != null || attempts > 5);

            if (exception != null) throw exception;
        }

        [Fact]
        public void scanner_scan_returns_routes_and_logs_with_exclusions()
        {
            ReflectionTypeLoadException exception = null;
            var attempts = 0;

            do
            {
                attempts++;
                exception = null;

                try
                {

                    var logger = new InMemoryLogger();
                    var scanner = new RouteScanner { Logger = logger };

                    logger.Logs.ShouldBeEmpty();

                    var routesBefore = scanner.Scan();

                    scanner.Exclude(Assembly.GetExecutingAssembly());

                    var routesAfter = scanner.Scan();

                    routesBefore.ShouldNotBeNull();
                    routesAfter.ShouldNotBeNull();
                    (routesBefore.Count - routesAfter.Count).ShouldBe(2);
                    logger.Logs.ShouldNotBeEmpty();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exception = ex;
                    Thread.Sleep(780);
                }
            } while (exception != null || attempts > 5);

            if (exception != null) throw exception;
        }

        [Fact]
        public void scanner_generates_basepath()
        {
            var scanner = new RouteScanner();

            scanner.BasePathGenerator(null, typeof(MethodsToScan)).ShouldBe("");
            scanner.BasePathGenerator(string.Empty, typeof(MethodsToScan)).ShouldBe("");
            scanner.BasePathGenerator(string.Empty, typeof(TypeWithoutBasePath)).ShouldBe("");
            scanner.BasePathGenerator("thing", typeof(MethodsToScan)).ShouldBe("thing");
            scanner.BasePathGenerator("/thing/", typeof(MethodsToScan)).ShouldBe("/thing/");
            scanner.BasePathGenerator(string.Empty, typeof(TypeWithBasePath)).ShouldBe("/with");
            scanner.BasePathGenerator(string.Empty, typeof(TypeWithBasePath)).ShouldBe("/with");
            scanner.BasePathGenerator("thing", typeof(TypeWithBasePath)).ShouldBe("thing/with");
            scanner.BasePathGenerator("/thing/", typeof(TypeWithBasePath)).ShouldBe("/thing/with");
        }

        [Fact]
        public void scanner_generates_pathinfo()
        {
            const string basepath1 = "/base1";
            const string basepath2 = "base2";

            var scanner = new RouteScanner();
            var empty = string.Empty;

            scanner.PathInfoGenerator("^/api/resource", empty).Equals("^/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("^api/resource", empty).Equals("^/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("/api/resource", empty).Equals("/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("api/resource", empty).Equals("/api/resource").ShouldBeTrue();

            scanner.PathInfoGenerator("^/api/resource", basepath1).Equals("^/base1/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("^api/resource", basepath1).Equals("^/base1/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("/api/resource", basepath1).Equals("/base1/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("api/resource", basepath1).Equals("/base1/api/resource").ShouldBeTrue();

            scanner.PathInfoGenerator("^/api/resource", basepath2).Equals("^/base2/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("^api/resource", basepath2).Equals("^/base2/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("/api/resource", basepath2).Equals("/base2/api/resource").ShouldBeTrue();
            scanner.PathInfoGenerator("api/resource", basepath2).Equals("/base2/api/resource").ShouldBeTrue();
        }

        [Fact]
        public void scanner_sanitizes_basepath()
        {
            var scanner = new RouteScanner();
            scanner.BasePathSanitizer(null).ShouldBe(string.Empty);
            scanner.BasePathSanitizer(string.Empty).ShouldBe(string.Empty);
            scanner.BasePathSanitizer("path").ShouldBe("/path");
            scanner.BasePathSanitizer("/path").ShouldBe("/path");
            scanner.BasePathSanitizer("path/").ShouldBe("/path");
            scanner.BasePathSanitizer(" path").ShouldBe("/path");
            scanner.BasePathSanitizer("path ").ShouldBe("/path");
            scanner.BasePathSanitizer(" path ").ShouldBe("/path");
            scanner.BasePathSanitizer(" path/ ").ShouldBe("/path");
            scanner.BasePathSanitizer(" /path ").ShouldBe("/path");
        }
    }
}
