using System;
using System.Net;

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

        /// Http status code.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Creates exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="userMessage">User message.</param>
        /// <param name="innerException">Inner exception.</param>
        /// <param name="httpStatusCode">Http status code.</param>
        public DomainException(string message = null, string userMessage = null, Exception innerException = null, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            UserMessage = userMessage;
            HttpStatusCode = httpStatusCode;
        }
    }
}




