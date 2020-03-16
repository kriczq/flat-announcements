using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Scrapers.Extensions;
using Scrapers.Logging;
using Scrapers.Model;
using ScrapySharp.Extensions;

namespace Scrapers
{
    public class OlxScraper : BaseScraper
    {
        public OlxScraper()
        {
            HomeUrl = "https://www.olx.pl/nieruchomosci/mieszkania/?page=498";
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
                
                offers.Add(new BaseAnnouncementInfo
                {
                    IsAd = container.Attributes.AttributesWithName("class").First().Value.Contains("promoted"),
                    Url = SanitizeUrl(link.Attributes.AttributesWithName("href").First().Value),
                    Type = BreadcrumbToAnnouncementType(breadcrumb.InnerText)
                });
            }
            
            Logger.Log(LogLevel.Info, $"Found {offers.Count} announcements on page.");

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
                    Logger.Log(LogLevel.Decision, "Next link not found. Scrapping complete...");
                    return null;
                }

                Logger.Log(LogLevel.Decision, $"Found next link: {nextLink.Attributes["href"].Value}. Processing...");
                return nextLink.Attributes.AttributesWithName("href").First().Value;
            }
            catch (InvalidOperationException)
            {
                Logger.Log(LogLevel.Error, "Next link not found");
                return null;
            }
        }

        /// <inheritdoc cref="IAnnouncementScraper.ParseOffer" />
        public override Announcement ParseOffer(HtmlNode html)
        {
            var id = html.CssSelect(".offer-titlebox__details > em > small").First().InnerText.Trim();
            var title = html.CssSelect(".offer-titlebox > h1").First().InnerText.Trim();
            var locationNode = html.CssSelect(".offer-titlebox__details > .show-map-link > strong").First();
            var price = html.CssSelect(".price-label > strong").First().InnerText.Trim();
            var details = ParseDetails(
                html.CssSelect(".descriptioncontent > table.details .item"));

            var livingSpace = details["Powierzchnia"]
                .RemoveWhitespace();
            livingSpace = livingSpace.Substring(0, livingSpace.Length - 2);

            return new Announcement
            {
                Id = id.Split(":").Last().Trim(),
                Title = title,
                
                Voivodeship = locationNode.InnerText.Trim(),
                City = locationNode.InnerText.Trim(),
                
                BasePrice = int.Parse(price.Substring(0, price.Length - 3).RemoveWhitespace()),
                
                Rent = details.ContainsKey("Czynsz (dodatkowo") ? 1 : 0,
                
                IsFromDeveloper = details["Oferta od"].Contains("Deweloper"),
                IncludesFurniture = details["Umeblowane"] == "Tak",
                LivingSpace = float.Parse(livingSpace),
                BuildingType = details["Rodzaj zabudowy"],
                Rooms = details["Liczba pokoi"],
                Floor = details["Poziom"],
                
                CreatedAt = ""
            };
        }

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

            if (breadcrumb.Contains("Zamiana"))
                return AnnouncementType.Swap;

            return AnnouncementType.Unknown;
        }

        private Dictionary<string, string> ParseDetails(IEnumerable<HtmlNode> detailsNodes)
        {
            var details = new Dictionary<string, string>();

            foreach (var node in detailsNodes)
            {
                var header = node.CssSelect("th").First().InnerText.Trim();
                var value = node.CssSelect(".value").First().InnerText.Trim();
                details.Add(header, value);
            }

            return details;
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