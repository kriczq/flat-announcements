using System.Collections.Generic;
using HtmlAgilityPack;
using Scrapers.Model;

namespace Scrapers
{
    public interface IAnnouncementScraper
    {
        /// <summary>
        /// Start scraping
        /// </summary>
        void Start();
        
        /// <summary>
        /// Get set of offers from html contents of a page.
        /// </summary>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Set of offers</returns>
        ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        /// <summary>
        /// Get next page url from html contents of a page.
        /// Return null when there is no next page.
        /// </summary>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Next page url or null</returns>
        string GetNextPageUrl(HtmlNode html);

        /// <summary>
        /// Scrape offers from links got earlier
        /// </summary>
        void ScrapeOffers();
    }
}