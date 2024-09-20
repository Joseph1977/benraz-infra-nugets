using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Gateways.BenrazCommon;
using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;

namespace Benraz.Infrastructure.Gateways.BenrazServices
{
    /// <summary>
    /// Benraz services gateway.
    /// </summary>
    public class BenrazServicesGateway : HttpGatewayBase, IBenrazServicesGateway
    {
        private readonly BenrazCommonGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        public BenrazServicesGateway(
            IHttpClientFactory httpClientFactory,
            IOptions<BenrazCommonGatewaySettings> settings)
            : base(httpClientFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends email request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        public async Task<EmailResponse> SendAsync(EmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var content = request.GetContent();

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new EmailResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;

                return response;
            }
        }

        /// <summary>
        /// Sends SMS V2 request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        public async Task<PhoneResponse> SendSmsAsync(PhoneRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var content = request.GetContent();

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new PhoneResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;

                return response;
            }
        }

        private HttpClient CreateHttpClient(BenrazServicesRequestBase request)
        {
            return CreateHttpClient(request.AccessToken);
        }

        private string TrimEndSlash(string url)
        {
            return url.TrimEnd(new[] { '/' });
        }
    }
}



