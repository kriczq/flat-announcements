using System;

namespace Scrapers.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.ForegroundColor = level switch
            {
                LogLevel.Decision => ConsoleColor.DarkGreen,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Info => ConsoleColor.White,
                _ => ConsoleColor.White
            };

            Console.WriteLine($"[{level}] {message}");
        }
    }
}