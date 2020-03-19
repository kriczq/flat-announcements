using System;

namespace Scrapers.Model
{
    public class OlxAnnouncement
    {
        public BaseAnnouncementInfo BaseInfo { get; set; }
        public string Id { get; set; }

        public string Title { get; set; }

        public string Voivodeship { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        
        public int BasePrice { get; set; }
        public float Rent { get; set; }
        public float PricePerSquareMeter { get; set; }
        
        public bool IsFromDeveloper { get; set; }
        public bool IncludesFurniture { get; set; }
        public float LivingSpace { get; set; }
        public string BuildingType { get; set; }
        public string Rooms { get; set; }
        public string Floor { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime ScrapedAt { get; set; } = DateTime.Now;
    }
}