using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Benraz.Infrastructure.Common.AccessControl;
using Microsoft.IdentityModel.Tokens;

namespace Benraz.Infrastructure.Authorization.ClaimValidators
{
    /// <summary>  
    /// Claim validator.  
    /// </summary>  
    public class ClaimValidator
    {
        /// <summary>
        /// To search claims in a JWT using predefined matching and filtering logic while optionally validating token expiration and audience.
        /// </summary>
        /// <param name="jwtToken">JWT token.</param>  
        /// <param name="desiredClaims">Desired claims.</param>  
        /// <param name="matchType">Match type.</param>  
        /// <param name="filterType">Filter Type.</param>  
        /// <returns></returns>
        public bool SearchClaims(string jwtToken,
                    IEnumerable<string> desiredClaims,
                    ClaimMatchType matchType,
                    ClaimFilterType filterType
                    )
        {
            return ValidateClaims(jwtToken, desiredClaims, matchType, filterType, ClaimType.Claim);
        }

        /// <summary>
        /// To search roles in a JWT using predefined matching and filtering logic while optionally validating token expiration and audience.
        /// </summary>
        /// <param name="jwtToken">JWT token.</param>  
        /// <param name="desiredClaims">Desired claims.</param>  
        /// <param name="matchType">Match type.</param>  
        /// <param name="filterType">Filter Type.</param>  
        /// <returns></returns>
        public bool SearchRoles(string jwtToken,
                    IEnumerable<string> desiredClaims,
                    ClaimMatchType matchType,
                    ClaimFilterType filterType)
        {
            return ValidateClaims(jwtToken, desiredClaims, matchType, filterType, ClaimType.Role);
        }

        /// <summary>
        /// Check if the desired claims are included in the JWT based on the specified MatchType and FilterType criteria.  
        /// </summary>
        /// <param name="jwtToken">JWT token.</param>  
        /// <param name="desiredClaims">Desired claims.</param>  
        /// <param name="matchType">Match type.</param>  
        /// <param name="filterType">Filter Type.</param>  
        /// <param name="claimType">Claim type.</param>  
        /// <param name="isValidateTokenExpiration">Is validate token expiration.</param>  
        /// <param name="validAudiences">Valid audiences.</param>  
        public bool ValidateClaims(
                    string jwtToken,
                    IEnumerable<string> desiredClaims,
                    ClaimMatchType matchType,
                    ClaimFilterType filterType,
                    ClaimType claimType = ClaimType.Claim,
                    bool isValidateTokenExpiration = false,
                    string validAudiences = null)
        {

            //If the token is null or empty, or desired claims are null.  
            if (string.IsNullOrWhiteSpace(jwtToken) || desiredClaims is null)
                return false;

            // Remove "Bearer " prefix if present.  
            var tokenParts = jwtToken.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            jwtToken = tokenParts.Length == 2 ? tokenParts[1].Trim() : jwtToken;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(jwtToken);

            // Check expiration
            if (isValidateTokenExpiration)
            {
                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (!long.TryParse(expClaim, out var expUnix))
                    throw new SecurityTokenException("Token 'exp' claim is missing or invalid.");

                var expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                if (DateTime.UtcNow > expiration)
                    throw new SecurityTokenExpiredException("JWT token has expired.");
            }

            // Check audience
            if (validAudiences != null)
            {
                var audClaims = jwt.Audiences;
                var expectedAudiences = validAudiences.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (!audClaims.Any(a => expectedAudiences.Any(ea => string.Equals(a, ea, StringComparison.OrdinalIgnoreCase))))
                    throw new SecurityTokenInvalidAudienceException("JWT token audience is invalid.");
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));

            // It filters the claims where the claim type matches CLAIM or Both (case-insensitive)
            var userClaims = new List<string>();
            if (claimType == ClaimType.Claim || claimType == ClaimType.Both)
            {
                userClaims.AddRange(
                    principal.Claims
                        .Where(c => c.Type.Equals(CommonClaimTypes.CLAIM, StringComparison.OrdinalIgnoreCase))
                        .Select(c => c.Value.ToUpperInvariant())
                );
            }

            // It filters the role where the claim type matches Role or Both (case-insensitive)
            if (claimType == ClaimType.Role || claimType == ClaimType.Both)
            {
                userClaims.AddRange(
                    principal.Claims
                        .Where(c => c.Type.Equals(CommonClaimTypes.ROLE, StringComparison.OrdinalIgnoreCase))
                        .Select(c => c.Value.ToUpperInvariant())
                );
            }

            var normalizedDesiredClaims = desiredClaims.Select(c => c.ToUpperInvariant()).ToList();

            Func<string, string, bool> matchFunc = filterType switch
            {
                ClaimFilterType.Exact => (claim, desired) => claim == desired,
                ClaimFilterType.Include => (claim, desired) => claim.Contains(desired),
                _ => throw new ArgumentOutOfRangeException(nameof(filterType))
            };

            bool result = matchType switch
            {
                ClaimMatchType.All => normalizedDesiredClaims.All(dc => userClaims.Any(uc => matchFunc(uc, dc))),
                ClaimMatchType.AtLeastOne => normalizedDesiredClaims.Any(dc => userClaims.Any(uc => matchFunc(uc, dc))),
                _ => false
            };

            return result;
        }
    }
}
