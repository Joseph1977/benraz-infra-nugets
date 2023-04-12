using Benraz.Infrastructure.Common.Http;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user info by email request.
    /// </summary>
    public class GetUserInfoByEmailRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
                 .SetBaseEndpoint(BenrazAuthorizationUsersEndpoints.Users.GetByEmail)
                 .AddQueryParameter("email", Email)
                 .ToString();

            return endpoint;
        }
    }
}


