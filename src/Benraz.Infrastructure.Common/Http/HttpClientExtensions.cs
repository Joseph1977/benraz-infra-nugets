using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Http
{
    /// <summary>
    /// HTTP client extensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends object as JSON with POST method.
        /// </summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <returns>HTTP response message.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync(
            this HttpClient httpClient, string requestUri, object request)
        {
            return httpClient.PostAsync(requestUri, CreateJsonContent(request));
        }

        /// <summary>
        /// Sends object as JSON with POST method.
        /// </summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <returns>HTTP response message.</returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync(
            this HttpClient httpClient, Uri requestUri, object request)
        {
            return httpClient.PostAsync(requestUri, CreateJsonContent(request));
        }

        /// <summary>
        /// Sends object as JSON with PUT method.
        /// </summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <returns>HTTP response message.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync(
            this HttpClient httpClient, string requestUri, object request)
        {
            return httpClient.PutAsync(requestUri, CreateJsonContent(request));
        }

        /// <summary>
        /// Sends object as JSON with PUT method.
        /// </summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="request">Request.</param>
        /// <returns>HTTP response message.</returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync(
            this HttpClient httpClient, Uri requestUri, object request)
        {
            return httpClient.PutAsync(requestUri, CreateJsonContent(request));
        }

        private static HttpContent CreateJsonContent(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }
    }
}




