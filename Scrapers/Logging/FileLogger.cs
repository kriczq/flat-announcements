using System;
using System.IO;

namespace Scrapers.Logging
{
    public class FileLogger : ILogger
    {
        /// <summary>
        /// Absolute path where data will be written
        /// </summary>
        private readonly string _dataPath;

        public FileLogger()
        {
            _dataPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Logs";

            if (!Directory.Exists(_dataPath))
                Directory.CreateDirectory(_dataPath);
        }
        
        /// <inheritdoc cref="ILogger.Log" />
        public void Log(LogLevel level, string message)
        {
            var now = DateTime.Now;
            var file = $"{_dataPath}/{now:yyyy-MM-dd}.log";

            using var writer = File.AppendText(file);
            writer.WriteLine($"{now:g} [{level}] {message}");
        }
    }
}