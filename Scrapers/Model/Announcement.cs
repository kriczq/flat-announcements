﻿using System;
using System.Collections.Generic;

namespace Scrapers.Model
{
    public class Announcement
    {
        public BaseAnnouncementInfo BaseInfo { get; set; }
        public string Id { get; set; }

        public string Title { get; set; }
        public List<string> Images { get; set; }

        public string Voivodeship { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public float BasePrice { get; set; }
        public float Rent { get; set; }
        public float PricePerSquareMeter { get; set; }
        
        public bool IsFromDeveloper { get; set; }
        public bool IncludesFurniture { get; set; }
        public float LivingSpace { get; set; }
        public string BuildingType { get; set; }
        public string Rooms { get; set; }
        public string Floor { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime ScrapedAt { get; } = DateTime.Now;
    }
}