﻿using System.Collections.Generic;
using Scrapers.Model;

namespace Scrapers.Writing
{
    public interface IDataWriter
    {
        /// <summary>
        /// Save scraped base announcement info
        /// </summary>
        /// <param name="enumerable">Scraped base announcement info</param>
        public void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable);
        
        /// <summary>
        /// Save one announcement
        /// </summary>
        /// <param name="announcement">Announcement</param>
        public void SaveOne(Announcement announcement);

        /// <summary>
        /// Save many announcements
        /// </summary>
        /// <param name="enumerable">Announcements</param>
        public void SaveMany(IEnumerable<Announcement> enumerable);
    }
}