using Benraz.Infrastructure.Common.Http;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Entities;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages
{
    /// <summary>
    /// Restore password request.
    /// </summary>
    public class RestorePasswordRequest : BenrazAuthorizationInternalLoginRequestBase
    {
        /// <summary>
        /// User.
        /// </summary>
        public RestorePassword User { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
             .SetBaseEndpoint(BenrazAuthorizationInternalLoginEndpoints.InternalLogin.RestorPassword)
             .ToString();

            return endpoint;
        }
    }
}

