using System.Net.Http;
using System.Net.Http.Headers;

namespace Benraz.Infrastructure.Gateways
{
    /// <summary>
    /// HTTP base gateway.
    /// </summary>
    public abstract class HttpGatewayBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        public HttpGatewayBase(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Creates HTTP client.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <returns>HTTP client.</returns>
        protected HttpClient CreateHttpClient(string accessToken = null)
        {
            var httpClient = _httpClientFactory?.CreateClient() ?? new HttpClient();

            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return httpClient;
        }
    }
}



