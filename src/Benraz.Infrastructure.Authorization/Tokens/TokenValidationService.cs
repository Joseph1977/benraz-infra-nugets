using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Authorization.Tokens
{
    /// <summary>
    /// Token validation service.
    /// </summary>
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IBenrazAuthorizationAuthGateway _BenrazAuthGateway;
        private readonly IMemoryCache _memoryCache;
        private readonly TokenValidationServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="BenrazAuthGateway">Benraz authorization auth gateway.</param>
        /// <param name="memoryCache">Memory cache.</param>
        /// <param name="settings">Settings.</param>
        public TokenValidationService(
            IBenrazAuthorizationAuthGateway BenrazAuthGateway,
            IMemoryCache memoryCache,
            IOptions<TokenValidationServiceSettings> settings)
        {
            _BenrazAuthGateway = BenrazAuthGateway;
            _memoryCache = memoryCache;
            _settings = settings.Value;
        }

        /// <summary>
        /// Returns issuer signing keys.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="kid">Key identifier.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Signing keys.</returns>
        public IEnumerable<SecurityKey> IssuerSigningKeyResolver(
            string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            var signingKeyCacheKey = GetSigningKeyCacheKey(kid);

            var remoteSigningKey = _memoryCache.Get<SecurityKey>(signingKeyCacheKey);
            if (remoteSigningKey == null)
            {
                var parametersResponse = SendParametersRequest();

                var keySet = JsonConvert.DeserializeObject<JsonWebKeySet>(parametersResponse?.KeySet);
                if (keySet == null)
                {
                    return null;
                }

                remoteSigningKey = keySet.Keys.Where(y => y.KeyId == kid).FirstOrDefault();

                _memoryCache.Set(signingKeyCacheKey, remoteSigningKey);
            }

            return new List<SecurityKey> { remoteSigningKey };
        }

        /// <summary>
        /// Validates issuer.
        /// </summary>
        /// <param name="issuer">Issuer.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Issuer.</returns>
        public string IssuerValidator(
            string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var issuerCacheKey = GetIssuerCacheKey();

            var remoteIssuer = _memoryCache.Get<string>(issuerCacheKey);
            if (string.IsNullOrEmpty(remoteIssuer))
            {
                var parametersResponse = SendParametersRequest();
                remoteIssuer = parametersResponse?.Issuer;

                _memoryCache.Set(issuerCacheKey, remoteIssuer);
            }

            return remoteIssuer;
        }

        /// <summary>
        /// Validates audience.
        /// </summary>
        /// <param name="audiences">Audiences.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="validationParameters">Validation parameters.</param>
        /// <returns>Validation result.</returns>
        public bool AudienceValidator(
            IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (string.IsNullOrEmpty(_settings.Audience))
            {
                return false;
            }

            var allowedAudiences = _settings.Audience.Split(',');
            return audiences.Any(x => allowedAudiences.Contains(x));
        }

        /// <summary>
        /// Validates if a token is active.
        /// </summary>
        /// <param name="jti">Token identifier.</param>
        /// <returns>Validation result.</returns>
        public bool ActiveValidator(string jti)
        {
            if (string.IsNullOrEmpty(jti))
            {
                return true;
            }

            if (!Guid.TryParse(jti, out var tokenId))
            {
                return false;
            }

            var isTokenActiveResponse = SendIsTokenActiveRequest(new IsTokenActiveRequest { TokenId = tokenId });
            return isTokenActiveResponse?.IsActive ?? false;
        }

        private string GetSigningKeyCacheKey(string kid)
        {
            return $"signingkey_{kid}";
        }

        private string GetIssuerCacheKey()
        {
            return "issuer";
        }

        private ParametersResponse SendParametersRequest()
        {
            var response = Task.Run(() => _BenrazAuthGateway.SendAsync(new ParametersRequest()))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return response;
        }

        private IsTokenActiveResponse SendIsTokenActiveRequest(IsTokenActiveRequest request)
        {
            var response = Task.Run(() => _BenrazAuthGateway.SendAsync(request))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return response;
        }
    }
}




