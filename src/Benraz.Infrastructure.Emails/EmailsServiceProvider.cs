using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Benraz.Infrastructure.Emails
{
    /// <summary>
    /// Emails service provider.
    /// </summary>
    public class EmailsServiceProvider : IEmailsServiceProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BenrazEmailsService.BenrazEmailsService> _logger;
        private readonly EmailsServiceProviderSettings _settings;

        /// <summary>
        /// Creates emails service provider.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="logger">Logger.</param>
        public EmailsServiceProvider(
            IHttpClientFactory httpClientFactory,
            ILogger<BenrazEmailsService.BenrazEmailsService> logger,
            IOptions<EmailsServiceProviderSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _settings = settings.Value;
        }

        /// <summary>
        /// Returns actual emails service.
        /// </summary>
        /// <returns>Emails service.</returns>
        public IEmailsService GetService()
        {
            switch (_settings.ServiceType)
            {
                case EmailsServiceType.Benraz:
                    return new BenrazEmailsService.BenrazEmailsService(
                        _httpClientFactory, _logger, Options.Create(_settings.Benraz));
                default:
                    throw new NotSupportedException("Not supported emails service type.");
            }
        }
    }
}



