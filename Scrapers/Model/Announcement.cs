namespace Scrapers.Model
{
    public class Announcement
    {
        public BaseAnnouncementInfo BaseInfo { get; set; }
        public string Id { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsAd { get; set; }
        
        public string Voivodeship { get; set; }
        public string City { get; set; }
        
        public int BasePrice { get; set; }
        public int Rent { get; set; }
        
        public bool IsFromDeveloper { get; set; }
        public bool IncludesFurniture { get; set; }
        public int LivingSpace { get; set; }
        public string BuildingType { get; set; }
        public string Rooms { get; set; }
        public string Floor { get; set; }
    }
}