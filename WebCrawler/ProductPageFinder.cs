using Newtonsoft.Json;
using Supremes;
using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class ProductPageFinder
    {
        private Document _htmlDocument;
        private string _productPageUrl;

        public ProductPageFinder(string productDetailUrl)
        {
            _productPageUrl = productDetailUrl;
        }

        public async Task CrawlProductPageAsync()
        {
            var response = await HtmlContentLoader.TryGetHtmlContentAsync(_productPageUrl);
            if (!response.IsLoaded) return;
            _htmlDocument = Dcsoup.Parse(response.HtmlContent);
            await TryInsertRecipeToDB(_htmlDocument.Html);

        }

        /**
         * Performs a search on the body of on the HTML document that is retrieved. This
         * method should only be called after a successful crawl.
         * 
         * @param searchWord - The word or string to look for
         * @return whether or not the word was found
         */

        public async Task<bool> TryInsertRecipeToDB(string htmlContent)
        {
            // Defensive coding. This method should only be used after a successful crawl.
            if (_htmlDocument == null)
            {
                Console.WriteLine("ERROR! Call crawl() before performing analysis on the document");
                return false;
            }

            try
            {
                var recipeData = GetRecipeData();

                if (recipeData == null) return false;
                recipeData.RecipeUrl = _productPageUrl;

                Console.WriteLine($"Found Recipe Name: {recipeData.Name}");
                Console.WriteLine($"Found Recipe URL: {recipeData.RecipeUrl}");
                //Console.WriteLine($"Found Product Allergies: {string.Join(",", foundAllergyString)}");
                Console.WriteLine(Environment.NewLine);

                // INDICATOR Console.WriteLine("ONLY ALLERGY: " + onlyAllergySection);
                // Console.WriteLine("Allergy Information: " + foundAllergyString);

                await ConnectToDB.InsertProductToDbAsync(recipeData);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }

            return true;
        }

        private RecipeData GetRecipeData()
        {
            //var formatValueRegexString = FormatValueRegexString("id=\"recipe - schema\"", 20, 5, IndexRegexType.NoIndex);

            Regex regexPattern = new Regex("id=\"recipe-schema\">.*?</script>", RegexOptions.Singleline);
            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());

            if (regexMatch.Success)
            {
                var foundValue = regexMatch.Value;
                //startPosition += foundValue.IndexOf(indexSign);
                //var length = endPosition - startPosition;

                var endPosition = foundValue.Length - 9;

                var recipeDataRawObjectString = foundValue.Substring(20, endPosition - 20).Trim();

                var recipeDataObject = JsonConvert.DeserializeObject<RecipeData>(recipeDataRawObjectString);


                return recipeDataObject;
            }

            return null;

        }

    }
}
