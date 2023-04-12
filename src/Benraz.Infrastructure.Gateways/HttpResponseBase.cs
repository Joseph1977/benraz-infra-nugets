namespace Benraz.Infrastructure.Gateways
{
    /// <summary>
    /// HTTP response base.
    /// </summary>
    public abstract class HttpResponseBase
    {
        /// <summary>
        /// HTTP response message status code.
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// HTTP response content string.
        /// </summary>
        public string HttpContentString { get; set; }

        /// <summary>
        /// Returns whether HTTP statis code indicates success.
        /// </summary>
        public bool IsSuccessHttpStatusCode
        {
            get { return HttpStatusCode >= 200 && HttpStatusCode < 300; }
        }
    }
}



