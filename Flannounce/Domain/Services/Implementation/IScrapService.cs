using System.Collections.Generic;
using Flannounce.Model.DAO;
using Flannounce.Model.DTO;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IScrapService
    {
        IEnumerable<Announce> Scrap(ScrapParametersDto scrapParametersDto);

        IEnumerable<Announce> GetOnlyNewAnnounces(List<Announce> dbAnnounces, IEnumerable<Announce> scrapedAnnounces);

    }
}