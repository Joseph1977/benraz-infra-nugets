using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Web.Authorization
{
    /// <summary>
    /// Authorization service extensions.
    /// </summary>
    public static class AuthorizationServiceExtensions
    {
        /// <summary>
        /// Returns if user authorized.
        /// </summary>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="user">User.</param>
        /// <param name="policyName">Policy name.</param>
        /// <returns>Authorization result.</returns>
        public static async Task<bool> IsAuthorizedAsync(
            this IAuthorizationService authorizationService, ClaimsPrincipal user, string policyName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(user, policyName);
            return authorizationResult.Succeeded;
        }
    }
}




