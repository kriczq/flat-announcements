namespace Scrapers.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Message to be logged</param>
        public void Log(LogLevel level, string message);
    }
}