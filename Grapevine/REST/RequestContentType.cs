using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine.REST
{
    public enum RequestContentType
    {
        [RequestContentTypeValue("application/json")]
        JSON,

        [RequestContentTypeValue("application/xml")]
        XML,

        [RequestContentTypeValue("text/plain")]
        TEXT,

        [RequestContentTypeValue("multipart/form-data")]
        DEFAULT
    };

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RequestContentTypeValue : Attribute
    {
        public RequestContentTypeValue(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
