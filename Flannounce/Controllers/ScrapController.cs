using System.Collections.Generic;
using System.Linq;
using Flannounce.Domain.Parser;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
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
        private readonly IStreetParser _streetParser;

        public ScrapController(IAnnounceService announceService, IScrapService scrapService, IStreetParser streetParser)
        {
            _announceService = announceService;
            _scrapService = scrapService;
            _streetParser = streetParser;
        }

        [HttpPost]
        public ActionResult<List<Announce>> Scrap(ScrapParameters scrapParameters)
        {
            var scrapedAnnounces = _scrapService.Scrap(new ScrapParameters()); 
            var dbAnnounces = _announceService.Get();
            var newAnnounces =  _streetParser.ParseStreet(_scrapService.GetOnlyNewAnnounces(dbAnnounces, scrapedAnnounces)).ToList();
            
            foreach (var newAnnounce in newAnnounces)
            {
                _announceService.Create(newAnnounce);
            }

            return newAnnounces;
        }
    }
}