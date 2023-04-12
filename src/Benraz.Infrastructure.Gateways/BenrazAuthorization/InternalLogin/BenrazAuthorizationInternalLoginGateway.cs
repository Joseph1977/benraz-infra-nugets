using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin
{
    /// <summary>
    /// Authorization internal login gateway.
    /// </summary>
    public class BenrazAuthorizationInternalLoginGateway : HttpGatewayBase, IBenrazAuthorizationInternalLoginGateway
    {
        private readonly BenrazAuthorizationInternalLoginGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP company factory.</param>
        /// <param name="settings">Settings.</param>
        public BenrazAuthorizationInternalLoginGateway(
            IHttpClientFactory httpClientFactory,
            IOptions<BenrazAuthorizationInternalLoginGatewaySettings> settings
        ) : base(httpClientFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Send confirmation email request.
        /// </summary>
        /// <param name="request">Send confirmation email request.</param>
        /// <returns>Send confirmation email response.</returns>
        public async Task<SendConfirmationEmailResponse> SendAsync(SendConfirmationEmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var requestContent = BuildContent(request.User);
                var responseMessage = await httpClient.PostAsync(requestUri, requestContent);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new SendConfirmationEmailResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                return response;
            }
        }

        /// <summary>
        /// Send restore password request.
        /// </summary>
        /// <param name="request">Restore password request.</param>
        /// <returns>Restore password response.</returns>
        public async Task<RestorePasswordResponse> SendAsync(RestorePasswordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var requestContent = BuildContent(request.User);
                var responseMessage = await httpClient.PostAsync(requestUri, requestContent);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new RestorePasswordResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                return response;
            }
        }

        /// <summary>
        /// Get action URL request.
        /// </summary>
        /// <param name="request">Get action URL request.</param>
        /// <returns>Get action URL response.</returns>
        public async Task<GetActionUrlResponse> SendAsync(GetActionUrlRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpClient = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var requestContent = BuildContent(request);
                var responseMessage = await httpClient.PostAsync(requestUri, requestContent);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new GetActionUrlResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (response.IsSuccessHttpStatusCode)
                {
                    response.ActionUrl = responseText;
                }
                return response;
            }
        }

        private HttpClient CreateHttpClient(BenrazAuthorizationInternalLoginRequestBase request)
        {
            return CreateHttpClient(request.AccessToken);
        }

        private StringContent BuildContent<TRequest>(TRequest request)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(request, Formatting.None, jsonSerializerSettings);
            var result = new StringContent(json, Encoding.UTF8);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        private string TrimEndSlash(string url)
        {
            return url.TrimEnd(new[] { '/' });
        }
    }
}

