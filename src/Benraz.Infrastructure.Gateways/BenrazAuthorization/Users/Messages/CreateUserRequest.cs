using System.Collections.Generic;
using System.Net.Http;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Create user request.
    /// </summary>
    public class CreateUserRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Password to set.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public ICollection<BenrazAuthorizationUserClaim> Claims { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = $"v1/users";
            return endpoint;
        }

        /// <summary>
        /// Returns request content.
        /// </summary>
        /// <returns>Request content.</returns>
        public HttpContent GetContent()
        {
            return GetJsonContent();
        }
    }
}



