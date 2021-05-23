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

        public async Task<List<string>> CrawlProductPageAsync(bool crawlMainCategoriesOnly = false)
        {
            var (IsLoaded, HtmlContent) = await HtmlContentLoader.TryGetHtmlContentAsync(_productPageUrl);
            if (!IsLoaded) return null;
            _htmlDocument = Dcsoup.Parse(HtmlContent);

            if (crawlMainCategoriesOnly)
            {
                return GetAllAscendantRecipesCategory();
            }
            else
            {
                await TryInsertRecipeToDB(_htmlDocument.Html);
                return null;
            }

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
                var recipeData = GetRecipeData();
                if (recipeData == null) return false;

                var recipeCategory = GetRecipeAscendantCategory();
                if (recipeCategory != null) recipeData.RecipeCategory = recipeCategory;

                recipeData.RecipeUrl = _productPageUrl;
                recipeData.DecodeUnicodeChars();

                Console.WriteLine($"Found Recipe Name: {recipeData.Name}, Found Recipe URL: {recipeData.RecipeUrl}");


                //DB commands
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
                var endPosition = foundValue.Length - ConnectToConfig.EndIndexRegex;
                var recipeDataRawObjectString = foundValue.Substring(ConnectToConfig.StartIndexRegex, endPosition - ConnectToConfig.StartIndexRegex).Trim();

                var recipeDataObject = JsonConvert.DeserializeObject<RecipeData>(recipeDataRawObjectString);

                return recipeDataObject;
            }

            Console.WriteLine($"Not found data for recipe in url {_productPageUrl}!");
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

        private List<string> GetAllAscendantRecipesCategory()
        {
            Regex regexPattern = new Regex("quadmenu-item-level-0.*?quadmenu-item-level-0", RegexOptions.Singleline);

            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());
            var list = new List<string>();
            if (regexMatch.Success)
            {
                var foundValue = regexMatch.Value;

                Regex mainCategoriesRegex = new Regex("<li.*?quadmenu-item-level-1.*?</li>", RegexOptions.Singleline);
                var mainCategoriesRegexMatchs = mainCategoriesRegex.Matches(foundValue);

                foreach (Match categoryRegexMatch in mainCategoriesRegexMatchs)
                {
                    var foundCategoryMatch = categoryRegexMatch.Value;
                    var startPositionPrefix = foundCategoryMatch.IndexOf("hover t_1000\">");
                    var startPosition = startPositionPrefix + 14;
                    var endPosition = foundCategoryMatch.IndexOf("</span> </span>");

                    var recipeAscendantCategory = foundCategoryMatch.Substring(startPosition, endPosition - startPosition).Trim();

                    list.Add(recipeAscendantCategory);
                }
            }
            return list;
        }

    }
}
