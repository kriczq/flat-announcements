using System.Collections.Generic;
using Flannounce.Model.DAO;

namespace Flannounce.Controllers
{
    public interface IAnnounceService
    {
        List<Announce> Get();

        Announce Get(string id);

        Announce Create(Announce student);

        void Update(string id, Announce studentIn);

        void Remove(Announce studentIn);

        void Remove(string id);
    }
}