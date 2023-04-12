using Benraz.Infrastructure.Common.Http;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user request.
    /// </summary>
    public class GetUserRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
                 .SetBaseEndpoint(string.Format(BenrazAuthorizationUsersEndpoints.Users.GetByUserId, UserId))
                 .ToString();

            return endpoint;
        }
    }
}



