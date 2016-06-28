using System;

namespace Grapevine.Server
{
    /// <summary>
    /// <para>Method attribute for defining a RESTRoute</para>
    /// <para>Targets: Method, AllowMultipe: true</para>
    /// <para>&#160;</para>
    /// <para>A method with the RESTRoute attribute can have traffic routed to it by a RESTServer if the request matches the assigned Method and PathInfo properties.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RESTRoute : Attribute
    {
        /// <summary>
        /// The HttpMethod this route will respond to; defaults to HttpMethod.GET
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// A regular expression string to match against the value of HttpListenerContext.Request.RawUrl; defaults to @"^.*$"
        /// </summary>
        public string PathInfo { get; set; }

        public RESTRoute()
        {
            this.Method = HttpMethod.GET;
            this.PathInfo = @"^.*$";
        }
    }

    /// <summary>
    /// <para>Class attribute for defining a RESTScope</para>
    /// <para>Targets: Class, AllowMultipe: true</para>
    /// <para>&#160;</para>
    /// <para>RESTScope limits the availability of the RESTRoutes defined in a RESTResourse to RESTServers whose BaseUrl property matches the BaseUrl property of the RESTScope.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RESTScope : Attribute
    {
        /// <summary>
        /// The BaseUrl this RESTResource will be limited to.
        /// </summary>
        public string BaseUrl { get; set; }

        public RESTScope()
        {
            this.BaseUrl = "*";
        }
    }
}
