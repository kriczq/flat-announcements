using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Scrapers.Model;

namespace Scrapers.Writing
{
    /// <summary>
    /// Writer that outputs information to the JSON file.
    /// </summary>
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
        public async void SaveOne(Announcement announcement)
        {
            var directory = $"{_dataPath}/Offers";
            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            await using var stream = new FileStream($"{directory}/{announcement.Id}.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, announcement);
        }

        /// <inheritdoc cref="IDataWriter.SaveMany" />
        public async void SaveMany(IEnumerable<Announcement> enumerable)
        {
            await using var stream = new FileStream($"{_dataPath}/Offers.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, enumerable);
        }
    }
}