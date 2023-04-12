namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages
{
    /// <summary>
    /// Get action url response.
    /// </summary>
    public class GetActionUrlResponse : HttpResponseBase
    {
        /// <summary>
        /// Action URL.
        /// </summary>
        public string ActionUrl { get; set; }
    }
}


