using Newtonsoft.Json;

namespace WebCrawler
{
    public class Nutrition
    {
        [JsonProperty("@type")]
        public string Type { get; private set; }

        [JsonProperty("calories")]
        public string Calories { get; private set; }

        [JsonProperty("carbohydrateContent")]
        public string CarbohydrateContent { get; private set; }

        [JsonProperty("fatContent")]
        public string FatContent { get; private set; }

        [JsonProperty("sodiumContent")]
        public string SodiumContent { get; private set; }

        [JsonProperty("proteinContent")]
        public string ProteinContent { get; private set; }
    }
}
