using System;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await MainAsync();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Executing spider of product web crawler");
            Spider spider = new Spider();

            await spider.Search(ConnectToConfig.FirstLinkToCrawl, ConnectToConfig.ScopeLink);
        }
    }
}
