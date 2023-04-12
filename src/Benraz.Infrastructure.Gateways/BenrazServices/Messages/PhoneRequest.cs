using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Benraz.Infrastructure.Gateways.BenrazServices.Messages
{
    /// <summary>
    /// Phone request.
    /// </summary>
    public class PhoneRequest : BenrazServicesRequestBase
    {
        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Recipients.
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        /// Creates request.
        /// </summary>
        public PhoneRequest()
        {
        }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = "api/Phone";
            return endpoint;
        }

        /// <summary>
        /// Returns request content.
        /// </summary>
        /// <returns>Request content.</returns>
        public HttpContent GetContent()
        {
            var jsonSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            var json = JsonConvert.SerializeObject(this, jsonSettings);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }
    }
}


