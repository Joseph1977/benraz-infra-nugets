using Newtonsoft.Json;

namespace Benraz.Infrastructure.Gateways.BenrazServices.Messages
{
    /// <summary>
    /// Benraz services base request.
    /// </summary>
    public abstract class BenrazServicesRequestBase : HttpRequestBase
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}



