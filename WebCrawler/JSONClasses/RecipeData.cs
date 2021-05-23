using Newtonsoft.Json;

namespace WebCrawler
{
    public class RecipeData
    {
        [JsonProperty("@context")]
        public string Context { get; private set; }

        [JsonProperty("@type")]
        public string Type { get; private set; }

        [JsonProperty("aggregateRating")]
        public AggregateRating AggregateRating { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("nutrition")]
        public Nutrition Nutrition { get; private set; }

        [JsonProperty("image")]
        public string ImageUrl { get; private set; }

        [JsonProperty("author")]
        public Author Author { get; private set; }

        [JsonProperty("datePublished")]
        public string DatePublished { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("cookTime")]
        public string CookTime { get; private set; }

        [JsonProperty("totalTime")]
        public string TotalTime { get; private set; }

        [JsonProperty("keywords")]
        public string Keywords { get; private set; }

        [JsonProperty("recipeYield")]
        public string RecipeYield { get; private set; }

        [JsonProperty("recipeCategory")]
        public string RecipeCategory { get; set; }

        [JsonProperty("recipeIngredient")]
        public string[] RecipeIngredients { get; private set; }

        [JsonProperty("recipeInstructions")]
        public string RecipeInstructions { get; private set; }

        public string RecipeUrl { get; set; }

        public void DecodeUnicodeChars()
        {
            Name = System.Net.WebUtility.HtmlDecode(Name);
            RecipeCategory = System.Net.WebUtility.HtmlDecode(RecipeCategory);

            for (var index = 0; index < RecipeIngredients.Length; index++)
            {
                RecipeIngredients[index] = System.Net.WebUtility.HtmlDecode(RecipeIngredients[index]);
            }
        }
    }
}
