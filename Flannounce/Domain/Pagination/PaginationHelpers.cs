using System.Collections.Generic;
using System.Linq;
using Flannounce.Contracts;
using Flannounce.Domain.Filter;
using Flannounce.Domain.Services.Implementation;

namespace Flannounce.Domain
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response, string path, GetAllAnnouncesFilter filter)
        {
            var nextPage = pagination.PageNumber >= 1
                ? uriService.GetAllAnnouncesUri(path,new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize),filter).ToString()
                : null;
            
            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetAllAnnouncesUri(path,new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize),filter).ToString()
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };
        }
    }
}