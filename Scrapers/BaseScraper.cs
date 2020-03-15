using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Scrapers.Model;
using ScrapySharp.Network;

namespace Scrapers
{
    public abstract class BaseScraper
    {
        private ScrapingBrowser _browser;
        
        /// <summary>
        /// URL to start from
        /// </summary>
        protected string HomeUrl;

        /// <summary>
        /// Unique offers
        /// </summary>
        private readonly ISet<BaseAnnouncementInfo> _offers = new HashSet<BaseAnnouncementInfo>();

        public BaseScraper()
        {
            _browser = new ScrapingBrowser();
        }

        public void Start()
        {
            if (HomeUrl == "")
                throw new ArgumentException("HomeUrl must be provided");

            Request(HomeUrl);
        }

        public void Request(string url)
        {
            var page = _browser.NavigateToPage(new Uri(url));
            Parse(page.Html);
        }

        public void Parse(HtmlNode html)
        {
            _offers.UnionWith(GetOffers(html));

            var nextPage = GetNextPageUrl(html);
            if (nextPage != null)
                Request(nextPage);
        }

        public void Log(LogLevel level, string message)
        {
            Console.ForegroundColor = level switch
            {
                LogLevel.Decision => ConsoleColor.DarkGreen,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Info => ConsoleColor.White,
                _ => ConsoleColor.White
            };

            Console.WriteLine($"[{level}] {message}");
        }
        
        public abstract ISet<BaseAnnouncementInfo> GetOffers(HtmlNode html);

        public abstract string GetNextPageUrl(HtmlNode html);
    }
}