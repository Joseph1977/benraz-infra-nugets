using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Benraz.Infrastructure.Phone
{
    /// <summary>
    /// Phone service provider.
    /// </summary>
    public class PhoneServiceProvider : IPhoneServiceProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BenrazPhoneService.BenrazPhoneService> _logger;
        private readonly PhoneServiceProviderSettings _settings;

        /// <summary>
        /// Creates phone service provider.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="logger">Logger.</param>
        public PhoneServiceProvider(
            IHttpClientFactory httpClientFactory,
            ILogger<BenrazPhoneService.BenrazPhoneService> logger,
            IOptions<PhoneServiceProviderSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _settings = settings.Value;
        }

        /// <summary>
        /// Returns actual phone service.
        /// </summary>
        /// <returns>Phone service.</returns>
        public IPhoneService GetService()
        {
            switch (_settings.ServiceType)
            {
                case PhoneServiceType.Benraz:
                    return new BenrazPhoneService.BenrazPhoneService(
                        _httpClientFactory, _logger, Options.Create(_settings.Benraz));
                default:
                    throw new NotSupportedException("Not supported phone service type.");
            }
        }
    }
}


