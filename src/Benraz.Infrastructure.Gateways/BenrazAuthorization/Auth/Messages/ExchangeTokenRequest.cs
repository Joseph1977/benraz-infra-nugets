using Newtonsoft.Json;
using System;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Exchange token request.
    /// </summary>
    public class ExchangeTokenRequest
    {
        /// <summary>
        /// Access token.
        /// </summary>
        [JsonIgnore]
        public string AccessToken { get; set; }

        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Access token to exchange.
        /// </summary>
        [JsonProperty("AccessToken")]
        public string AccessTokenToExchange { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            return "v1/auth/token-exchange";
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



