using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Emails
{
    /// <summary>
    /// Email service.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Email service settings.</param>
        public EmailService(IOptions<EmailServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends email messages.
        /// </summary>
        /// <param name="sender">Email sender.</param>
        /// <param name="recipients">Email recipients.</param>
        /// <param name="subject">Message subject.</param>
        /// <param name="body">Message body.</param>
        /// <returns>Task.</returns>
        public Task SendEmailAsync(string sender, IEnumerable<string> recipients, string subject, string body)
        {
            return SendAsync(sender, recipients, subject, body);
        }

        /// <summary>
        /// Sends email messages.
        /// </summary>
        /// <param name="sender">Email sender.</param>
        /// <param name="recipients">Email recipients.</param>
        /// <param name="message">Email message.</param>
        /// <returns>Task.</returns>
        public Task SendEmailAsync(string sender, IEnumerable<string> recipients, EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return SendAsync(sender, recipients, message.Title, message.Body);
        }

        private async Task SendAsync(string sender, IEnumerable<string> recipients, string subject, string body)
        {
            sender = !string.IsNullOrEmpty(sender) ? sender : _settings.DefaultSenderAddress;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(sender);
                foreach(var recipient in recipients)
                {
                    mailMessage.To.Add(recipient);
                }

                using (var mailClient = CreateSmtpClient())
                {
                    await mailClient.SendMailAsync(mailMessage);
                }
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient
            {
                Host = _settings.SmtpHostName,
                Port = _settings.SmtpPort,
                EnableSsl = _settings.IsSslEnabled
            };

            if (!string.IsNullOrEmpty(_settings.Login) && !string.IsNullOrEmpty(_settings.Password))
            {
                client.Credentials = new NetworkCredential(_settings.Login, _settings.Password);
            }

            return client;
        }
    }
}




