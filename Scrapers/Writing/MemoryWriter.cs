using System.Collections.Generic;
using Scrapers.Model;

namespace Scrapers.Writing
{
    /// <summary>
    /// Writer class that saves data only to collection stored in memory.
    /// </summary>
    public class MemoryWriter : IDataWriter
    {
        /// <summary>
        /// List of base announcement infos
        /// </summary>
        public List<BaseAnnouncementInfo> BaseAnnouncementInfos { get; } = new List<BaseAnnouncementInfo>();

        /// <summary>
        /// List of announcements
        /// </summary>
        public List<Announcement> Announcements { get; } = new List<Announcement>();
        
        /// <inheritdoc cref="IDataWriter.SaveUrls" />
        public void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable)
        {
            BaseAnnouncementInfos.AddRange(enumerable);
        }

        /// <inheritdoc cref="IDataWriter.SaveOne" />
        public void SaveOne(Announcement announcement)
        {
            Announcements.Add(announcement);
        }

        /// <inheritdoc cref="IDataWriter.SaveMany" />
        public void SaveMany(IEnumerable<Announcement> enumerable)
        {
            Announcements.AddRange(enumerable);
        }
    }
}