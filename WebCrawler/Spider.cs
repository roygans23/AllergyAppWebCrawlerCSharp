using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Spider
    {
        private HashSet<string> _pagesVisited;
        private List<string> _pagesToVisit;

        public Spider()
        {
            _pagesVisited = new HashSet<string>();
            _pagesToVisit = new List<string>();
        }

        /**
         * Our main launching point for the Spider's functionality. Internally it
         * creates spider legs that make an HTTP request and parse the response (the web
         * page).
         * 
         * @param url        - The starting point of the spider
         * @param searchWord - The word or string that you are searching for
         */
        public async Task Search(string url, string scope)
        {
            while (_pagesVisited.Count < ConnectToConfig.MaxPagesToSearch)
            {
                string currentUrl;
                SpiderLeg leg = new SpiderLeg();
                if (!_pagesToVisit.Any())
                {
                    currentUrl = url;
                    _pagesVisited.Add(url);
                }
                else
                {
                    currentUrl = NextUrl();
                }
                await leg.CrawlAsync(currentUrl, scope); // find url links in current url & split to ordinary scope links vs product details links
                await leg.CrawlProductDetailsPagesAsync(); // crawl product details links of current link

                _pagesToVisit.AddRange(leg.PageLinks); // add links of current url page to pages to visit
            }
            // INDICATOR Console.WriteLine("\n**Done** Visited " + pagesVisited.size()
            // + " web page(s)");
        }

        /**
         * Returns the next URL to visit (in the order that they were found). We also do
         * a check to make sure this method doesn't return a URL that has already been
         * visited.
         * 
         * @return
         */
        private string NextUrl()
        {
            string nextUrl;
            do
            {
                nextUrl = _pagesToVisit.First();
                _pagesToVisit.RemoveAt(0);
            } while (_pagesVisited.Contains(nextUrl));
            _pagesVisited.Add(nextUrl);
            return nextUrl;
        }
    }
}
