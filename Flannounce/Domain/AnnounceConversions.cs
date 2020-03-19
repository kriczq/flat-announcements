using System;
using Flannounce.Model.DAO;
using Flannounce.Model.DAO.Enums;
using Scrapers.Model;

namespace Flannounce.Domain
{
    public static class AnnounceConversions
    {
        public static Announce ToAnnounce(this OlxAnnouncement olxAnnouncement)
        {
            if (olxAnnouncement is null)
            {
                return null;
            }


            return new Announce()
            {
                AnnounceId = olxAnnouncement.Id,
                Title = olxAnnouncement.Title,
                Url = olxAnnouncement.BaseInfo.Url,
                City = olxAnnouncement.City,
                District = olxAnnouncement.District,
                Price = olxAnnouncement.BasePrice,
                PricePerSquareMeter = (decimal) olxAnnouncement.PricePerSquareMeter,
                IsFromDeveloper = olxAnnouncement.IsFromDeveloper,
                IncludesFurniture = olxAnnouncement.IncludesFurniture,
                LivingSpace = (decimal) olxAnnouncement.LivingSpace,
                BuildingType = Parsers.Parse(olxAnnouncement.BuildingType),
                Rooms = olxAnnouncement.Rooms,
                Floor = olxAnnouncement.Floor,
                CreatedAt = olxAnnouncement.CreatedAt,
                ScrapedAt = olxAnnouncement.CreatedAt,
            };
        }
    }
}