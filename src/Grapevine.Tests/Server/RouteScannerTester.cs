using System;
using System.Linq;
using Grapevine.Server;
using Grapevine.Tests.Server.Attributes.Helpers;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteScannerTester
    {
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

        /* Scan Method
         * 
         */

        /* Scan Type
         * 
         */

        /* Scan Assembly
         * 
         */

        /* Scan
         * 
         */

        [Fact]
        public void scanner_sanitizes_basepath()
        {
            var scanner = new RouteScanner();
            scanner.BasePathSanitizer(null).ShouldBe("/");
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
