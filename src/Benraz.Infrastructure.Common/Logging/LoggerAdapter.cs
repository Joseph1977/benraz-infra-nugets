using Microsoft.Extensions.Logging;

namespace Benraz.Infrastructure.Common.Logging
{
    /// <summary>
    /// Logger adapter.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        /// <summary>
        /// Creates logger adapter.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs trace message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void LogTrace(string message)
        {
            _logger.LogTrace(message);
        }

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="exceptionMessage">Message.</param>
        public void LogError(string exceptionMessage)
        {
            _logger.LogError(exceptionMessage);
        }
    }
}




