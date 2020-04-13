using System.Collections.Generic;
using HtmlAgilityPack;
using Scrapers.Model;

namespace Scrapers
{
    public interface IAnnouncementScraper
    {
        /// <summary>
        /// Start scraping process
        /// <param name="startPage">Page from which scraping will start</param>
        /// <param name="stopAfter">
        /// After how many pages scrapping should end. If provided number is 0 process will not stop until last
        /// page. If provided number bigger than actual number of pages process will stop on the last page.
        /// </param>
        /// </summary>
        void Start(int startPage = 1, int stopAfter = 0);
        
        /// <summary>
        /// Get set of offers from html contents of a page.
        /// </summary>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Set of offers</returns>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        /// <summary>
        /// Get next page url from html contents of a page.
        /// Return null when there is no next page.
        /// </summary>
        /// <param name="html">Page HTML contents</param>
        /// <returns>Next page url or null</returns>
        string GetNextPageUrl(HtmlNode html);

        /// <summary>
        /// Get url for specified page
        /// </summary>
        /// <returns>Page url</returns>
        string GetPageUrl(int page);

        /// <summary>
        /// Scrape offers from links acquired earlier with <see cref="IAnnouncementScraper.Start" /> method.
        /// </summary>
        void ScrapeOffers();
    }
}