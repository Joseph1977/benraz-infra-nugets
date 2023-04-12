using System;

namespace Benraz.Infrastructure.Common.Exceptions
{
    /// <summary>
    /// Domain exception.
    /// </summary>
    public class DomainException : InvalidOperationException
    {
        /// <summary>
        /// Message to a user.
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// Creates exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="userMessage">User message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DomainException(string message = null, string userMessage = null, Exception innerException = null)
            : base(message, innerException)
        {
            UserMessage = userMessage;
        }
    }
}




