using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RestResource : Attribute
    {
        public string PathInfo { get; set; }

        public RestResource(string PathInfo)
        {
            this.PathInfo = PathInfo;

            if (object.ReferenceEquals(PathInfo, null))
            {
                throw new ArgumentException("Classes with the RestResource attribute must provide a PathInfo RegEx string.");
            }
            else if (!PathInfo.StartsWith("^/"))
            {
                throw new ArgumentException("The RestResource attribute PathInfo RegEx string must begin with \"^/\".");
            }
        }
    }
}
