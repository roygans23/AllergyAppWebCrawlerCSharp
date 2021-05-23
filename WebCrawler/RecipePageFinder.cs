using Newtonsoft.Json;
using Supremes;
using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.Enums;

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
            var (IsLoaded, HtmlContent) = await HtmlContentLoader.TryGetHtmlContentAsync(_productPageUrl);
            if (!IsLoaded) return;
            _htmlDocument = Dcsoup.Parse(HtmlContent);

            await TryInsertRecipeToDB(_htmlDocument.Html);
        }

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
                //get all recipe data from html document
                var recipeData = GetRecipeData();

                if (recipeData == null) return false;

                //get recipe ascendant category based on relative naming in gui of page
                var recipeCategory = GetRecipeAscendantCategory();
                if (recipeCategory != null) recipeData.RecipeCategory = recipeCategory;

                //edit page url
                recipeData.RecipeUrl = _productPageUrl;

                //decode unfriendly unicode chars for readability in DB
                recipeData.DecodeUnicodeChars();

                Console.WriteLine($"Found Recipe Name: {recipeData.Name}, Found Recipe URL: {recipeData.RecipeUrl}");

                //DB transcations
                if (await ConnectToDB.ExecuteCountRecipeRowsQueryCommandAsync(recipeData, SqlCommandType.Select))
                {
                    await ConnectToDB.ExecuteNonQueryRecipeCommandAsync(recipeData, SqlCommandType.Update);
                    Console.WriteLine("Updated existing recipe in DB");
                }
                else
                {
                    await ConnectToDB.ExecuteNonQueryRecipeCommandAsync(recipeData, SqlCommandType.Insert);
                    Console.WriteLine("Inserted new recipe in DB");
                }
                Console.WriteLine(Environment.NewLine);
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
                var endPosition = foundValue.Length - ConnectToConfig.EndIndexJsonObjectRegex;
                var recipeDataRawObjectString = foundValue.Substring(ConnectToConfig.StartIndexJsonObjectRegex, endPosition - ConnectToConfig.StartIndexJsonObjectRegex).Trim();

                var recipeDataObject = JsonConvert.DeserializeObject<RecipeData>(recipeDataRawObjectString);

                return recipeDataObject;
            }

            Console.WriteLine($"Not found data for recipe in url {_productPageUrl}!");
            return null;

        }

        private string GetRecipeAscendantCategory()
        {
            Regex regexPattern = new Regex(ConnectToConfig.RecipeMainCategoryRegex, RegexOptions.Singleline);
            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());

            if (regexMatch.Success)
            {
                var foundValue = regexMatch.Value;
                var startPositionPrefix = foundValue.LastIndexOf(ConnectToConfig.StartPositionPrefixMainCategoryRegex);
                var startPosition = startPositionPrefix + ConnectToConfig.StartIndexMainCategoryRegex;
                var endPosition = foundValue.LastIndexOf(ConnectToConfig.EndPositionSuffixMainCategoryRegex);

                var recipeAscendantCategory = foundValue.Substring(startPosition, endPosition - startPosition).Trim();

                return recipeAscendantCategory;
            }
            return null;
        }
    }
}