using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Set user roles request.
    /// </summary>
    public class SetUserRolesRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [JsonIgnore]
        public string UserId { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = $"v1/users/{UserId}/roles";
            return endpoint;
        }

        /// <summary>
        /// Returns request content.
        /// </summary>
        /// <returns>Request content.</returns>
        public HttpContent GetContent()
        {
            return GetJsonContent(Roles);
        }
    }
}



