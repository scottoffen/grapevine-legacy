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
            if (info.Length <= 0) return null;

            var attrs = info[0].GetCustomAttributes(typeof(ContentTypeMetadata), false);
            return attrs.Length > 0 ? attrs[0] as ContentTypeMetadata : null;
        }

        /// <summary>
        /// Returns the mime type and subtype string from the Value property of the ContentTypeMetadata
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>string</returns>
        public static string ToValue(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return metadata != null ? metadata.Value : ct.ToString();
        }

        /// <summary>
        /// Gets a value that indicates whether this mime type represents a text file
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>bool</returns>
        public static bool IsText(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return metadata?.IsText ?? true;
        }

        /// <summary>
        /// Gets a value that indicates whether this mime type represents a binary file
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>bool</returns>
        public static bool IsBinary(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return metadata?.IsBinary ?? false;
        }
    }
}
