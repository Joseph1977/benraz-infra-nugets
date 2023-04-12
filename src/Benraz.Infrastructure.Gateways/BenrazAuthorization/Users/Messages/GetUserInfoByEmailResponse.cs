using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Entities;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user info by email response.
    /// </summary>
    public class GetUserInfoByEmailResponse : HttpResponseBase
    {
        /// <summary>
        /// User.
        /// </summary>
        public BenrazUserOpenIdViewModel User { get; set; }
    }
}


