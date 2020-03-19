using System.Collections.Generic;
using Flannounce.Model.Content;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Flannounce.Controllers
{
    public interface IScrapService
    {
        List<Announce> Scrap(ScrapParameters scrapParameters);
    }
}