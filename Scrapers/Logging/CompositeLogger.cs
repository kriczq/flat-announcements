﻿using System.Collections.Generic;

namespace Scrapers.Logging
{
    public class CompositeLogger : ILogger
    {
        /// <summary>
        /// List of loggers
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public IList<ILogger> Loggers { get; } = new List<ILogger>();
        
        /// <inheritdoc cref="ILogger.Log" />
        public void Log(LogLevel level, string message)
        {
            foreach (var logger in Loggers)
                logger.Log(level, message);
        }
    }
}