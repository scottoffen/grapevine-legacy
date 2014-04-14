using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Grapevine
{
    public class ContentTypes
    {
        private static string path = Path.Combine(Directory.GetCurrentDirectory(), "content-types.json");
        private static string @default = "DEFAULT";
        private static Dictionary<string, ContentType> types;

        public static void LoadContentTypes()
        {
            if (types == null)
            {
                types = new Dictionary<string, ContentType>();
                types.Add("DEFAULT", new ContentType(){ Extension = "DEFAULT", MIMEType = "application/octet-stream", IsText = false});

                if (!File.Exists(path))
                {
                    try
                    {
                        var defaults = @"{""types"":[{""Extension"":""TXT"",""MIMEType"":""text/plain"",""IsText"":true},{""Extension"":""HTML"",""MIMEType"":""text/html"",""IsText"":true},{""Extension"":""CSS"",""MIMEType"":""text/css"",""IsText"":true},{""Extension"":""JS"",""MIMEType"":""application/javascript"",""IsText"":true},{""Extension"":""PNG"",""MIMEType"":""image/png"",""IsText"":false},{""Extension"":""JPG"",""MIMEType"":""image/jpg"",""IsText"":false},{""Extension"":""GIF"",""MIMEType"":""image/gif"",""IsText"":false},{""Extension"":""ICO"",""MIMEType"":""image/vnd.microsoft.icon"",""IsText"":false}]}";
                        System.IO.File.WriteAllText(path, defaults);
                    }
                    catch { return; }
                }

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
