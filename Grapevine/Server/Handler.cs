using System;
using System.Reflection;

namespace Grapevine.Server
{
    internal class Handler : IComparable<Handler>
    {
        public RESTRoute Route { get; set; }
        public MethodInfo Method { get; set; }

        /// <summary>
        /// Delegate comparision to our Route
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Handler other)
        {
            return Route.CompareTo(other.Route);
        }
    }
}