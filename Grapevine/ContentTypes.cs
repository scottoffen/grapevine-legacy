using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Grapevine
{
    public class ContentTypes
    {
        private static string path = Path.Combine(Directory.GetCurrentDirectory(), "content-types.json");
        private static string @default = "TXT";
        private static Dictionary<string, ContentType> types;

        private static void LoadContentTypes()
        {
            if (types == null)
            {
                types = new Dictionary<string, ContentType>();
                types.Add("TXT", new ContentType(){ Extension = "TXT", MIMEType = "text/plain", IsText = true});

                if (File.Exists(path))
                {
                    JObject json = JObject.Parse(File.ReadAllText(path));
                    JArray data = json["types"].Value<JArray>();

                    foreach (JToken obj in data.Children())
                    {
                        var type = obj.ToObject<ContentType>();
                        types.Add(type.Extension, type);
                    }
                }
            }
        }

        public static ContentType GetContentType(string filename)
        {
            LoadContentTypes();
            var ext = Path.GetExtension(filename).ToUpper().Substring(1);

            if (types.ContainsKey(ext))
            {
                return types[ext];
            }
            else
            {
                return types[@default];
            }
        }
    }

    public class ContentType
    {
        [JsonProperty("MIMEType")]
        public string MIMEType { get; set; }

        [JsonProperty("Extension")]
        public string Extension { get; set; }

        [JsonProperty("IsText")]
        public bool IsText { get; set; }
    }
}
