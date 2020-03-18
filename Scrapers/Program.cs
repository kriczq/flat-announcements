using System.IO;
using Scrapers.Logging;
using Scrapers.Writing;

namespace Scrapers
{
    internal static class Program
    {
        private static void Main()
        {
            var scraper = new OlxScraper
            {
                Logger = new CompositeLogger
                {
                    Loggers =
                    {
                        new ConsoleLogger(),
                        new FileLogger()
                    }
                },
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