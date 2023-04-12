using Benraz.Infrastructure.Common.Paging;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get users response.
    /// </summary>
    public class GetUsersResponse : HttpResponseBase
    {
        /// <summary>
        /// Users page.
        /// </summary>
        public Page<BenrazAuthorizationUser> Page { get; set; }
    }
}



