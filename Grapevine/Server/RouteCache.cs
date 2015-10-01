using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Grapevine.Server
{
    /// <summary>
    /// Externally, a route is a decorated method. Internally, a route is a
    /// regular expression that resolves to a method.
    /// </summary>
    internal class RouteCache
    {
        /// <summary>
        /// One entry in the route cache.
        /// All data is thread-safe
        /// </summary>
        internal class Entry
        {
            #region Constructors

            internal Entry(RESTResource resource, MethodInfo methodInfo, Regex regex, string httpMethod, int priority)
            {
                this.RESTResource = resource;
                this.MethodInfo = methodInfo;
                this.Regex = regex;
                this.HttpMethod = httpMethod;
                this.Priority = priority;
            }

            #endregion

            #region Public Fields

            /// <summary>
            /// Reference to the resource object. Combined with MethodInfo, we can 
            /// Invoke().
            /// </summary>
            internal readonly RESTResource RESTResource;

            /// <summary>
            /// Method to call, from reflection.
            /// </summary>
            internal readonly MethodInfo MethodInfo;

            /// <summary>
            /// HTTP Method attribute (GET/POST/etc)
            /// </summary>
            internal readonly string HttpMethod;

            /// <summary>
            /// PathInfo attribute regular expression, compiled
            /// </summary>
            internal readonly Regex Regex;

            /// <summary>
            /// Priority from the RESTRoute attribute Priority. Higher numbers
            /// are preferred when matching.
            /// </summary>
            internal readonly int Priority;

            #endregion

            #region Public Methods

            internal bool Match(string url, string httpMethod, out Match match)
            {
                match = this.Regex.Match(url);
                return match.Success && httpMethod.Equals(this.HttpMethod);
            }

            #endregion
        }
        
        #region Constructors

        /// <summary>
        /// Construction loads all routes found in the project's DLLs.
        /// </summary>
        internal RouteCache(RESTServer server, string baseUrl)
        {
            _routes = new List<Entry>();

            foreach (RESTResource resource in LoadRestResources(baseUrl))
            {
                resource.Server = server;

                // Create a route cache Entry for each valid RESTRoute attribute found

                foreach (MethodInfo mi in resource.GetType().GetMethods())
                {
                    if (!mi.IsStatic && mi.GetCustomAttributes(true).Any(attr => attr is RESTRoute))
                    {
                        foreach (var attr in mi.GetCustomAttributes(true))
                        {
                            RESTRoute routeAttr = (RESTRoute)attr;
                            var regex = new Regex(routeAttr.PathInfo, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            var httpMethod = routeAttr.Method.ToString();
                            int priority = routeAttr.Priority;
                            _routes.Add( new Entry( resource, mi, regex, httpMethod, priority ) );
                        }
                    }
                }
            }


            // Sort found resources by Priority.
            // For backward compatibility, RESTRoutes with the same Priority
            // should remain in the same order, so this must be a /stable/
            // sort (as in Enumerable.OrderBy).
            _routes = _routes.OrderByDescending( entry => entry.Priority ).ToList<Entry>();
        }

        #endregion

        #region Public Methods

        //
        // todo: I don't like multiple out parameters, but I don't like special "result"
        //       data structures either. Hmm.
        //
        internal bool FindRoute(HttpListenerContext context, out RESTResource resource, out MethodInfo method, out Match match)
        {
           var httpMethod = context.Request.HttpMethod.ToUpper();
           string url = UrlPathPart(context.Request.RawUrl);
           foreach (Entry route in _routes)
           {
              if (route.Match(url, httpMethod, out match))
              {
                 method = route.MethodInfo;
                 resource = route.RESTResource;
                 return true;
              }
           }

           match = null;        // out parameters must be set
           resource = null;
           method = null;
           return false;
        }

        /// <summary>
        /// Finds and invokes a route to match this context. Returns true if found,
        /// false if no route matches.
        /// </summary>
        internal bool FindAndInvokeRoute(HttpListenerContext context)
        {
            RESTResource resource;
            MethodInfo route;
            Match match;
            if (! FindRoute(context, out resource, out route, out match))
                return false;

            if (route.GetParameters().Length == 2)
            {
                route.Invoke(resource, new object[] { context, match });
            }
            else
            {
                route.Invoke(resource, new object[] { context });
            }
            return true;
        }

        #endregion

        #region Private Fields

        readonly List<Entry> _routes = new List<Entry>();

        #endregion

        #region Private Methods

        /// <summary>
        /// Strip the query from the url (everything after the question mark)
        /// and return just the path. This first unescapes the uri to 
        /// make finding the question mark easier.
        /// </summary>
        protected static string UrlPathPart(string url)
        {
            int offset = url.IndexOf("?");
            if (offset == -1)
                return url;

            return url.Substring( 0, offset );
        }

        private List<RESTResource> LoadRestResources( string baseUrl )
        {
            List<RESTResource> resources = new List<RESTResource>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Matches(@"^(microsoft|mscorlib|vshost|system|grapevine)")) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if ((!type.IsAbstract) && (type.IsSubclassOf(typeof(RESTResource))))
                    {
                        if (type.IsSealed)
                        {
                            if (type.GetCustomAttributes(true).Any(attr => attr is RESTScope))
                            {
                                var scopes = type.GetCustomAttributes(typeof(RESTScope), true);
                                foreach (RESTScope scope in scopes)
                                {
                                    if ((scope.BaseUrl.Equals(baseUrl)) || (scope.BaseUrl.Equals("*")))
                                    {
                                        resources.Add(Activator.CreateInstance(type) as RESTResource);
                                    }
                                }
                            }
                            else
                            {
                                resources.Add(Activator.CreateInstance(type) as RESTResource);
                            }
                        }
                        else
                        {
                            throw new ArgumentException(type.Name + " inherits from RESTResource but is not sealed!");
                        }
                    }
                }
            }

            return resources;
        }


        #endregion
    }
}
