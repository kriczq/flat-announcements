using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Scrapers.Model;

namespace Scrapers.Writing
{
    public class FileWriter : IDataWriter
    { 
        /// <summary>
        /// Absolute path where data will be written
        /// </summary>
        private readonly string _dataPath;

        public FileWriter(string directory)
        {
            _dataPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{directory}";
            
            if (!Directory.Exists(_dataPath))
                Directory.CreateDirectory(_dataPath);
        }

        /// <inheritdoc cref="IDataWriter.SaveUrls" />
        public async void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable)
        {
            await using var stream = new FileStream($"{_dataPath}/Urls.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, enumerable);
        }

        /// <inheritdoc cref="IDataWriter.SaveOne" />
        public async void SaveOne(OlxAnnouncement olxAnnouncement)
        {
            var directory = $"{_dataPath}/Offers";
            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            await using var stream = new FileStream($"{directory}/{olxAnnouncement.Id}.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, olxAnnouncement);
        }

        /// <inheritdoc cref="IDataWriter.SaveMany" />
        public async void SaveMany(IEnumerable<OlxAnnouncement> enumerable)
        {
            await using var stream = new FileStream($"{_dataPath}/Offers.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, enumerable);
        }
    }
}