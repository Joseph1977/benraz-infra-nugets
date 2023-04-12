using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Gateways.BenrazCommon;
using Benraz.Infrastructure.Gateways.BenrazServices;
using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Phone.BenrazPhoneService
{
    /// <summary>
    /// Benraz phone service.
    /// </summary>
    public class BenrazPhoneService : IPhoneService
    {
        private readonly IBenrazServicesGateway _BenrazServicesGateway;
        private readonly BenrazPhoneServiceSettings _phoneSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BenrazPhoneService> _logger;

        /// <summary>
        /// Creates Benraz phone service.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="phoneSettings">phone settings.</param>
        /// <param name="logger">Logger.</param>
        public BenrazPhoneService(
            IHttpClientFactory httpClientFactory,
            ILogger<BenrazPhoneService> logger,
            IOptions<BenrazPhoneServiceSettings> phoneSettings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _phoneSettings = phoneSettings.Value;

            var BenrazGatewaySettings = new BenrazCommonGatewaySettings
            {
                BaseUrl = _phoneSettings.BaseUrl
            };
            _BenrazServicesGateway = new BenrazServicesGateway(
                _httpClientFactory, Options.Create(BenrazGatewaySettings));
        }

        /// <summary>
        /// Sends sms.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="body">Body.</param>
        public async Task SendSmsAsync(string sender, List<string> recipients, string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (recipients == null)
            {
                throw new ArgumentNullException(nameof(recipients));
            }

            await SendAsync(sender, recipients, body);
        }

        private async Task SendAsync(string sender, List<string> recipients, string body)
        {
            try
            {
                var request = CreateRequest(sender, recipients, body);
                var response = await _BenrazServicesGateway.SendSmsAsync(request);

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
                    "Error while sending sms from {0} to {1}. Message: {2}.",
                    sender,
                    string.Join(",", GeRecipientsStrings(recipients)),
                    body);
                throw;
            }
        }

        private PhoneRequest CreateRequest(string sender, List<string> recipients, string body)
        {
            var request = new PhoneRequest
            {
                Body = body,
                Sender = sender,
                Recipients = recipients
            };

            return request;
        }

        private string[] GeRecipientsStrings(List<string> recipients)
        {
            var addressStrings = recipients.ToArray();
            return addressStrings;
        }
    }
}


