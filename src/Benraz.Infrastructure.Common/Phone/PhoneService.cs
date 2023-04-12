using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Benraz.Infrastructure.Common.Phone
{
    /// <summary>
    /// Phone service.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        private readonly PhoneServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Phone service settings.</param>
        public PhoneService(IOptions<PhoneServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends sms.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="body">Body.</param>
        /// <returns>Task.</returns>
        public Task SendSms(string sender, List<string> recipients, string body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            return SendAsync(sender, recipients, body);
        }

        private async Task SendAsync(string sender, List<string> recipients, string body)
        {
            sender = !string.IsNullOrEmpty(sender) ? sender : _settings.OutTwillionumber;
            CreateTwillioClient(_settings.AccountSID, _settings.AuthToken);
            foreach (var number in recipients)
            {
                var message = await MessageResource.CreateAsync(
                body: body,
                from: new Twilio.Types.PhoneNumber(sender),
                to: new Twilio.Types.PhoneNumber(number));
            }
        }

        private void CreateTwillioClient(string accountSID, string authToken)
        {
            TwilioClient.Init(accountSID, authToken);
        }
    }
}


