﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler
{
    public static class HtmlContentLoader
    {
        public static async Task<(bool IsLoaded, string HtmlContent)> TryGetHtmlContentAsync(string url)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.ToString().Contains("text/html") &&
                        response.Content.Headers.ContentType.CharSet.Contains("UTF-8")) // status code == 200 && content type of response contains text/html
                    {
                        string htmlContent = await httpClient.GetStringAsync(url);

                        return (true, htmlContent);
                    }
                    Console.WriteLine($"{url} is not in correct format - text/html & UTF-8");
                    return (false, null);
                }
            }
            catch (IOException ioe)
            {
                // We were not successful in our HTTP request
                Console.WriteLine(ioe.Message);
                return (false, null);
            }
        }
    }
}
