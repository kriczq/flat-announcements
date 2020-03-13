using IronWebScraper;

namespace Scrapers
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var scraper = new OlxScraper(490, WebScraper.LogLevel.All);
            scraper.Start();
        }
    }
}