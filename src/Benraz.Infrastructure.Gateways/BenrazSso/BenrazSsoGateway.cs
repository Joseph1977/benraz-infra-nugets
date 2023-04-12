using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Benraz.Infrastructure.Gateways.BenrazSso.Messages;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazSso
{
    /// <summary>
    /// Benraz SSO gateway.
    /// </summary>
    public class BenrazSsoGateway : HttpGatewayBase, IBenrazSsoGateway
    {
        private readonly BenrazSsoGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        public BenrazSsoGateway(
            IHttpClientFactory httpClientFactory,
            IOptions<BenrazSsoGatewaySettings> settings)
            : base(httpClientFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends login request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        public async Task<LoginResponse> SendAsync(LoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient())
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), request.GetEndpoint());

                var content = new StringContent(request.GetContent());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<LoginResponse>(responseText);
                return response;
            }
        }
    }
}



