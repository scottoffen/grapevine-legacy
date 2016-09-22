using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Grapevine.Shared.Loggers;
using Grapevine.TestAssembly;
using Grapevine.Tests.Server.Attributes;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class RouteScannerFacts
    {
        protected Assembly GetTestAssembly()
        {
            return typeof(DefaultRouter).Assembly;
        }

        public class ExcludeMethod
        {
            [Fact]
            public void ExcludesAssemblies()
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
                var scanner = new RouteScanner();
                scanner.ExcludedAssemblies().Count.ShouldBe(0);

                scanner.Exclude(assembly);

                scanner.ExcludedAssemblies().Count.ShouldBe(1);
                scanner.ExcludedAssemblies()[0].ShouldBe(assembly);
            }

            [Fact]
            public void ExcludesGenericTypes()
            {
                var scanner = new RouteScanner();
                scanner.ExcludedTypes().Count.ShouldBe(0);

                scanner.Exclude<Route>();

                scanner.ExcludedTypes().Count.ShouldBe(1);
                scanner.ExcludedTypes()[0].ShouldBe(typeof(Route));
            }

            [Fact]
            public void ExcludesTypes()
            {
                var scanner = new RouteScanner();
                scanner.ExcludedTypes().Count.ShouldBe(0);

                scanner.Exclude(typeof(Route));

                scanner.ExcludedTypes().Count.ShouldBe(1);
                scanner.ExcludedTypes()[0].ShouldBe(typeof(Route));
            }

            [Fact]
            public void ExcludesNamespaces()
            {
                const string ns = "Grapevine.Tests.Server";
                var scanner = new RouteScanner();
                scanner.ExcludedNamespaces().Count.ShouldBe(0);

                scanner.Exclude(ns);

                scanner.ExcludedNamespaces().Count.ShouldBe(1);
                scanner.ExcludedNamespaces()[0].ShouldBe(ns);
            }

            [Fact]
            public void SkipsDuplicateAssemblies()
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
            public void SkipsDuplicateTypes()
            {
                var scanner = new RouteScanner();
                scanner.ExcludedTypes().Count.ShouldBe(0);

                scanner.Exclude<Route>();

                scanner.ExcludedTypes().Count.ShouldBe(1);
                scanner.ExcludedTypes()[0].ShouldBe(typeof(Route));

                scanner.Exclude(typeof(Router));

                scanner.ExcludedTypes().Count.ShouldBe(2);
                scanner.ExcludedTypes()[0].ShouldBe(typeof(Route));
                scanner.ExcludedTypes()[1].ShouldBe(typeof(Router));

                scanner.Exclude(typeof(Route));

                scanner.ExcludedTypes().Count.ShouldBe(2);
                scanner.ExcludedTypes()[0].ShouldBe(typeof(Route));
                scanner.ExcludedTypes()[1].ShouldBe(typeof(Router));
            }

            [Fact]
            public void SkipsDuplicateNamespaces()
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

            public class IsExcludedMethod
            {
                [Fact]
                public void ReturnsFalseWhenExcludesIsEmpty()
                {
                    var scanner = new RouteScanner();
                    scanner.CheckIsExcluded(AppDomain.CurrentDomain.GetAssemblies().First()).ShouldBeFalse();
                    scanner.CheckIsExcluded("Some.Other.Namespace").ShouldBeFalse();
                    scanner.CheckIsExcluded(typeof(Route)).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenAssemblyIsExcluded()
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
                    var scanner = new RouteScanner();
                    scanner.Exclude(assembly);
                    scanner.CheckIsExcluded(assembly).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenTypeIsExcluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Exclude(typeof(Route));
                    scanner.CheckIsExcluded(typeof(Route)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenGenericTypeIsExcluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Exclude<Route>();
                    scanner.CheckIsExcluded(typeof(Route)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenNamespaceIsExcluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Exclude("Some.Random.Namespace");
                    scanner.CheckIsExcluded("Some.Random.Namespace").ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenAssemblyIsNotExcluded()
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var assembly1 = assemblies[0];
                    var assembly2 = assemblies[2];
                    var scanner = new RouteScanner();

                    scanner.Exclude(assembly1);
                    scanner.CheckIsExcluded(assembly2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenTypeIsNotExcluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Exclude<Route>();
                    scanner.CheckIsExcluded(typeof(Router)).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenNamespaceIsNotExcluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Exclude("Some.Random.Namespace");
                    scanner.CheckIsExcluded("Some.Other.Namespace").ShouldBeFalse();
                }
            }
        }

        public class IncludeMethod
        {
            [Fact]
            public void IncludesAssemblies()
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
                var scanner = new RouteScanner();
                scanner.IncludedAssemblies().Count.ShouldBe(0);

                scanner.Include(assembly);

                scanner.IncludedAssemblies().Count.ShouldBe(1);
                scanner.IncludedAssemblies()[0].ShouldBe(assembly);
            }

            [Fact]
            public void IncludesGenericTypes()
            {
                var scanner = new RouteScanner();
                scanner.IncludedTypes().Count.ShouldBe(0);

                scanner.Include<Route>();

                scanner.IncludedTypes().Count.ShouldBe(1);
                scanner.IncludedTypes()[0].ShouldBe(typeof(Route));
            }

            [Fact]
            public void IncludesTypes()
            {
                var scanner = new RouteScanner();
                scanner.IncludedTypes().Count.ShouldBe(0);

                scanner.Include(typeof(Route));

                scanner.IncludedTypes().Count.ShouldBe(1);
                scanner.IncludedTypes()[0].ShouldBe(typeof(Route));
            }

            [Fact]
            public void IncludesNamespaces()
            {
                const string ns = "Grapevine.Tests.Server";
                var scanner = new RouteScanner();
                scanner.IncludedNamespaces().Count.ShouldBe(0);

                scanner.Include(ns);

                scanner.IncludedNamespaces().Count.ShouldBe(1);
                scanner.IncludedNamespaces()[0].ShouldBe(ns);
            }

            [Fact]
            public void SkipsDuplicateAssemblies()
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
            public void SkipsDuplicateTypes()
            {
                var scanner = new RouteScanner();
                scanner.IncludedTypes().Count.ShouldBe(0);

                scanner.Include<Route>();

                scanner.IncludedTypes().Count.ShouldBe(1);
                scanner.IncludedTypes()[0].ShouldBe(typeof(Route));

                scanner.Include(typeof(Router));

                scanner.IncludedTypes().Count.ShouldBe(2);
                scanner.IncludedTypes()[0].ShouldBe(typeof(Route));
                scanner.IncludedTypes()[1].ShouldBe(typeof(Router));

                scanner.Include(typeof(Route));

                scanner.IncludedTypes().Count.ShouldBe(2);
                scanner.IncludedTypes()[0].ShouldBe(typeof(Route));
                scanner.IncludedTypes()[1].ShouldBe(typeof(Router));
            }

            [Fact]
            public void SkipsDuplicateNamespaces()
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

            public class IsIncludedMethod
            {
                [Fact]
                public void ReturnsTrueWhenIncludesIsEmpty()
                {
                    var scanner = new RouteScanner();
                    scanner.CheckIsIncluded(AppDomain.CurrentDomain.GetAssemblies().First()).ShouldBeTrue();
                    scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenAssemblyIsIncluded()
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().First();
                    var scanner = new RouteScanner();
                    scanner.Include(assembly);
                    scanner.CheckIsIncluded(assembly).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenTypeIsIncluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Include(typeof(Route));
                    scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenGenericTypeIsIncluded()
                {
                    var scanner = new RouteScanner();
                    scanner.Include<Route>();
                    scanner.CheckIsIncluded(typeof(Route)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenAssemblyIsNotIncluded()
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var assembly1 = assemblies[0];
                    var assembly2 = assemblies[2];
                    var scanner = new RouteScanner();

                    scanner.Include(assembly1);
                    scanner.CheckIsIncluded(assembly2).ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenTypeIsNotInIncludes()
                {
                    var scanner = new RouteScanner();
                    scanner.Include<Route>();
                    scanner.CheckIsIncluded(typeof(Router)).ShouldBeFalse();
                }
            }
        }

        public class ScopeProperty
        {
            [Fact]
            public void SetScope()
            {
                const string scope = "MyScope";
                var scanner = new RouteScanner();
                scanner.GetScope().Equals(string.Empty).ShouldBeTrue();

                scanner.SetScope(scope);

                scanner.GetScope().Equals(scope).ShouldBeTrue();
            }

            public class IsInScopeMethod
            {
                [Fact]
                public void ReturnsTrueWhenTypeIsNotRestResource()
                {
                    var scanner = new RouteScanner();
                    scanner.CheckIsInScope(typeof(NotARestResource)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsTrueWhenScopeIsNullOrEmpty()
                {
                    var scanner = new RouteScanner();
                    scanner.CheckIsInScope(typeof(ClassInScopeA)).ShouldBeTrue();
                    scanner.CheckIsInScope(typeof(ClassInScopeB)).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenTypeScopeDoesNotMatch()
                {
                    var scanner = new RouteScanner();
                    scanner.SetScope("ScopeA");
                    scanner.CheckIsInScope(typeof(ClassInScopeB)).ShouldBeFalse();
                }

                [Fact]
                public void LogsWhenNotInScope()
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
                public void ReturnsTrueWhenTypeIsInScope()
                {
                    var scanner = new RouteScanner();
                    scanner.SetScope("ScopeA");
                    scanner.CheckIsInScope(typeof(ClassInScopeA)).ShouldBeTrue();
                }
            }
        }

        public class ScanMethod
        {
            [Fact]
            public void ReturnsRoutesAndLogs()
            {
                //ReflectionTypeLoadException exception = null;
                //var attempts = 0;

                //do
                //{
                //    attempts++;
                //    exception = null;

                //    try
                //    {
                        var logger = new InMemoryLogger();
                        var scanner = new RouteScanner { Logger = logger };

                        logger.Logs.ShouldBeEmpty();

                        var routes = scanner.Scan();

                        routes.ShouldNotBeNull();
                        routes.ShouldNotBeEmpty();
                        routes.Count.ShouldBeGreaterThanOrEqualTo(2);
                        logger.Logs.ShouldNotBeEmpty();
                //    }
                //    catch (ReflectionTypeLoadException ex)
                //    {
                //        exception = ex;
                //        Thread.Sleep(417);
                //    }
                //} while (exception != null || attempts > 5);

                //if (exception != null) throw exception;
            }

            [Fact]
            public void ReturnsRoutesAndLogsWithInclusions()
            {
                //ReflectionTypeLoadException exception = null;
                //var attempts = 0;

                //do
                //{
                //    attempts++;
                //    exception = null;

                //    try
                //    {
                        var logger = new InMemoryLogger();
                        var scanner = new RouteScanner { Logger = logger };

                        scanner.Include(Assembly.GetExecutingAssembly());
                        logger.Logs.ShouldBeEmpty();

                        var routes = scanner.Scan();

                        routes.ShouldNotBeNull();
                        routes.Count.ShouldBe(2);
                        logger.Logs.ShouldNotBeEmpty();
                //    }
                //    catch (ReflectionTypeLoadException ex)
                //    {
                //        exception = ex;
                //        Thread.Sleep(231);
                //    }
                //} while (exception != null || attempts > 5);

                //if (exception != null) throw exception;
            }

            [Fact]
            public void ReturnsRoutesAndLogsWithExclusions()
            {
                //ReflectionTypeLoadException exception = null;
                //var attempts = 0;

                //do
                //{
                //    attempts++;
                //    exception = null;

                //    try
                //    {
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
                //    }
                //    catch (ReflectionTypeLoadException ex)
                //    {
                //        exception = ex;
                //        Thread.Sleep(780);
                //    }
                //} while (exception != null || attempts > 5);

                //if (exception != null) throw exception;
            }
        }

        public class ScanAssemblyMethod : RouteScannerFacts
        {
            [Fact]
            public void ReturnsEmptyWhenNoRoutesAreInAssembly()
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
            public void ReturnsRoutesAndLogs()
            {
                var assembly = GetTestAssembly();
                var logger = new InMemoryLogger();
                var scanner = new RouteScanner { Logger = logger };

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
            public void ReturnsRoutesAndLogsWithBaseUrlArgument()
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
            public void ReturnsEmptyIfAssemblyIsExcluded()
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
        }

        public class ScanTypeMethod
        {
            [Fact]
            public void ReturnsEmptyAndDoesNotLogAbstractType()
            {
                var logger = new InMemoryLogger();
                var scanner = new RouteScanner { Logger = logger };

                logger.Logs.ShouldBeEmpty();

                var routes = scanner.ScanType(typeof(ScannableAbstract));

                routes.ShouldNotBeNull();
                routes.ShouldBeEmpty();
                logger.Logs.ShouldBeEmpty();
            }

            [Fact]
            public void ReturnsEmptyAndDoesNotLogNonClassType()
            {
                var logger = new InMemoryLogger();
                var scanner = new RouteScanner { Logger = logger };

                logger.Logs.ShouldBeEmpty();

                var routes = scanner.ScanType(typeof(IScannable));

                routes.ShouldNotBeNull();
                routes.ShouldBeEmpty();
                logger.Logs.ShouldBeEmpty();
            }

            [Fact]
            public void ReturnsRoutesAndLogsWithoutAttribute()
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
            public void ReturnsRoutesAndLogsWithAttributeWithoutBasepath()
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
            public void ReturnsRoutesAndLogsWithAttributeWithBasepath()
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
            public void ReturnsRoutesAndLogsWithAttributeWithBasepathAndArgument()
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
            public void ReturnsRoutesAndLogsWithArgumentAndBasepathInAttribute()
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
        }

        public class ScanMethodMethod
        {
            [Fact]
            public void ThrowsExceptionWhenMethodIsNotEligible()
            {
                var scanner = new RouteScanner();
                var method = typeof(MethodsToScan).GetMethod("InvalidRoute");
                Should.Throw<InvalidRouteMethodExceptions>(() => scanner.ScanMethod(method));
            }

            [Fact]
            public void ReturnsNoRoutesWhenMethodHasNoAttribute()
            {
                var scanner = new RouteScanner();
                var method = typeof(MethodsToScan).GetMethod("HasNoAttributes");

                var routes = scanner.ScanMethod(method);

                routes.ShouldNotBeNull();
                routes.ShouldBeEmpty();
            }

            [Fact]
            public void ReturnsOneRouteWhenMethodHasOneAttribute()
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
            public void ReturnsMultipleRoutesWhenMethodHasMultipleAttributes()
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
            public void AppliesBasepathToRoutesReturned()
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
            public void LogsMessageForGeneratedRoutes()
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
        }

        public class GenerateBasePathMethod
        {
            [Fact]
            public void ReturnsEmptyStringWhenBasepathIsNull()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator(null, typeof(MethodsToScan)).ShouldBe("");
            }

            [Fact]
            public void ReturnsEmptyStringWhenBasepathIsEmpty()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator(string.Empty, typeof(MethodsToScan)).ShouldBe("");
            }

            [Fact]
            public void ReturnsEmptyStringWhenAttributeHasNoBasepathAndBasepathIsEmpty()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator(string.Empty, typeof(TypeWithoutBasePath)).ShouldBe("");
            }

            [Fact]
            public void ReturnsBasepathWhenTypeHasNoAttribute()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator("thing", typeof(MethodsToScan)).ShouldBe("thing");
            }

            [Fact]
            public void ReturnsBasepathWithForwardSlashes()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator("/thing/", typeof(MethodsToScan)).ShouldBe("/thing/");
            }

            [Fact]
            public void ReturnsAttributeBasepathWhenBasepathIsEmpty()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator(string.Empty, typeof(TypeWithBasePath)).ShouldBe("/with");
            }

            [Fact]
            public void AppendsBasepathAndTypeBasepath()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator("thing", typeof(TypeWithBasePath)).ShouldBe("thing/with");
            }

            [Fact]
            public void DoesNotDuplicateForwardSlashesWhenAppending()
            {
                var scanner = new RouteScanner();
                scanner.BasePathGenerator("/thing/", typeof(TypeWithBasePath)).ShouldBe("/thing/with");
            }
        }

        public class SanitizeBasePathMethod
        {
            [Fact]
            public void ReturnsEmptyWhenBasepathIsNull()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(null).ShouldBe(string.Empty);
            }

            [Fact]
            public void ReturnsEmptyWhenBasepathIsEmpty()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(string.Empty).ShouldBe(string.Empty);
            }

            [Fact]
            public void AppendsLeadingForwardSlash()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer("path").ShouldBe("/path");
            }

            [Fact]
            public void DoesNotDuplicateLeadingForwardSlash()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer("/path").ShouldBe("/path");
            }

            [Fact]
            public void TrimsTrailingForwardSlash()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer("path/").ShouldBe("/path");
            }

            [Fact]
            public void TrimsLeadingWhitespace()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(" path").ShouldBe("/path");
            }

            [Fact]
            public void TrimsTrailingWhitespace()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer("path ").ShouldBe("/path");
            }

            [Fact]
            public void TrimsLeadingAndTrailingWhitespace()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(" path ").ShouldBe("/path");
            }

            [Fact]
            public void TrimsWhitespaceAndTrailingForwardSlash()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(" path/ ").ShouldBe("/path");
            }

            [Fact]
            public void TrimsWhitespaceAndDoesNotDuplicateLeadingForwardSlash()
            {
                var scanner = new RouteScanner();
                scanner.BasePathSanitizer(" /path ").ShouldBe("/path");
            }
        }

        public class GeneratePathInfoMethod
        {
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
        }
    }

    public class MethodsToScan
    {
        [RestRoute]
        public void InvalidRoute() { /* method intentionally left blank */ }

        public IHttpContext HasNoAttributes(IHttpContext context) { return context; }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

        [RestRoute(HttpMethod = HttpMethod.DELETE, PathInfo = "/more/stuff")]
        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/stuff/[id]")]
        public IHttpContext HasMultipleAttributes(IHttpContext context) { return context; }
    }

    [RestResource]
    public class TypeWithoutBasePath
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

    }

    [RestResource(BasePath = "/with")]
    public class TypeWithBasePath
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }

    }

    public abstract class ScannableAbstract
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        public IHttpContext HasOneAttribute(IHttpContext context) { return context; }
    }

    public interface IScannable
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stuff")]
        IHttpContext HasOneAttribute(IHttpContext context);
    }

    public class ScanEmptyClass {  /* class body intentionally left blank */ }

    [RestResource(Scope = "ScopeA")]
    public class ClassInScopeA {  /* class body intentionally left blank */ }

    [RestResource(Scope = "ScopeB")]
    public class ClassInScopeB {  /* class body intentionally left blank */ }

    public static class RouteScannerExtensions
    {
        internal static List<string> ExcludedNamespaces(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedNamespaces", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<string>)field?.GetValue(scanner);
        }

        internal static List<string> IncludedNamespaces(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedNamespaces", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<string>)field?.GetValue(scanner);
        }

        internal static List<Type> ExcludedTypes(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Type>)field?.GetValue(scanner);
        }

        internal static List<Type> IncludedTypes(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Type>)field?.GetValue(scanner);
        }

        internal static List<Assembly> ExcludedAssemblies(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_excludedAssemblies", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Assembly>)field?.GetValue(scanner);
        }

        internal static List<Assembly> IncludedAssemblies(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_includedAssemblies", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<Assembly>)field?.GetValue(scanner);
        }

        internal static string GetScope(this IRouteScanner scanner)
        {
            var memberInfo = scanner.GetType();
            var field = memberInfo?.GetField("_scope", BindingFlags.Instance | BindingFlags.NonPublic);
            return (string)field?.GetValue(scanner);
        }

        internal static bool CheckIsInScope(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsInScope", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Type) }, null);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, string nameSpace)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
            return (bool)method.Invoke(scanner, new object[] { nameSpace });
        }

        internal static bool CheckIsExcluded(this IRouteScanner scanner, Assembly assembly)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsExcluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Assembly) }, null);
            return (bool)method.Invoke(scanner, new object[] { assembly });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Type) }, null);
            return (bool)method.Invoke(scanner, new object[] { type });
        }

        internal static bool CheckIsIncluded(this IRouteScanner scanner, Assembly assembly)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("IsIncluded", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Assembly) }, null);
            return (bool)method.Invoke(scanner, new object[] { assembly });
        }

        internal static string PathInfoGenerator(this IRouteScanner scanner, string pathinfo, string basepath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("GeneratePathInfo", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { pathinfo, basepath });
        }

        internal static string BasePathGenerator(this IRouteScanner scanner, string basepath, Type type)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("GenerateBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basepath, type });
        }

        internal static string BasePathSanitizer(this IRouteScanner scanner, string basepath)
        {
            var memberInfo = scanner.GetType();
            var method = memberInfo?.GetMethod("SanitizeBasePath", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method.Invoke(null, new object[] { basepath });
        }
    }
}
