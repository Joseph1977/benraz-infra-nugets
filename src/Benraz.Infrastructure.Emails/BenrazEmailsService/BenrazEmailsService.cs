using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Gateways.BenrazCommon;
using Benraz.Infrastructure.Gateways.BenrazServices;
using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Emails.BenrazEmailsService
{
    /// <summary>
    /// Benraz emails service.
    /// </summary>
    public class BenrazEmailsService : IEmailsService
    {
        private readonly IBenrazServicesGateway _BenrazServicesGateway;
        private readonly BenrazEmailsServiceSettings _emailSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BenrazEmailsService> _logger;

        /// <summary>
        /// Creates Benraz emails service.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="emailSettings">Email settings.</param>
        /// <param name="logger">Logger.</param>
        public BenrazEmailsService(
            IHttpClientFactory httpClientFactory,
            ILogger<BenrazEmailsService> logger,
            IOptions<BenrazEmailsServiceSettings> emailSettings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _emailSettings = emailSettings.Value;

            var BenrazGatewaySettings = new BenrazCommonGatewaySettings
            {
                BaseUrl = _emailSettings.BaseUrl
            };
            _BenrazServicesGateway = new BenrazServicesGateway(
                _httpClientFactory, Options.Create(BenrazGatewaySettings));
        }

        /// <summary>
        /// Sends email.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        public async Task SendEmailAsync(string @from, string[] to, string subject, string body)
        {
            if (string.IsNullOrEmpty(@from))
            {
                throw new ArgumentNullException(nameof(@from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(@from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(string.Join(",", to));

            await SendAsync(mailMessage);
        }

        /// <summary>
        /// Sends email.
        /// </summary>
        /// <param name="mailMessage">Mail message.</param>
        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException(nameof(mailMessage));
            }

            await SendAsync(mailMessage);
        }

        private async Task SendAsync(MailMessage mailMessage)
        {
            try
            {
                var request = CreateRequest(mailMessage);
                var response = await _BenrazServicesGateway.SendAsync(request);

                if (response == null)
                {
                    throw new InvalidOperationException("Failed to receive a response from the gateway.");
                }

                if (!response.IsSuccessHttpStatusCode)
                {
                    throw new InvalidOperationException(
                        $"Response from the gateway does not indicate success. " +
                        $"Status code: {response.HttpStatusCode}. " +
                        $"Content: {response.HttpContentString}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while sending email from {0} to {1}. Email subject: {2}.",
                    mailMessage.From.Address,
                    string.Join(",", GetAddressStrings(mailMessage.To)),
                    mailMessage.Subject);

                throw;
            }
        }

        private EmailRequest CreateRequest(MailMessage mailMessage)
        {
            var toAddresses = GetAddressStrings(mailMessage.To);
            var request = new EmailRequest
            {
                AccessToken = _emailSettings.AccessToken,
                BasicInfo = new EmailBasicInfo
                {
                    From = mailMessage.From.Address,
                    DisplayName = mailMessage.From.DisplayName,
                    To = toAddresses.Length == 1 ? toAddresses.Single() : null,
                    Tos = toAddresses.Length > 1 ? toAddresses : null,
                    Subject = mailMessage.Subject,
                    TemplateId = _emailSettings.TemplateId,
                    SkipOptOutCheck = true
                },
                EmailParams = new Dictionary<string, string>
                {
                    {"-html-", mailMessage.Body}
                },
            };

            return request;
        }

        private string[] GetAddressStrings(MailAddressCollection addresses)
        {
            var addressStrings = addresses.Select(x => x.Address).ToArray();
            return addressStrings;
        }
    }
}