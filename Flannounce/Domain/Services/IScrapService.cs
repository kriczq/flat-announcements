using System.Collections.Generic;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Services
{
    public interface IScrapService
    {
        List<Announce> Scrap(ScrapParameters scrapParameters);
    }
}