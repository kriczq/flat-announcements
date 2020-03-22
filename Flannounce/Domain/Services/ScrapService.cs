using System.Collections.Generic;
using System.Linq;
using Flannounce.Controllers;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;
using Scrapers;
using Scrapers.Logging;
using Scrapers.Model;
using Scrapers.Writing;

namespace Flannounce.Domain.Services
{
    public class ScrapService : IScrapService
    {
        public List<Announce> Scrap(ScrapParameters scrapParameters)
        {
            var memory = new MemoryWriter();
            
            var scraper = new OlxScraper
            {
                Logger = new CompositeLogger
                {
                    Loggers =
                    {
                        new ConsoleLogger()
                    }
                },
                Writer = new CompositeWriter
                {
                    Writers =
                    {
                        new ConsoleWriter(),
                        memory
                    }
                }
            };
            scraper.Start(new[]{AnnouncementType.Sale});
            scraper.ScrapeOffers();

            return memory.Announcements.Select(a => a.ToAnnounce()).ToList();
        }
    }
}