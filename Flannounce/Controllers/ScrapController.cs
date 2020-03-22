using System.Collections.Generic;
using System.Linq;
using Flannounce.Domain.Services;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapController : ControllerBase
    {
        private readonly IAnnounceService _announceService;
        private readonly IScrapService _scrapService;

        public ScrapController(IAnnounceService announceService, IScrapService scrapService)
        {
            _announceService = announceService;
            _scrapService = scrapService;
        }

        [HttpPost]
        public ActionResult<List<Announce>> Scrap(ScrapParameters scrapParameters)
        {
            var scrapedAnnounces = _scrapService.Scrap(new ScrapParameters()); 
            var dbAnnounces = _announceService.Get();
            var newAnnounces = GetOnlyNew(dbAnnounces,scrapedAnnounces).ToList();
            
            
            foreach (var newAnnounce in newAnnounces)
            {
                _announceService.Create(newAnnounce);
            }

            return newAnnounces;
        }

        private IEnumerable<Announce> GetOnlyNew(List<Announce> dbAnnounces, List<Announce> scrapedAnnounces)
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
                else
                {
                    var lol = "wad";
                }
            }
        }
    }
}