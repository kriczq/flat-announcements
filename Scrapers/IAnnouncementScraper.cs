using System.Collections.Generic;
using IronWebScraper;

namespace Scrapers
{
    public interface IAnnouncementScraper
    {
        /// <summary>
        /// Get last page from response
        /// </summary>
        /// <param name="response">HTTP Response</param>
        /// <returns>Last page with offers</returns>
        public int GetLastPage(Response response);

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        /// <summary>
        /// Get list of offers urls on current page
        /// </summary>
        /// <param name="response">HTTP Response</param>
        /// <returns>Set of links to offers</returns>
        public ISet<string> GetOffers(Response response);
        
        /// <summary>
        /// Get url for specified page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>Specified page URL</returns>
        public string GetPageUrl(int page);

        /// <summary>
        /// Scrape URLs to file
        /// </summary>
        public void ScrapeUrls();
    }
}