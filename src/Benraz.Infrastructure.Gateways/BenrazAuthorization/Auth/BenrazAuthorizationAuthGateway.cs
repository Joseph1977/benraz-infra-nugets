using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth
{
    /// <summary>
    /// Benraz auth gateway.
    /// </summary>
    public class BenrazAuthorizationAuthGateway : HttpGatewayBase, IBenrazAuthorizationAuthGateway
    {
        private readonly BenrazAuthorizationAuthGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        public BenrazAuthorizationAuthGateway(
            IHttpClientFactory httpClientFactory,
            IOptions<BenrazAuthorizationAuthGatewaySettings> settings)
            : base(httpClientFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends auth parameters request.
        /// </summary>
        /// <param name="request">Auth parameters request.</param>
        /// <returns>Auth parameters response.</returns>
        public async Task<ParametersResponse> SendAsync(ParametersRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient())
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<ParametersResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends is token active verification request.
        /// </summary>
        /// <param name="request">Is token active request.</param>
        /// <returns>Is token active response.</returns>
        public async Task<IsTokenActiveResponse> SendAsync(IsTokenActiveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient())
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                bool.TryParse(responseText, out var isActive);
                var response = new IsTokenActiveResponse
                {
                    IsActive = isActive
                };

                return response;
            }
        }

        /// <summary>
        /// Sends token request.
        /// </summary>
        /// <param name="request">Token request.</param>
        /// <returns>Token response.</returns>
        public async Task<TokenResponse> SendAsync(TokenRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient())
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var content = new StringContent(request.GetContent());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<TokenResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends exchange token request.
        /// </summary>
        /// <param name="request">Exchange token request.</param>
        /// <returns>Token response.</returns>
        public async Task<TokenResponse> SendAsync(ExchangeTokenRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request.AccessToken))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var content = new StringContent(request.GetContent());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<TokenResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends exchange token request.
        /// </summary>
        /// <param name="request">Exchange token request.</param>
        /// <returns>Token response.</returns>
        public async Task<ClaimVerifyResponse> SendAsync(ClaimVerifyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request.AccessToken))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new ClaimVerifyResponse
                {
                    HttpStatusCode = (int)responseMessage.StatusCode,
                    HttpContentString = responseText
                };
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.IsVerified = JsonConvert.DeserializeObject<bool>(responseText);
                }

                return response;
            }
        }

        private string TrimEndSlash(string url)
        {
            return url.TrimEnd(new[] { '/' });
        }
    }
}

