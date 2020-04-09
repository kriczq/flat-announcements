using System.Collections.Generic;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IScrapService
    {
        IEnumerable<Announce> Scrap(ScrapParameters scrapParameters);

        IEnumerable<Announce> GetOnlyNewAnnounces(List<Announce> dbAnnounces, IEnumerable<Announce> scrapedAnnounces);

    }
}