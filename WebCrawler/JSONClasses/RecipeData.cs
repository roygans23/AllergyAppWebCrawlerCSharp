using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string RecipeCategory { get; private set; }

        [JsonProperty("recipeIngredient")]
        public string[] RecipeIngredient { get; private set; }

        [JsonProperty("recipeInstructions")]
        public string RecipeInstructions { get; private set; }

        public string RecipeUrl { get; set; }

    }
}
