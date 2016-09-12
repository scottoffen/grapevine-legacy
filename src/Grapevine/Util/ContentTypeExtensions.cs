using System;
using System.Linq;

namespace Grapevine.Util
{
    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Returns the ContentTypeMetadata for the given content type
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>ContentTypeMetadata</returns>
        private static ContentTypeMetadata GetMetadata(ContentType ct)
        {
            var info = ct.GetType().GetMember(ct.ToString());
            var attributes = info[0].GetCustomAttributes(typeof(ContentTypeMetadata), false);
            return attributes.Length > 0 ? attributes[0] as ContentTypeMetadata : new ContentTypeMetadata();
        }

        /// <summary>
        /// Returns the mime type and subtype string from the Value property of the ContentTypeMetadata
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>string</returns>
        public static string ToValue(this ContentType ct)
        {
            return GetMetadata(ct).Value;
        }

        /// <summary>
        /// Gets a value that indicates whether this mime type represents a text file
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>bool</returns>
        public static bool IsText(this ContentType ct)
        {
            return GetMetadata(ct).IsText;
        }

        /// <summary>
        /// Gets a value that indicates whether this mime type represents a binary file
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>bool</returns>
        public static bool IsBinary(this ContentType ct)
        {
            return GetMetadata(ct).IsBinary;
        }

        public static ContentType FromString(this ContentType ct, string contentType)
        {
            return string.IsNullOrWhiteSpace(contentType)
                ? ContentType.DEFAULT
                : Enum.GetValues(typeof(ContentType)).Cast<ContentType>().FirstOrDefault(t => t.ToValue().Equals(contentType));
        }
    }
}
