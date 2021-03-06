using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Scrapers.Model;
using Scrapers.Parsing;
using ScrapySharp.Extensions;

namespace Scrapers
{
    public class OlxScraper : BaseScraper
    {
        public OlxScraper()
        {
            HomeUrl = "https://www.olx.pl/nieruchomosci/mieszkania/";
            Parser = new CompositeParser
            {
                Parsers =
                {
                    { "https://www.olx.pl/", new OlxParser() },
                    { "https://www.otodom.pl/", new OtodomParser() }
                }
            };
        }

        #region IAnnouncementScraper

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public override ISet<BaseAnnouncementInfo> GetOffers(HtmlNode webPage)
        {
            var offers = new HashSet<BaseAnnouncementInfo>();

            var table = webPage.CssSelect("#offers_table > tbody").First();

            foreach (var announcement in table.CssSelect("tr.wrap"))
            {
                var container = announcement.CssSelect("td").First();
                var titleCell = announcement.CssSelect(".title-cell").First();
                var link = titleCell.CssSelect("h3 > a").First();
                var breadcrumb = titleCell.CssSelect(".breadcrumb").First();
                var href = link.Attributes.AttributesWithName("href").First().Value;
                
                offers.Add(new BaseAnnouncementInfo
                {
                    IsAd = container.Attributes.AttributesWithName("class").First().Value.Contains("promoted"),
                    Url = SanitizeUrl(href),
                    Type = BreadcrumbToAnnouncementType(breadcrumb.InnerText)
                });
            }
            
            Logger.Info($"Found {offers.Count} announcements on page.");

            return offers;
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetNextPageUrl" />
        public override string GetNextPageUrl(HtmlNode webPage)
        {
            try
            {
                var nextLink = webPage.CssSelect("[data-cy=page-link-next]").Last();

                if (!nextLink.Attributes.Contains("href"))
                {
                    Logger.Decision("Next link not found. Scrapping complete...");
                    return null;
                }

                Logger.Decision($"Found next link: {nextLink.Attributes["href"].Value}. Processing...");
                return nextLink.Attributes.AttributesWithName("href").First().Value;
            }
            catch (InvalidOperationException)
            {
                Logger.Error("Next link not found");
                return null;
            }
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetPageUrl" />
        public override string GetPageUrl(int page) => $"{HomeUrl}?page={page}";

        #endregion

        #region Utility methods

        /// <summary>
        /// Parse breadcrumb text and assign proper announcement type
        /// </summary>
        /// <param name="breadcrumb">Breadcrumb innerText</param>
        /// <returns>Announcement type</returns>
        private static AnnouncementType BreadcrumbToAnnouncementType(string breadcrumb)
        {
            if (breadcrumb.Contains("Wynajem"))
                return AnnouncementType.Rent;

            if (breadcrumb.Contains("Sprzedaż"))
                return AnnouncementType.Sale;

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (breadcrumb.Contains("Zamiana"))
                return AnnouncementType.Swap;

            return AnnouncementType.Unknown;
        }
        
        /// <summary>
        /// Remove hash and unnecessary parameters from url
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Cleaned url</returns>
        private static string SanitizeUrl(string url)
        {
            if (url.Contains("#"))
                url = url.Substring(0, url.IndexOf("#", StringComparison.Ordinal));

            if (url.Contains("?"))
                url = url.Substring(0, url.IndexOf("?", StringComparison.Ordinal));

            return url;
        }

        #endregion
    }
}