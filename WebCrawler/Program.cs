using System;
using System.Threading.Tasks;
using WebCrawler.Enums;

namespace WebCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Executing spider of recipe web crawler");
            Spider spider = new Spider();
            
            //Check for updations when reached max links visited
            while(true)
            {
                //Start web crawler on first spider page to crawl/scrape
                await spider.Search(ConnectToConfig.FirstLinkToCrawl, ConnectToConfig.ScopeLink);
            }
        }
    }
}
