using System;
using System.Collections.Generic;
using System.Text.Json;
using Scrapers.Model;

namespace Scrapers.Writing
{
    /// <summary>
    /// Writer that outputs information to the standard output.
    /// </summary>
    public class ConsoleWriter : IDataWriter
    {
        /// <inheritdoc cref="IDataWriter.SaveUrls" />
        public void SaveUrls(IEnumerable<BaseAnnouncementInfo> enumerable)
        {
            Header("Scraped urls");
            
            foreach (var info in enumerable)
                Console.WriteLine(JsonSerializer.Serialize(info));
        }

        /// <inheritdoc cref="IDataWriter.SaveOne" />
        public void SaveOne(Announcement announcement)
        {
            Header("Scraped announcement");
            
            Console.WriteLine(JsonSerializer.Serialize(announcement));
        }

        /// <inheritdoc cref="IDataWriter.SaveMany" />
        public void SaveMany(IEnumerable<Announcement> enumerable)
        {
            Header("Scraped announcements");
            
            foreach (var announcement in enumerable)
                Console.WriteLine(JsonSerializer.Serialize(announcement));
        }

        /// <summary>
        /// Write header to console in format:
        /// # --------------
        /// # message
        /// # --------------
        /// </summary>
        /// <param name="message">Message to be written</param>
        private static void Header(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            
            Console.Write("# ");
            Console.WriteLine(new string('-', message.Length * 2));
            
            Console.WriteLine($"# {message}");
            
            Console.Write("# ");
            Console.WriteLine(new string('-', message.Length * 2));
            
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}