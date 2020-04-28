namespace Scrapers.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Message to be logged</param>
        void Log(LogLevel level, string message);

        /// <summary>
        /// Log with <see cref="LogLevel.Decision" /> level.
        /// </summary>
        /// <param name="message">Message to be logged</param>
        void Decision(string message);

        /// <summary>
        /// Log with <see cref="LogLevel.Info" /> level.
        /// </summary>
        /// <param name="message">Message to be logged</param>
        void Info(string message);

        /// <summary>
        /// Log with <see cref="LogLevel.Error" /> level.
        /// </summary>
        /// <param name="message">Message to be logged</param>
        void Error(string message);
    }
}