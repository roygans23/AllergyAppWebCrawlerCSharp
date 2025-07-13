using System;
using System.Threading.Tasks;
using WebCrawler.Enums;

namespace WebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await MainAsync();
            Console.WriteLine("Web crawler execution completed.");
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Executing spider of product web crawler");
            Spider spider = new Spider();
            
            //Check for updations when reached max links visited
            while(true)
            {
                var productPageFinder = new RecipePageFinder(ConnectToConfig.FirstLinkToCrawl);
                await productPageFinder.CrawlProductPageAsync();

                //Start web crawler
                await spider.Search(ConnectToConfig.FirstLinkToCrawl, ConnectToConfig.ScopeLink);
            }
        }
    }
}
