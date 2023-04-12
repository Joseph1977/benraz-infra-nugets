using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Benraz.Infrastructure.Gateways.BenrazServices.Messages
{
    /// <summary>
    /// Email request.
    /// </summary>
    public class EmailRequest : BenrazServicesRequestBase
    {
        /// <summary>
        /// Acount identifier.
        /// </summary>
        [JsonProperty("AccountId")]
        public int? AccountId { get; set; }

        /// <summary>
        /// Is corporate.
        /// </summary>
        [JsonProperty("IsCorporate")]
        public bool? IsCorporate { get; set; }

        /// <summary>
        /// Basic information.
        /// </summary>
        [JsonProperty("basicInfo")]
        public EmailBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// Email parameters.
        /// </summary>
        /// <remarks>Null by default.</remarks>
        [JsonProperty("emailParams")]
        public IDictionary<string, string> EmailParams { get; set; }

        /// <summary>
        /// White label parameters.
        /// </summary>
        /// <remarks>Null by default.</remarks>
        [JsonProperty("whiteLabelParams")]
        public IDictionary<string, string> WhileLabelParams { get; set; }

        /// <summary>
        /// Creates request.
        /// </summary>
        public EmailRequest()
        {
        }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = "v1/Emails";
            return endpoint;
        }

        /// <summary>
        /// Returns request content.
        /// </summary>
        /// <returns>Request content.</returns>
        public HttpContent GetContent()
        {
            var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(this, jsonSettings);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }
    }
}



