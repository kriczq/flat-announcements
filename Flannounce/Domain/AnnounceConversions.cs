using System;
using Flannounce.Model.DAO;
using Flannounce.Model.DAO.Enums;
using Scrapers.Model;

namespace Flannounce.Domain
{
    public static class AnnounceConversions
    {
        public static Announce ToAnnounce(this Announcement announcement)
        {
            if (announcement is null)
            {
                return null;
            }


            return new Announce()
            {
                AnnounceId = announcement.Id,
                AnnounceType = announcement.BaseInfo.Type.ToString(),
                Title = announcement.Title,
                Url = announcement.BaseInfo.Url,
                City = announcement.City,
                District = announcement.District,
                Price = announcement.BasePrice,
                PricePerSquareMeter = (decimal) announcement.PricePerSquareMeter,
                OfferedBy = announcement.IsFromDeveloper ? OfferedBy.Private : OfferedBy.Agency,
                IncludesFurniture = announcement.IncludesFurniture,
                LivingSpace = (decimal) announcement.LivingSpace,
                BuildingType = Parsers.Parse(announcement.BuildingType),
                Rooms = announcement.Rooms,
                Floor = announcement.Floor,
                CreatedAt = announcement.CreatedAt,
                ScrapedAt = announcement.ScrapedAt,
            };
        }
    }
}