using System;

namespace Scrapers.Logging
{
    /// <summary>
    /// Logging class that outputs logged information to a standard output.
    /// Message color indicates different log levels.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <inheritdoc cref="ILogger.Log" />
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