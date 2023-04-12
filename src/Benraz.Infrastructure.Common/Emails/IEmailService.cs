using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Emails
{
    /// <summary>
    /// Email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends email messages.
        /// </summary>
        /// <param name="sender">Email sender.</param>
        /// <param name="recipients">Email recipients.</param>
        /// <param name="subject">Message subject.</param>
        /// <param name="body">Message body.</param>
        /// <returns>Task.</returns>
        Task SendEmailAsync(string sender, IEnumerable<string> recipients, string subject, string body);

        /// <summary>
        /// Sends email messages.
        /// </summary>
        /// <param name="sender">Email sender.</param>
        /// <param name="recipients">Email recipients.</param>
        /// <param name="message">Email message.</param>
        /// <returns>Task.</returns>
        Task SendEmailAsync(string sender, IEnumerable<string> recipients, EmailMessage message);
    }
}




