using Newtonsoft.Json;
using System;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Token request.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// SSO provider code.
        /// </summary>
        public int SsoProviderCode { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Grant type.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            return "v1/auth/token";
        }

        /// <summary>
        /// Returns request content in string format.
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}



