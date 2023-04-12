using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth
{
    /// <summary>
    /// Benraz auth gateway.
    /// </summary>
    public interface IBenrazAuthorizationAuthGateway
    {
        /// <summary>
        /// Sends auth parameters request.
        /// </summary>
        /// <param name="request">Auth parameters request.</param>
        /// <returns>Auth parameters response.</returns>
        Task<ParametersResponse> SendAsync(ParametersRequest request);

        /// <summary>
        /// Sends is token active verification request.
        /// </summary>
        /// <param name="request">Is token active request.</param>
        /// <returns>Is token active response.</returns>
        Task<IsTokenActiveResponse> SendAsync(IsTokenActiveRequest request);

        /// <summary>
        /// Sends token request.
        /// </summary>
        /// <param name="request">Token request.</param>
        /// <returns>Token response.</returns>
        Task<TokenResponse> SendAsync(TokenRequest request);

        /// <summary>
        /// Sends exchange token request.
        /// </summary>
        /// <param name="request">Exchange token request.</param>
        /// <returns>Token response.</returns>
        Task<TokenResponse> SendAsync(ExchangeTokenRequest request);

        /// <summary>
        /// Verify if a claim is associated with a role.
        /// </summary>
        /// <param name="request">Claim verify request.</param>
        /// <returns>Claim verify request.</returns>
        Task<ClaimVerifyResponse> SendAsync(ClaimVerifyRequest request);
    }
}



