using Microsoft.Extensions.Logging;
using System;

namespace Benraz.Infrastructure.Gateways
{
    /// <summary>
    /// Logger extensions.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a gateway HTTP response.
        /// </summary>
        /// <typeparam name="TResponse">Response type.</typeparam>
        /// <param name="logger">Logger.</param>
        /// <param name="response">Response.</param>
        public static void LogResponseTrace<TResponse>(this ILogger logger, TResponse response)
            where TResponse : HttpResponseBase
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            logger.LogTrace(
                "HTTP response - status code: {0}; content string: {1}.",
                response.HttpStatusCode,
                response.HttpContentString);
        }
    }
}



