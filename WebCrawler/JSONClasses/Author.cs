using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
