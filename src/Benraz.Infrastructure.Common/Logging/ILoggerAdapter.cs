namespace Benraz.Infrastructure.Common.Logging
{
    /// <summary>
    /// Logger adapter.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface ILoggerAdapter<T>
    {
        /// <summary>
        /// Logs trace message.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogTrace(string message);

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="message">Message.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="exceptionMessage">Message.</param>
        void LogError(string exceptionMessage);
    }
}




