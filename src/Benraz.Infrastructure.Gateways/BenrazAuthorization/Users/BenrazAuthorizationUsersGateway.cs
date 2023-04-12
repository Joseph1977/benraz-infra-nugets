using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Entities;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users
{
    /// <summary>
    /// Benraz auth gateway.
    /// </summary>
    public class BenrazAuthorizationUsersGateway : HttpGatewayBase, IBenrazAuthorizationUsersGateway
    {
        private readonly BenrazAuthorizationUsersGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <param name="settings">Settings.</param>
        public BenrazAuthorizationUsersGateway(
            IHttpClientFactory httpClientFactory,
            IOptions<BenrazAuthorizationUsersGatewaySettings> settings)
            : base(httpClientFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends get users request.
        /// </summary>
        /// <param name="request">Get users request.</param>
        /// <returns>Get users response.</returns>
        public async Task<GetUsersResponse> SendAsync(GetUsersRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var responseMessage = await client.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new GetUsersResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.Page = JsonConvert.DeserializeObject<Page<BenrazAuthorizationUser>>(responseText);
                }

                return response;
            }
        }

        /// <summary>
        /// Sends get user request.
        /// </summary>
        /// <param name="request">Get user request.</param>
        /// <returns>Get user response.</returns>
        public async Task<GetUserResponse> SendAsync(GetUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var responseMessage = await client.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new GetUserResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.User = JsonConvert.DeserializeObject<BenrazAuthorizationUser>(responseText);
                }

                return response;
            }
        }

        /// <summary>
        /// Sends get user info by email request.
        /// </summary>
        /// <param name="request">Get user info by email request.</param>
        /// <returns>Get user info by email response.</returns>
        public async Task<GetUserInfoByEmailResponse> SendAsync(GetUserInfoByEmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var responseMessage = await client.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new GetUserInfoByEmailResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.User = JsonConvert.DeserializeObject<BenrazUserOpenIdViewModel>(responseText);
                }

                return response;
            }
        }

        /// <summary>
        /// Creates user request.
        /// </summary>
        /// <param name="request">Create user request.</param>
        /// <returns>Create user response.</returns>
        public async Task<CreateUserResponse> SendAsync(CreateUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var content = request.GetContent();

                var responseMessage = await client.PostAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new CreateUserResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.UserId = responseText;
                }

                return response;
            }
        }

        /// <summary>
        /// Sends get user roles request.
        /// </summary>
        /// <param name="request">Get user roles request.</param>
        /// <returns>Get user roles response.</returns>
        public async Task<GetUserRolesResponse> SendAsync(GetUserRolesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";

                var responseMessage = await client.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new GetUserRolesResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.Roles = JsonConvert.DeserializeObject<ICollection<string>>(responseText);
                }

                return response;
            }
        }

        /// <summary>
        /// Sends set user roles request.
        /// </summary>
        /// <param name="request">Set user roles request.</param>
        /// <returns>Set user roles response.</returns>
        public async Task<SetUserRolesResponse> SendAsync(SetUserRolesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var client = CreateHttpClient(request))
            {
                var baseUrl = TrimEndSlash(_settings.BaseUrl);
                var requestUri = $"{baseUrl}/{request.GetEndpoint()}";
                var content = request.GetContent();

                var responseMessage = await client.PutAsync(requestUri, content);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = new SetUserRolesResponse();
                response.HttpStatusCode = (int)responseMessage.StatusCode;
                response.HttpContentString = responseText;

                return response;
            }
        }

        private HttpClient CreateHttpClient(BenrazAuthorizationUsersRequestBase request)
        {
            return CreateHttpClient(request.AccessToken);
        }

        private string TrimEndSlash(string url)
        {
            return url.TrimEnd(new[] { '/' });
        }
    }
}



