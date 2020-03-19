using System.Collections.Generic;
using Scrapers.Model;

namespace Scrapers.Writing
{
    public interface IDataWriter
    {
        /// <summary>
        /// Save scraped base announcement info
        /// </summary>
        /// <param name="enumerable">Scraped base announcement info</param>
        void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable);
        
        /// <summary>
        /// Save one announcement
        /// </summary>
        /// <param name="olxAnnouncement">Announcement</param>
        void SaveOne(OlxAnnouncement olxAnnouncement);

        /// <summary>
        /// Save many announcements
        /// </summary>
        /// <param name="enumerable">Announcements</param>
        void SaveMany(IEnumerable<OlxAnnouncement> enumerable);
    }
}