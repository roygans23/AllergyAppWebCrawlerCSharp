using Newtonsoft.Json;

namespace WebCrawler
{
    public class Author
    {
        [JsonProperty("@type")]
        public string Type { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}
