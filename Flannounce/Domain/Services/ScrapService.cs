using System.Collections.Generic;
using System.Linq;
using Flannounce.Domain;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;
using Scrapers;

namespace Flannounce.Controllers
{
    public class ScrapService : IScrapService
    {
        public List<Announce> Scrap(ScrapParameters scrapParameters)
        {
            var scraper = new OlxScraper();
            scraper.Start();
            scraper.ScrapeOffers();
            var announcments = scraper.GetAnnouncements();

            return announcments.Select(a => a.ToAnnounce()).ToList();
        }
    }
}