using Benraz.Infrastructure.Common.Http;
using Benraz.Infrastructure.Domain.Common;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages
{
    /// <summary>
    /// Get action url request.
    /// </summary>
    public class GetActionUrlRequest : BenrazAuthorizationInternalLoginRequestBase
    {
        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Action type.
        /// </summary>
        public EmailActionType ActionType { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
             .SetBaseEndpoint(BenrazAuthorizationInternalLoginEndpoints.InternalLogin.GetActionUrl)
             .ToString();

            return endpoint;
        }
    }
}


