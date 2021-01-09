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
            await TryInsertProductToDB(_htmlDocument.Html);

        }

        /**
         * Performs a search on the body of on the HTML document that is retrieved. This
         * method should only be called after a successful crawl.
         * 
         * @param searchWord - The word or string to look for
         * @return whether or not the word was found
         */

        public async Task<bool> TryInsertProductToDB(string htmlContent)
        {
            // Defensive coding. This method should only be used after a successful crawl.
            if (_htmlDocument == null)
            {
                Console.WriteLine("ERROR! Call crawl() before performing analysis on the document");
                return false;
            }

            try
            {
                var realBarcode = GetBarcodeNumber();
                var productName = GetProductName();
                var foundIngredientsStringWithStrong = GetIngredientsWithAllergies();
                var foundAllergyString = GetAllergies();

                Console.WriteLine($"Found Product Barcode: {realBarcode}");
                Console.WriteLine($"Found Product Name: {productName}");
                Console.WriteLine($"Found Product Ingredients: {string.Join(",", foundIngredientsStringWithStrong)}");
                //Console.WriteLine($"Found Product Allergies: {string.Join(",", foundAllergyString)}");
                Console.WriteLine(Environment.NewLine);

                // INDICATOR Console.WriteLine("ONLY ALLERGY: " + onlyAllergySection);
                // Console.WriteLine("Allergy Information: " + foundAllergyString);

                //await ConnectToDB.InsertProductToDbAsync(Guid.NewGuid().ToString(), realBarcode, productName,
                //    foundIngredientsStringWithStrong, foundAllergyString);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }

            return true;
        }

        private string GetProductName()
        {
            //"id=\"modaltitle\">.*</h3>"
            var formatValueRegexString = FormatValueRegexString(ConnectToConfig.ProductNameRegex, 16, 5, IndexRegexType.NoIndex);

            if (formatValueRegexString.IsFound) return formatValueRegexString.FoundValue;

            return string.Empty;
        }

        private string GetBarcodeNumber()
        {
            var formatValueRegexString = FormatValueRegexString("<div class=\"name\">\n.*מק\"ט:\n.*</div>.*\n.*\n.*[0-9]*\n.*</div>", 3, 6, IndexRegexType.LastIndex, "\">");
            if (formatValueRegexString.IsFound) return formatValueRegexString.FoundValue;

            return string.Empty;
        }

        private string[] GetAllergies()
        {

            var formatValueRegexString = FormatValueRegexString("<strong>אלרגנים:</strong>.*\n.*</div>", 5, 6, IndexRegexType.SpecificIndex, "<div>");

            if (formatValueRegexString.IsFound) return formatValueRegexString.FoundValue.Split(",");

            return null;

            //<span>אלרגנים:</span><div>6809,גלוטן;6821,סויה</div>

            Regex allergyPattern = new Regex("<strong>אלרגנים:</strong>.*\n.*</div>");
            Match allergyMatcher = allergyPattern.Match(_htmlDocument.Html.ToLower());
            string onlyAllergySection = "";
            if (allergyMatcher.Success)
            {
                onlyAllergySection = allergyMatcher.Value;
                onlyAllergySection = onlyAllergySection.Substring(onlyAllergySection.IndexOf("<div>") + 5,
                        onlyAllergySection.Length - 6).Trim();
            }
            else
            {
                Console.WriteLine("NOT FOUND ALLERGY INFO");
            }

            return null;
        }

        private string[] GetIngredientsWithAllergies()
        {

            var formatValueRegexString = FormatValueRegexString("<div class=\"maininfo\">\n.*רכיבים\n.*</div>.*\n.+\n.+\n.+\n.+\n.*</div>", 16, 6, IndexRegexType.SpecificIndex, "componentstext");

            if (formatValueRegexString.Item1) return formatValueRegexString.Item2.Split(",");

            return null;

            //<span>רכיבים:</span><div>קמח חיטה (מכיל גלוטן), שמן צמחי, מלח (2.5%), סוכר, לתת שעורה, חומר תפיחה (סודיום ביקרבונט - סודה לשתיה), מתחלב (לציטין סויה), חומצת מאכל (חומצת לימון), מווסת חומציות (E524)</div>

            Regex ingredientsPattern = new Regex("<div class=\"maininfo\">\n.*רכיבים\n.*</div>.*\n.+\n.+\n.+\n.+\n.*</div>");
            Match ingredientsMatcher = ingredientsPattern.Match(_htmlDocument.Html.ToLower());
            string foundIngredientsString = "";
            if (ingredientsMatcher.Success)
            {
                foundIngredientsString = ingredientsMatcher.Value;
                foundIngredientsString = foundIngredientsString.Substring(foundIngredientsString.IndexOf("componentsText") + 16,
                        foundIngredientsString.Length - 6).Trim();


                Console.WriteLine("Ingredients: " + foundIngredientsString.Replace("<strong>", "").Replace("</strong>", ""));
            }
            else
            {
                Console.WriteLine("NOT FOUND INGREDIENTS");
            }

            return null;
        }

        private (bool IsFound, string FoundValue) FormatValueRegexString(string regexString, int startBuffer, int endBuffer, IndexRegexType indexRegexType, string indexSign = null)
        {
            Regex regexPattern = new Regex(regexString);
            Match regexMatch = regexPattern.Match(_htmlDocument.Html.ToLower());
            var foundValue = "";
            if (regexMatch.Success)
            {
                var index = regexMatch.Index;
                foundValue = regexMatch.Value;

                var startPosition = startBuffer;

                switch (indexRegexType)
                {
                    case IndexRegexType.SpecificIndex:
                        startPosition += foundValue.IndexOf(indexSign);
                        break;
                    case IndexRegexType.LastIndex:
                        startPosition += foundValue.LastIndexOf(indexSign);
                        break;
                }

                var endPosition = foundValue.Length - endBuffer;
                var length = endPosition - startPosition;

                return (true, foundValue = foundValue.Substring(startPosition, length).Trim());
            }

            return (false, foundValue);
        }
    }
}
