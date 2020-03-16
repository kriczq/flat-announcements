namespace Scrapers
{
    internal static class Program
    {
        private static void Main()
        {
            var scraper = new OlxScraper
            {
            // Writer = new FileWriter(Path.Combine("Scrapes", "Olx"))
            };
            scraper.Start();
            scraper.ScrapeOffers();
        }
    }
}