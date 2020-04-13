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
                        new FileWriter(Path.Combine("Scrapers", "Olx")),
                        new MemoryWriter()
                    }
                }
            };
            scraper.Start(380, 5);
            scraper.ScrapeOffers();
        }
    }
}