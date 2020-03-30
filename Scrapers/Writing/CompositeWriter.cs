using System.Collections.Generic;
using System.Linq;
using Scrapers.Model;

namespace Scrapers.Writing
{
    /// <summary>
    /// Writer class that should be used to compose multiple writers into one
    /// </summary>
    public class CompositeWriter : IDataWriter
    {
        /// <summary>
        /// List of writers
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public IList<IDataWriter> Writers { get; } = new List<IDataWriter>();
        
        /// <inheritdoc cref="IDataWriter.SaveUrls" />
        public void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable)
        {
            var baseInfos = enumerable.ToList();
            foreach (var writer in Writers)
                writer.SaveUrls(baseInfos);
        }

        /// <inheritdoc cref="IDataWriter.SaveOne" />
        public void SaveOne(Announcement announcement)
        {
            foreach (var writer in Writers)
                writer.SaveOne(announcement);
        }

        /// <inheritdoc cref="IDataWriter.SaveMany" />
        public void SaveMany(IEnumerable<Announcement> enumerable)
        {
            var announcements = enumerable.ToList();
            foreach (var writer in Writers)
                writer.SaveMany(announcements);
        }
    }
}