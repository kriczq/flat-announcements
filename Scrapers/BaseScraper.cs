using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Scrapers.Logging;
using Scrapers.Model;
using Scrapers.Parsing;
using Scrapers.Writing;
using ScrapySharp.Network;

namespace Scrapers
{
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
        /// Announcement types to scrap
        /// </summary>
        public AnnouncementType[] TypesToScrap { get; set; } =
        {
            AnnouncementType.Rent, AnnouncementType.Sale, AnnouncementType.Swap, AnnouncementType.Unknown
        };

        /// <summary>
        /// Parser instance
        /// </summary>
        protected IAnnouncementParser Parser;
        
        /// <summary>
        /// URL to start from
        /// </summary>
        protected string HomeUrl;

        /// <summary>
        /// After how many pages scraper will stop processing data
        /// </summary>
        private int _stopAfter;

        /// <summary>
        /// How many pages were processed already
        /// </summary>
        private int _alreadyParsedCount;

        /// <summary>
        /// Scraping browser instance
        /// </summary>
        private readonly ScrapingBrowser _browser = new ScrapingBrowser
        {
            AutoDetectCharsetEncoding = false,
            Encoding = Encoding.UTF8,
            IgnoreCookies = true
        };

        /// <summary>
        /// Already visited URLs
        /// </summary>
        private readonly ISet<string> _alreadyVisited = new HashSet<string>();

        /// <summary>
        /// Unique offers
        /// </summary>
        private readonly ISet<BaseAnnouncementInfo> _offers = new HashSet<BaseAnnouncementInfo>();
        
        /// <summary>
        /// Perform request to the specified url
        /// </summary>
        /// <param name="url">Page URL</param>
        private void Request(string url)
        {
            _alreadyVisited.Add(url);
            
            var page = _browser.NavigateToPage(new Uri(url));
            Parse(page.Html);
        }
        
        /// <summary>
        /// Parse HTML acquired from requested page
        /// </summary>
        /// <param name="html">HTML nodes</param>
        private void Parse(HtmlNode html)
        {
            var offers = GetOffers(html);
            var validOffers = offers.Where(offer => TypesToScrap.Contains(offer.Type));
            _offers.UnionWith(validOffers);
            _alreadyParsedCount++;

            if (_stopAfter != 0 && _alreadyParsedCount >= _stopAfter)
            {
                Logger.Log(LogLevel.Decision, $"Scraping limit of {_stopAfter} pages reached. Saving results...");
                Writer.SaveUrls(_offers);
                return;
            }

            var nextPage = GetNextPageUrl(html);
            if (nextPage == null)
            {
                Logger.Log(LogLevel.Decision, "Last page reached. Saving results...");
                Writer.SaveUrls(_offers);
                return;
            }

            if (_alreadyVisited.Contains(nextPage))
            {
                Logger.Log(LogLevel.Decision, $"{nextPage} already visited. Saving results...");
                Writer.SaveUrls(_offers);
                return;
            }

            Request(nextPage);
        }

        #region IAnnouncementScraper
        
        /// <inheritdoc cref="IAnnouncementScraper.Start" />
        public void Start(int startPage = 1, int stopAfter = 0)
        {
            if (HomeUrl == "")
                throw new ArgumentException("HomeUrl must be provided");

            _alreadyParsedCount = 0;
            _stopAfter = stopAfter;
            
            Logger.Log(LogLevel.Decision, $"Started scraping from page {startPage} with limit of {stopAfter} pages...");
            
            Request(GetPageUrl(startPage));
        }

        /// <inheritdoc cref="IAnnouncementScraper.ScrapeOffers" />
        public void ScrapeOffers()
        {
            foreach (var offer in _offers)
            {
                try
                {
                    var uri = new Uri(offer.Url);
                    var announcement = Parser.ParseOffer(offer.Url, _browser.NavigateToPage(uri).Html);
                    announcement.BaseInfo = offer;
                    
                    Writer.SaveOne(announcement);
                }
                catch (Exception e) when (e is AggregateException || e is InvalidOperationException || e is FormatException)
                {
                    Logger.Log(LogLevel.Error, $"Url {offer.Url} not scrappable. Skipping. ({e.Message})");
                }
            }
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetPageUrl" />
        public abstract string GetPageUrl(int page);

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public abstract ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        /// <inheritdoc cref="IAnnouncementScraper.GetNextPageUrl" />
        public abstract string GetNextPageUrl(HtmlNode html);

        #endregion
    }
}