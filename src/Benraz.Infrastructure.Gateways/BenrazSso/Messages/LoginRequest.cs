using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Benraz.Infrastructure.Gateways.BenrazSso.Messages
{
    /// <summary>
    /// Login request.
    /// </summary>
    public class LoginRequest
    {
        private const string APPLICATION_ID_KEY = "applicationId";
        private const string RETURN_URL_KEY = "returnUrl";

        /// <summary>
        /// State.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Sets state.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="returnUrl">Return URL.</param>
        public void SetState(string applicationId, string returnUrl = null)
        {
            var values = new Dictionary<string, string>();
            values.Add(APPLICATION_ID_KEY, applicationId);
            values.Add(RETURN_URL_KEY, returnUrl);

            var valuePairs = new List<string>();
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value.Value))
                {
                    valuePairs.Add($"{value.Key}={value.Value}");
                }
            }

            var state = string.Join("&", valuePairs);
            State = WebUtility.UrlEncode(state);
        }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            return "v1/internal-login";
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



