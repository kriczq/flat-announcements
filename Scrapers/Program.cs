namespace Scrapers
{
    internal static class Program
    {
        private static void Main()
        {
            var scraper = new OlxScraper();
            scraper.Start();
        }
    }
}