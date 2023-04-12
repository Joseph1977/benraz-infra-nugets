using Benraz.Infrastructure.Domain.Common;

namespace Benraz.Infrastructure.Gateways
{
    /// <summary>
    /// Benraz backend basic profile request.
    /// </summary>
    public abstract class BenrazBasicProfileRequest : HttpRequestBase
    {
        /// <summary>
        /// Profile.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}


