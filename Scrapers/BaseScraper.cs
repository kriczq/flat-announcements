using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Scrapers.Logging;
using Scrapers.Model;
using Scrapers.Writing;
using ScrapySharp.Network;

namespace Scrapers
{
    // TODO(magiczne): List with already visited urls and skipping them if so.
    // E.g. look at olx
    public abstract class BaseScraper : IAnnouncementScraper
    {
        /// <summary>
        /// Logger instance
        /// </summary>
        public ILogger Logger { get; set; } = new ConsoleLogger();

        /// <summary>
        /// Data writer instance
        /// </summary>
        public IDataWriter Writer { get; set; } = new ConsoleWriter();
        
        /// <summary>
        /// URL to start from
        /// </summary>
        protected string HomeUrl;
        
        /// <summary>
        /// Scraping browser instance
        /// </summary>
        private ScrapingBrowser _browser = new ScrapingBrowser
        {
            AutoDetectCharsetEncoding = false,
            Encoding = Encoding.UTF8
        };

        /// <summary>
        /// Unique offers
        /// </summary>
        private readonly ISet<BaseAnnouncementInfo> _offers = new HashSet<BaseAnnouncementInfo>();

        private void Request(string url)
        {
            var page = _browser.NavigateToPage(new Uri(url));
            Parse(page.Html);
        }

        private void Parse(HtmlNode html)
        {
            _offers.UnionWith(GetOffers(html));

            var nextPage = GetNextPageUrl(html);
            if (nextPage != null)
                Request(nextPage);
            else
                Writer.SaveUrls(_offers);
        }

        #region IAnnouncementScraper
        
        /// <inheritdoc cref="IAnnouncementScraper.Start" />
        public void Start()
        {
            if (HomeUrl == "")
                throw new ArgumentException("HomeUrl must be provided");

            Request(HomeUrl);
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public abstract ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        /// <inheritdoc cref="IAnnouncementScraper.GetNextPageUrl" />
        public abstract string GetNextPageUrl(HtmlNode html);

        #endregion
    }
}