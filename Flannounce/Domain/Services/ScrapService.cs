using System.Collections.Generic;
using System.Linq;
using Flannounce.Controllers;
using Flannounce.Domain.Services.Implementation;
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
        public IEnumerable<Announce> Scrap(ScrapParameters scrapParameters)
        {
            var memory = new MemoryWriter();
            
            var scraper = new OlxScraper
            {
                TypesToScrap = new [] { AnnouncementType.Sale },
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
            scraper.Start();
            scraper.ScrapeOffers();

            return memory.Announcements.Select(a => a.ToAnnounce());
        }

        public IEnumerable<Announce> GetOnlyNewAnnounces(List<Announce> dbAnnounces, IEnumerable<Announce> scrapedAnnounces)
        {
            var dic = new HashSet<Announce>(Announce.AnnounceComparer);
            foreach (var dbAnnounce in dbAnnounces)
            {
                dic.Add(dbAnnounce);
            }

            foreach (var scrapedAnnounce in scrapedAnnounces)
            {
                if (dic.Add(scrapedAnnounce))
                {
                    yield return scrapedAnnounce;
                }
            }
        }
    }
}