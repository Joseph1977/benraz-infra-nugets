using System;

namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// Principal service extensions.
    /// </summary>
    public static class PrincipalServiceExtensions
    {
        /// <summary>
        /// Returns user identifier.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <returns>User identifier.</returns>
        public static Guid? GetUserId(this IPrincipalService principalService)
        {
            var userIdString = principalService.GetUserIdString();
            return Guid.TryParse(userIdString, out var userId) ? (Guid?)userId : null;
        }

        /// <summary>
        /// Returns current user identifier.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <returns>Current user identifier.</returns>
        public static string GetCurrentUserId(this IPrincipalService principalService)
        {
            var userId = principalService.GetUserIdString();
            return userId;
        }

        /// <summary>
        /// Returns current access token.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <returns>Current access token.</returns>
        public static string GetCurrentAccessToken(this IPrincipalService principalService)
        {
            var accessToken = principalService.GetClaimValue(CommonClaimTypes.ACCESS_TOKEN);
            return accessToken;
        }

        /// <summary>
        /// Returns current claim value by claim type.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <param name="claimType">Claim type.</param>
        /// <returns></returns>
        public static string GetCurrentClaimValue(this IPrincipalService principalService, string claimType)
        {
            var claimValue = principalService.GetClaimValue(claimType);
            return claimValue;
        }

        private static string GetUserIdString(this IPrincipalService principalService)
        {
            return principalService.GetClaimValue(CommonClaimTypes.USER_ID);
        }

        private static string GetClaimValue(this IPrincipalService principalService, string claimType)
        {
            return principalService.GetPrincipal()?.FindFirst(claimType)?.Value;
        }
    }
}




