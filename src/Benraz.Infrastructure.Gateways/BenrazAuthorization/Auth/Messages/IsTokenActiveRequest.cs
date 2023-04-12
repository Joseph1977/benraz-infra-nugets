using System;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Is token active request.
    /// </summary>
    public class IsTokenActiveRequest
    {
        /// <summary>
        /// Token identifier.
        /// </summary>
        public Guid TokenId { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            return $"v1/auth/token/{TokenId}/is-active";
        }
    }
}



