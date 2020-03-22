namespace Scrapers.Model
{
    public class BaseEntryInfo
    {
        /// <summary>
        /// Url to the entry
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Announcement Type
        /// </summary>
        public AnnouncementType Type { get; set; }
        
        /// <summary>
        /// Flag telling if the announcement was flagged
        /// as an ad
        /// </summary>
        public bool IsAd { get; set; }
    }
}