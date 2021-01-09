using Supremes;
using Supremes.Nodes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class SpiderLeg
    {
        private List<string> _productPageUrls;

        public SpiderLeg()
        {
            PageLinks = new List<string>();
            _productPageUrls = new List<string>();
        }

        public List<string> PageLinks { get; private set; }

        /**
         * This performs all the work. It makes an HTTP request, checks the response,
         * and then gathers up all the links on the page. Perform a searchForWord after
         * the successful crawl
         * 
         * @param url   - The URL to visit
         * 
         * @param scope - The scope f the crawling activity
         * 
         * @return whether or not the crawl was successful
         */
        public async Task<bool> CrawlAsync(string url, string scope)
        {
            try
            {
                var response = await HtmlContentLoader.TryGetHtmlContentAsync(url);
                if (!response.Item1) return false;

                var htmlDocument = Dcsoup.Parse(response.Item2);

                Elements linksOnPage = htmlDocument.Select("a[href]");
                // INDICATOR Console.WriteLine("Found (" + linksOnPage.size() + ") links");
                foreach (Element link in linksOnPage)
                {
                    string dataProductCodeAttribute = link.Attr(ConnectToConfig.ProductLinkAttribute);
                    string absUrl = link.AbsUrl("href");

                    if (dataProductCodeAttribute.Any())
                    {
                        string productDetailsUrl = GetProductDetailUrl(dataProductCodeAttribute);
                        if (!_productPageUrls.Contains(productDetailsUrl))
                        {
                            _productPageUrls.Add(productDetailsUrl);
                        }
                    }

                    if (absUrl.Any() && (absUrl != "") && absUrl.StartsWith(scope))
                    {
                        PageLinks.Add(absUrl);
                    }
                }
                return true;
            }
            catch (IOException ioe)
            {
                // We were not successful in our HTTP request
                return false;
            }
        }

        public async Task CrawlProductDetailsPagesAsync()
        {
            foreach (string productDetailUrl in _productPageUrls)
            {
                var productPageFinder = new ProductPageFinder(productDetailUrl);
                await productPageFinder.CrawlProductPageAsync();
            }
        }

        private string GetProductDetailUrl(string productId)
        {
            return $"{ConnectToConfig.ScopeLink}/p/{productId}{ConnectToConfig.ProductPagePostfix}";
        }

      
    }
}
