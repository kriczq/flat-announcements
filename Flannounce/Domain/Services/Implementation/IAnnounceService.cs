using System.Collections.Generic;
using Flannounce.Domain.Filter;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IAnnounceService
    {
        List<Announce> Get(GetAllAnnouncesFilter filter,PaginationFilter paginationFilter);

        Announce Get(string id);

        Announce Create(Announce student);

        void Update(string id, Announce studentIn);

        void Remove(Announce studentIn);

        void Remove(string id);
    }
}