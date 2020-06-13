using AutoMapper;
using Flannounce.Domain.Filter;

namespace Flannounce.Domain.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllAnnouncesQuery, GetAllAnnouncesFilter>();
        }
    }
}