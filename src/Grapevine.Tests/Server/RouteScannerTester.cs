using Grapevine.Server;
using Grapevine.Tests.Server.Helpers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteScannerTester
    {
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
            scanner.CheckIsIncluded("Some.Random.Namespace").ShouldBeTrue();
            scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
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
            scanner.CheckIsExcluded("Some.Other.Namespace").ShouldBeFalse();
            scanner.CheckIsExcluded(typeof(Route)).ShouldBeFalse();
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

        /* IsInScope */

        /* Scan Method */

        /* Scan Type */

        /* Scan Assembly */

        /* Scan */

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
