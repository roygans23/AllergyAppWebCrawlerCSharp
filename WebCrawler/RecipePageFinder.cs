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
    public class RecipePageFinder
    {
        private Document _htmlDocument;
        private string _productPageUrl;

        public RecipePageFinder(string productDetailUrl)
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
                var recipeCategory = GetRecipeAscendantCategory();

                if (recipeData == null) return false;

                if (recipeCategory != null) recipeData.RecipeCategory = recipeCategory;

                recipeData.RecipeUrl = _productPageUrl;

                Console.WriteLine($"Found Recipe Name: {recipeData.Name}");
                Console.WriteLine($"Found Recipe URL: {recipeData.RecipeUrl}");
                Console.WriteLine(Environment.NewLine);

                if (await ConnectToDB.DoesRecipeAlreadyExistAsync(recipeData.Name))
                {
                    await ConnectToDB.UpdateProductToDbAsync(recipeData);
                }
                else
                {
                    await ConnectToDB.InsertProductToDbAsync(recipeData);
                }
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
            Regex regexPattern = new Regex(ConnectToConfig.RecipeJsonObjectRegex, RegexOptions.Singleline);
            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());

            if (regexMatch.Success)
            {
                var foundValue = regexMatch.Value;
                var endPosition = foundValue.Length - ConnectToConfig.EndIndexRegex;
                var recipeDataRawObjectString = foundValue.Substring(ConnectToConfig.StartIndexRegex, endPosition - ConnectToConfig.StartIndexRegex).Trim();

                var recipeDataObject = JsonConvert.DeserializeObject<RecipeData>(recipeDataRawObjectString);


                return recipeDataObject;
            }

            return null;

        }

        private string GetRecipeAscendantCategory()
        {
            Regex regexPattern = new Regex("<ol class=\"breadcrumb\">.*?קטגוריות.*?<li>.*?</li>", RegexOptions.Singleline);
            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());

            if (regexMatch.Success)
            {
                var foundValue = regexMatch.Value;
                var startPositionPrefix = foundValue.LastIndexOf("/\">");
                var startPosition = startPositionPrefix + 3;
                var endPosition = foundValue.LastIndexOf("</a>");

                var recipeAscendantCategory = foundValue.Substring(startPosition, endPosition - startPosition).Trim();

                return recipeAscendantCategory;
            }

            return null;

        }

    }
}
