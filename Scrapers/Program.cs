using System.IO;
using Scrapers.Writing;

namespace Scrapers
{
    internal static class Program
    {
        private static void Main()
        {
            var scraper = new OlxScraper
            {
                Writer = new CompositeWriter
                {
                    Writers =
                    {
                        new ConsoleWriter(),
                        new FileWriter(Path.Combine("Scrapers", "Olx"))
                    }
                }
            };
            scraper.Start();
            scraper.ScrapeOffers();
        }
    }
}