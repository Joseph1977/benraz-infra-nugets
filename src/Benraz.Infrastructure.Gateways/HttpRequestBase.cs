using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Benraz.Infrastructure.Gateways
{
    /// <summary>
    /// HTTP request base.
    /// </summary>
    public abstract class HttpRequestBase
    {
        /// <summary>
        /// Returns JSON content.
        /// </summary>
        /// <returns>JSON HTTP content.</returns>
        protected HttpContent GetJsonContent()
        {
            return GetJsonContent(this);
        }

        /// <summary>
        /// Returns JSON content.
        /// </summary>
        /// <param name="obj">Object to convert to JSON.</param>
        /// <returns>JSON HTTP content.</returns>
        protected HttpContent GetJsonContent(object obj)
        {
            var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(obj, jsonSettings);

            return GetContent("application/json", json);
        }

        /// <summary>
        /// Returns URL-encoded form content.
        /// </summary>
        /// <param name="contentValue">Content value.</param>
        /// <returns>URL-encoded form HTTP content.</returns>
        protected HttpContent GetUrlEncodedFormContent(string contentValue)
        {
            return GetContent("application/x-www-form-urlencoded", contentValue);
        }

        /// <summary>
        /// Returns content.
        /// </summary>
        /// <param name="contentType">Content type.</param>
        /// <param name="contentValue">Content value.</param>
        /// <returns>HTTP content.</returns>
        protected HttpContent GetContent(string contentType, string contentValue)
        {
            var content = new StringContent(contentValue);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return content;
        }
    }
}



