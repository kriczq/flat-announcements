using System.Collections.Generic;
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

    public ScrapController(IAnnounceService  announceService, IScrapService scrapService)
    {
        _announceService = announceService;
        _scrapService = scrapService;
    }

    [HttpPost]
    public ActionResult<List<Announce>> Scrap(ScrapParameters scrapParameters)
    {
        var announces = _scrapService.Scrap(new ScrapParameters());

        foreach (var announce in announces)
        {
            _announceService.Create(announce);
        }

        return announces;
    }

    }
}