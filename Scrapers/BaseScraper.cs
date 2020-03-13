using System;
using System.Collections.Generic;
using IronWebScraper;
using Scrapers.Model;

namespace Scrapers
{
    public abstract class BaseScraper : WebScraper, IAnnouncementScraper
    {
        /// <summary>
        /// URL to start from
        /// </summary>
        protected string HomeUrl;
        
        /// <summary>
        /// Currently processed page number
        /// </summary>
        protected int CurrentPage;
        
        /// <summary>
        /// Last page number
        /// </summary>
        private int _lastPage = 1;
        
        /// <summary>
        /// Unique offers
        /// </summary>
        private readonly ISet<BaseEntryInfo> _offers = new HashSet<BaseEntryInfo>();
        
        protected BaseScraper(int startPage = 1, LogLevel logLevel = LogLevel.Critical)
        {
            CurrentPage = startPage;
            LoggingLevel = logLevel;
            ObeyRobotsDotTxt = false;
        }

        public override void Init()
        {
            if (HomeUrl == "")
                throw new ArgumentException("HomeUrl must be provided");
            
            Request(GetPageUrl(CurrentPage), Parse);
        }
        
        public override void Parse(Response response)
        {
            if (CurrentPage == 1)
                _lastPage = GetLastPage(response);

            _offers.UnionWith(GetOffers(response));

            if (++CurrentPage <= _lastPage)
                Request(GetPageUrl(CurrentPage), Parse);
            else
                ScrapeUrls();
        }
        
        #region IAnnouncementScraper

        /// <inheritdoc cref="IAnnouncementScraper.GetLastPage" />
        public abstract int GetLastPage(Response response);

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public abstract ISet<BaseEntryInfo> GetOffers(Response response);

        /// <inheritdoc cref="IAnnouncementScraper.GetPageUrl" />
        public abstract string GetPageUrl(int page);

        /// <inheritdoc cref="IAnnouncementScraper.ScrapeUrls" />
        public void ScrapeUrls() => Scrape(_offers);

        #endregion
    }
}