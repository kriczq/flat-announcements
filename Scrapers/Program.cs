using System;
using System.IO;
using System.Text;
using Scrapers.Logging;
using Scrapers.Parsing;
using Scrapers.Writing;
using ScrapySharp.Network;

namespace Scrapers
{
    internal static class Program
    {
        private static void Main()
        {
            TestScraping(325, 2);
            
            // const string url = "https://www.olx.pl/oferta/sprzedam-mieszkanie-m-6-centrum-miasta-oferta-prywatna-CID3-IDEpt3z.html";
            // TestParsing(url);
        }

        private static void TestScraping(int start, int stopAfter)
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
            scraper.Start(start, stopAfter);
            scraper.ScrapeOffers();
        }

        private static void TestParsing(string url)
        {
            var uri = new Uri(url);
            var browser = new ScrapingBrowser
            {
                AutoDetectCharsetEncoding = false,
                Encoding = Encoding.UTF8,
                IgnoreCookies = true
            };
            var parser = new OlxParser();
            var writer = new ConsoleWriter();
            
            var announcement = parser.ParseOffer(url, browser.NavigateToPage(uri).Html);
            writer.SaveOne(announcement);
        }
    }
}