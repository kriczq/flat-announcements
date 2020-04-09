using System.Collections.Generic;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Parser
{
    public interface IStreetParser
    {
        IEnumerable<Announce> ParseStreet(IEnumerable<Announce> announces);
    }
}