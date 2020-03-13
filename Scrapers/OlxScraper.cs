using System;
using System.Collections.Generic;
using System.Linq;
using IronWebScraper;
using Scrapers.Model;

namespace Scrapers
{
    public sealed class OlxScraper : BaseScraper
    {
        public OlxScraper(int startPage = 1, LogLevel logLevel = LogLevel.Critical)
            : base(startPage, logLevel)
        {
            HomeUrl = "https://www.olx.pl/nieruchomosci/mieszkania/";
        }
        
        /// <inheritdoc cref="IAnnouncementScraper.GetLastPage" />
        public override int GetLastPage(Response response)
        {
            HtmlNode lastPageNode;
            int page;

            try
            {
                var paginator = response.Css(".pager").First();
                lastPageNode = paginator.Css(".block.fleft + .item.fleft > a > span").First();
            }
            catch (InvalidOperationException)
            {
                Log("Paginator node does not exist!", LogLevel.Critical);
                return 1;
            }

            try
            {
                page = int.Parse(lastPageNode.InnerText);
            }
            catch (FormatException)
            {
                Log("Last page text should contain numbers!", LogLevel.Critical);
                return 1;
            }

            return page;
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public override ISet<BaseEntryInfo> GetOffers(Response response)
        {
            var offers = new HashSet<BaseEntryInfo>();
            
            foreach (var announcement in response.Css("#offers_table > tbody > tr.wrap"))
            {
                var container = announcement.Css("> td").First();

                var titleCell = announcement.Css(".title-cell").First();
                var link = titleCell.Css("h3 > a").First();
                var breadcrumb = titleCell.Css(".breadcrumb").First();
                
                offers.Add(new BaseEntryInfo
                {
                    IsAd = container.Attributes["class"].Contains("promoted"),
                    Url = link.Attributes["href"],
                    Type = BreadcrumbToAnnouncementType(breadcrumb.InnerText)
                });
            }

            return offers;
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetPageUrl" />
        public override string GetPageUrl(int page) => $"{HomeUrl}?page={page}";

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

        #endregion
    }
}