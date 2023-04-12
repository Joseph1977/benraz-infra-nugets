using System.Collections.Generic;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user roles response.
    /// </summary>
    public class GetUserRolesResponse : HttpResponseBase
    {
        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }
    }
}



