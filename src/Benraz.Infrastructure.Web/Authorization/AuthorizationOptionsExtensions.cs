using Microsoft.AspNetCore.Authorization;
using Benraz.Infrastructure.Common.AccessControl;

namespace Benraz.Infrastructure.Web.Authorization
{
    /// <summary>
    /// Authorization options extensions.
    /// </summary>
    public static class AuthorizationOptionsExtensions
    {
        /// <summary>
        /// Add claims policy.
        /// </summary>
        /// <param name="options">Authorization options.</param>
        /// <param name="policyName">Policy name.</param>
        /// <param name="claimValues">Claim values.</param>
        public static void AddClaimsPolicy(
            this AuthorizationOptions options, string policyName, params string[] claimValues)
        {
            options.AddPolicy(policyName, builder => builder.RequireClaim(CommonClaimTypes.CLAIM, claimValues));
        }
    }
}




