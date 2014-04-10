using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class Responder : System.Attribute
    {
        public string Method;
        public string PathInfo;

        public Responder()
        {
            this.Method = "GET";
            this.PathInfo = "/";
        }
    }
}
