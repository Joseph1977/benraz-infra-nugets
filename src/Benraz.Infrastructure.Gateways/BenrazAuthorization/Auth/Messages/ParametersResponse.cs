using Newtonsoft.Json;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Auth parameters response.
    /// </summary>
    public class ParametersResponse
    {
        /// <summary>
        /// Key set in JWKS format.
        /// </summary>
        [JsonProperty("keySet")]
        public string KeySet { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
}



