using System;

namespace Grapevine.Server.Attributes
{
    /// <summary>
    /// <para>Class attribute for defining a GetRestResource</para>
    /// <para>Targets: Class</para>
    /// <para>&#160;</para>
    /// <para>A class with the GetRestResource attribute can be scanned for RestRoute attributed methods</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RestResource : Attribute
    {
        /// <summary>
        /// This value will be prepended to the PathInfo value on all RestRoutes in the class, defaults to an empty string
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// When set, will limit the accessibility of RestRoutes to Servers with the same Scope; defaults to no scope (an empty string)
        /// </summary>
        public string Scope { get; set; }

        public RestResource()
        {
            BasePath = string.Empty;
            Scope = string.Empty;
        }
    }
}