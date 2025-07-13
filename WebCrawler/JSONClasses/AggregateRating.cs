using Newtonsoft.Json;

namespace WebCrawler
{
    public class AggregateRating
    {
        [JsonProperty("@type")]
        public string Type { get; private set; }

        [JsonProperty("ratingValue")]
        public string RatingValue { get; private set; }

        [JsonProperty("reviewCount")]
        public string ReviewCount { get; private set; }

    }
}
