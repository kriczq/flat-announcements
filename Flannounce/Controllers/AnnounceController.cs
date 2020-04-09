using System.Collections.Generic;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        private readonly IAnnounceService _announceService;

        public AnnounceController(IAnnounceService  announceService)
        {
            _announceService = announceService;
        }

        [HttpGet]
        public ActionResult<List<Announce>> Get() =>
            _announceService.Get();

        [HttpGet("{id:length(24)}", Name = "GetFlat")]
        public ActionResult<Announce> Get(string id)
        {
            var flat = _announceService.Get(id);

            if (flat == null)
                return NotFound();

            return flat;
        }

        [HttpPost]
        public ActionResult<Announce> Create(Announce announce)
        {
            _announceService.Create(announce);

            return CreatedAtRoute("GetFlat", new { id = announce.Id }, announce);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Announce announceIn)
        {
            var flat = _announceService.Get(id);

            if (flat == null)
                return NotFound();

            _announceService.Update(id, announceIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var flat = _announceService.Get(id);

            if (flat == null)
                return NotFound();

            _announceService.Remove(flat.Id);

            return NoContent();
        }
    }
}