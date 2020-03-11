using System.Collections.Generic;
using Flannounce.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : ControllerBase
    {
        private readonly IFlatService _flatService;

        public FlatController(IFlatService  flatService)
        {
            _flatService = flatService;
        }

        [HttpGet]
        public ActionResult<List<Flat>> Get() =>
            _flatService.Get();

        [HttpGet("{id:length(24)}", Name = "GetFlat")]
        public ActionResult<Flat> Get(string id)
        {
            var flat = _flatService.Get(id);

            if (flat == null)
                return NotFound();

            return flat;
        }

        [HttpPost]
        public ActionResult<Flat> Create(Flat flat)
        {
            _flatService.Create(flat);

            return CreatedAtRoute("GetFlat", new { id = flat.FlatId }, flat);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Flat flatIn)
        {
            var flat = _flatService.Get(id);

            if (flat == null)
                return NotFound();

            _flatService.Update(id, flatIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var flat = _flatService.Get(id);

            if (flat == null)
                return NotFound();

            _flatService.Remove(flat.FlatId);

            return NoContent();
        }
    }
}