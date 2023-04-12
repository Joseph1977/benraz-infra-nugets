using Microsoft.IdentityModel.Tokens;
using Benraz.Infrastructure.Common.AccessControl;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Benraz.Infrastructure.Authorization.Tokens
{
    /// <summary>
    /// Token validator.
    /// </summary>
    public class TokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ITokenValidationService _tokenValidationService;

        /// <summary>
        /// Returns true if token can be validated.
        /// </summary>
        public bool CanValidateToken => true;

        /// <summary>
        /// Gets and sets the maximum size in bytes, that a will be processed.
        /// </summary>
        public int MaximumTokenSizeInBytes { get; set; }

        /// <summary>
        /// Creates validator.
        /// </summary>
        /// <param name="tokenValidationService">Token validation service.</param>
        public TokenValidator(ITokenValidationService tokenValidationService)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationService = tokenValidationService;
            MaximumTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        }

        /// <summary>
        /// Returns true if the token can be read, false otherwise.
        /// </summary>
        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        /// <summary>
        /// Validates a token passed as a string using TokenValidationParameters.
        /// </summary>
        public ClaimsPrincipal ValidateToken(
            string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            if (principal == null || validatedToken == null)
            {
                throw new SecurityTokenException("Token is invalid.");
            }

            var jti = principal.FindFirst("jti");
            var isActive = _tokenValidationService.ActiveValidator(jti?.Value);
            if (!isActive)
            {
                throw new SecurityTokenException("Token is inactive.");
            }

            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CommonClaimTypes.ACCESS_TOKEN, securityToken));

            return principal;
        }
    }
}




