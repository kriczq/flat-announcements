using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Flannounce.Configuration;
using Flannounce.Contracts;
using Flannounce.Domain;
using Flannounce.Domain.Filter;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
using Flannounce.Domain.Utils;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
    public class AnnounceController : Controller
    {
        private readonly IAnnounceService _announceService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public AnnounceController(IAnnounceService  announceService, IUriService uriService, IMapper mapper)
        {
            _announceService = announceService;
            _uriService = uriService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Announce.GetAll)]
        public ActionResult<List<Announce>>  GetAll([FromQuery]GetAllAnnouncesQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var filter = _mapper.Map<GetAllAnnouncesFilter>(query);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var announces = _announceService.Get(filter,paginationFilter);
            
            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<Announce>(announces));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, announces, ApiRoutes.Announce.GetAll,filter);
            return Ok(paginationResponse);
        }
        
        [HttpGet(ApiRoutes.Announce.GetAvgPricePerCity)]
        public ActionResult<List<Announce>>  GetAvgPricePerCity([FromQuery]GetAllAnnouncesQuery query, [FromQuery]int count = 10000)
        {
            var filter = _mapper.Map<GetAllAnnouncesFilter>(query);
            var paginationFilter = new PaginationFilter()
            {
                PageNumber = 1,
                PageSize = count
            };
            var announces = _announceService.Get(filter,paginationFilter);      

            return Ok(announces.ConvertToAveragePricePerCities());
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