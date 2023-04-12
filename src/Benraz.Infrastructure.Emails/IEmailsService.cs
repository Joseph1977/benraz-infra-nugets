using System.Net.Mail;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public interface IEmailsService
    {
        /// <summary>
        /// Sends email.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        Task SendEmailAsync(string from, string[] to, string subject, string body);

        /// <summary>
        /// Sends email.
        /// </summary>
        /// <param name="mailMessage">Mail message.</param>
        Task SendEmailAsync(MailMessage mailMessage);
    }
}



