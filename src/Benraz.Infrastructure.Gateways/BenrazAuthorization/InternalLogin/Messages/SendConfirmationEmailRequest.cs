using Benraz.Infrastructure.Common.Http;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Entities;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages
{
    /// <summary>
    /// Send confirmation email request.
    /// </summary>
    public class SendConfirmationEmailRequest : BenrazAuthorizationInternalLoginRequestBase
    {
        /// <summary>
        /// User.
        /// </summary>
        public SendConfirmationEmail User { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
             .SetBaseEndpoint(BenrazAuthorizationInternalLoginEndpoints.InternalLogin.SendConfirmationEmail)
             .ToString();

            return endpoint;
        }
    }
}

