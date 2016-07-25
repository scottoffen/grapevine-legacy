using System;

namespace Grapevine.Util
{
    /// <summary>
    /// <para>Attribute for ContentType enumeration</para>
    /// <para>Targets: Field</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class ContentTypeMetadata : Attribute
    {
        /// <summary>
        /// String representation of the MIME type and subtype
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// A value that indicates whether this mime type represents a text file
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// A value that indicates whether this mime type represents a binary file
        /// </summary>
        public bool IsBinary
        {
            get { return !IsText; }
            set { IsText = !value; }
        }

        public ContentTypeMetadata()
        {
            Value = "text/plain";
            IsText = true;
        }
    }
}
