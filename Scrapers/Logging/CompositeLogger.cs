using System.Collections.Generic;

namespace Scrapers.Logging
{
    /// <summary>
    /// Logging class that should be used to compose multiple loggers into one.
    /// </summary>
    public class CompositeLogger : BaseLogger
    {
        /// <summary>
        /// List of loggers
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public IList<ILogger> Loggers { get; } = new List<ILogger>();
        
        /// <inheritdoc cref="BaseLogger.Log" />
        public override void Log(LogLevel level, string message)
        {
            foreach (var logger in Loggers)
                logger.Log(level, message);
        }
    }
}