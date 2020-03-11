using System.Collections.Generic;
using Flannounce.Domain.Model;

namespace Flannounce.Controllers
{
    public interface IFlatService
    {
        List<Flat> Get();

        Flat Get(string id);

        Flat Create(Flat student);

        void Update(string id, Flat studentIn);

        void Remove(Flat studentIn);

        void Remove(string id);
    }
}