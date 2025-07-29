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
        /// Validate claims (second level claim checker).  
        /// </summary>  
        /// <param name="jwtToken">JWT token.</param>  
        /// <param name="desiredClaims">Desired claims.</param>  
        /// <param name="matchType">Match type.</param>  
        /// <param name="filterType">Filter Type.</param>  
        /// <param name="isValidateTokenExpiration">Is validate token expiration.</param>  
        /// <param name="validAudiences">Valid audiences.</param>  
        /// <returns>True/False.</returns>  
        public bool ValidateClaims(
                    string jwtToken,
                    IEnumerable<string> desiredClaims,
                    ClaimMatchType matchType,
                    ClaimFilterType filterType,
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
                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (!long.TryParse(expClaim, out var expUnix))
                    throw new SecurityTokenException("Token 'exp' claim is missing or invalid.");

                var expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                if (DateTime.UtcNow > expiration)
                    throw new SecurityTokenExpiredException("JWT token has expired.");
            }

            // Check audience
            if (validAudiences != null)
            {
                var audClaims = jwt.Audiences.Select(a => a.ToUpperInvariant());
                var expectedAudiences = validAudiences.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                        .Select(a => a.ToUpperInvariant()).ToList();

                if (!audClaims.Any(a => expectedAudiences.Contains(a)))
                    throw new SecurityTokenInvalidAudienceException("JWT token audience is invalid.");
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));
            var userClaims = principal.Claims.Where(c => c.Type == CommonClaimTypes.CLAIM).Select(c => c.Value.ToUpperInvariant()).ToList();
            var normalizedDesiredClaims = desiredClaims.Select(c => c.ToUpperInvariant()).ToList();

            // Filter claims based on the filter type.
            Func<string, string, bool> matchFunc = filterType switch
            {
                ClaimFilterType.Exact => (claim, desiredClaim) => claim == desiredClaim,
                ClaimFilterType.Include => (claim, desiredClaim) => claim.Contains(desiredClaim),
                _ => throw new ArgumentOutOfRangeException(nameof(filterType))
            };

            // Determine if the claims match based on the match type.
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
