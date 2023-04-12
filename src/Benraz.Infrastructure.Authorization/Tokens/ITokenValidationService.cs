using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Authorization.Tokens
{
    /// <summary>
    /// Token validation service.
    /// </summary>
    public interface ITokenValidationService
    {
        /// <summary>
        /// Returns issuer signing keys.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="kid">Key identifier.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Signing keys.</returns>
        IEnumerable<SecurityKey> IssuerSigningKeyResolver(
            string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);

        /// <summary>
        /// Validates issuer.
        /// </summary>
        /// <param name="issuer">Issuer.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Issuer.</returns>
        string IssuerValidator(
            string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);

        /// <summary>
        /// Validates audience.
        /// </summary>
        /// <param name="audiences">Audiences.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Validation result.</returns>
        bool AudienceValidator(
            IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);

        /// <summary>
        /// Validates if a token is active.
        /// </summary>
        /// <param name="jti">Token identifier.</param>
        /// <returns>Validation result.</returns>
        bool ActiveValidator(string jti);
    }
}




