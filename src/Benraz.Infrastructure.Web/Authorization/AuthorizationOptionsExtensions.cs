using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Benraz.Infrastructure.Authorization;
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
            options.AddPolicy(
                policyName,
                builder => builder.RequireAssertion(context =>
                {
                    if (AuthorizationToggle.IsAuthorizationDisabled)
                    {
                        return true;
                    }

                    if (claimValues == null || claimValues.Length == 0)
                    {
                        return false;
                    }

                    return claimValues.Any(value => context.User.HasClaim(CommonClaimTypes.CLAIM, value));
                }));
        }
    }
}




