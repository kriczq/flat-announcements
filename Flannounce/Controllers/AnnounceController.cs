using System.Collections.Generic;
using System.Threading.Tasks;
using Flannounce.Configuration;
using Flannounce.Contracts;
using Flannounce.Domain;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
//    [Route("api/[controller]")]
//    [ApiController]
    public class AnnounceController : Controller
    {
        private readonly IAnnounceService _announceService;
        private readonly IUriService _uriService;

        public AnnounceController(IAnnounceService  announceService, IUriService uriService)
        {
            _announceService = announceService;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Announce.GetAll)]
        public ActionResult<List<Announce>>  GetAll([FromQuery]PaginationQuery paginationQuery)
        {
            var paginationFilter = paginationQuery.ToFilter();
            var announces = _announceService.Get(paginationFilter);
            
            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<Announce>(announces));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, announces, ApiRoutes.Announce.GetAll);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Announce.Get)]
        public ActionResult<Announce> Get(string id)
        {
            var flat = _announceService.Get(id);

            if (flat == null)
                return NotFound();

            return flat;
        }
    }
}