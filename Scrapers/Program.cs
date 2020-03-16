using System;
using System.IO;
using System.Text;
using Scrapers.Writing;
using ScrapySharp.Network;

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
            // scraper.Start();
            // scraper.ScrapeOffers();

            const string url = "https://www.olx.pl/oferta/mieszkanie-w-scislym-centrum-pierwsze-pietro-warminskiego-14-CID3-IDDO3BM.html";
            var browser = new ScrapingBrowser()
            {
                AutoDetectCharsetEncoding = false,
                Encoding = Encoding.UTF8
            };

            var offer = scraper.Parser.ParseOffer(browser.NavigateToPage(new Uri(url)).Html);
            scraper.Writer.SaveOne(offer);
        }
    }
}