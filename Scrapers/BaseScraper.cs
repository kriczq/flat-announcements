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
        /// Parser instance
        /// </summary>
        public IAnnouncementParser Parser { get; set; }

        /// <summary>
        /// Data writer instance
        /// </summary>
        public IDataWriter Writer { get; set; } = new ConsoleWriter();
        
        /// <summary>
        /// URL to start from
        /// </summary>
        protected string HomeUrl;

        /// <summary>
        /// AnnouncementTypes to scrap
        /// </summary>
        protected AnnouncementType[] AnnouncementTypes = new[]
            {AnnouncementType.Rent, AnnouncementType.Sale, AnnouncementType.Swap, AnnouncementType.Unknown};
        
        /// <summary>
        /// Scraping browser instance
        /// </summary>
        private readonly ScrapingBrowser _browser = new ScrapingBrowser
        {
            AutoDetectCharsetEncoding = false,
            Encoding = Encoding.UTF8
        };

        /// <summary>
        /// Already visited URLs
        /// </summary>
        private readonly ISet<string> _alreadyVisited = new HashSet<string>();

        /// <summary>
        /// Unique offers
        /// </summary>
        private readonly ISet<BaseAnnouncementInfo> _offers = new HashSet<BaseAnnouncementInfo>();
        

        private void Request(string url)
        {
            _alreadyVisited.Add(url);
            
            var page = _browser.NavigateToPage(new Uri(url));
            Parse(page.Html);
        }

        private void Parse(HtmlNode html)
        {
            _offers.UnionWith(GetOffers(html));
            var nextPage = GetNextPageUrl(html);

            if (nextPage != null)
            {
                if (!_alreadyVisited.Contains(nextPage))
                {
                    Request(nextPage);   
                }
                else
                {
                    Logger.Log(LogLevel.Decision, $"{nextPage} already visited. Saving results...");
                    Writer.SaveUrls(_offers);
                }
            }
            else
            {
                Logger.Log(LogLevel.Decision, "Last page reached. Saving results...");
                Writer.SaveUrls(_offers);
            }
        }

        #region IAnnouncementScraper
        
        /// <inheritdoc cref="IAnnouncementScraper.Start" />
        public void Start(AnnouncementType[] types = null)
        {
            if (HomeUrl == "")
                throw new ArgumentException("HomeUrl must be provided");
            
            if (types != null && types.Length > 0)
            {
                AnnouncementTypes = types;
            }
            
            Request(HomeUrl);
        }

        /// <inheritdoc cref="IAnnouncementScraper.ScrapeOffers" />
        public void ScrapeOffers()
        {
            foreach (var offer in _offers)
            {
                try
                {
                    var announcement = Parser.ParseOffer(_browser.NavigateToPage(new Uri(offer.Url)).Html);
                    announcement.BaseInfo = offer;
                    
                    Writer.SaveOne(announcement);
                }
                catch (Exception e) when (e is AggregateException || e is InvalidOperationException || e is FormatException)
                {
                    Logger.Log(LogLevel.Error, $"Url {offer.Url} not scrappable. Skipping. ({e.Message})");
                }
            }
        }

        public ISet<BaseAnnouncementInfo> GetOffers()
        {
            return _offers;
        }

        /// <inheritdoc cref="IAnnouncementScraper.GetOffers" />
        public abstract ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        /// <inheritdoc cref="IAnnouncementScraper.GetNextPageUrl" />
        public abstract string GetNextPageUrl(HtmlNode html);

        #endregion
    }
}