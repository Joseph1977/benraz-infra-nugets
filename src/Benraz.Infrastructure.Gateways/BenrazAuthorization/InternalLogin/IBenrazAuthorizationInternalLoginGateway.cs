using Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin
{
    /// <summary>
    /// Authorization internal login gateway.
    /// </summary>
    public interface IBenrazAuthorizationInternalLoginGateway
    {
        /// <summary>
        /// Send confirmation email request.
        /// </summary>
        /// <param name="request">Send confirmation email request.</param>
        /// <returns>Send confirmation email response.</returns>
        Task<SendConfirmationEmailResponse> SendAsync(SendConfirmationEmailRequest request);

        /// <summary>
        /// Send restore password request.
        /// </summary>
        /// <param name="request">Restore password request.</param>
        /// <returns>Restore password response.</returns>
        Task<RestorePasswordResponse> SendAsync(RestorePasswordRequest request);

        /// <summary>
        /// Get action URL request.
        /// </summary>
        /// <param name="request">Get action URL request.</param>
        /// <returns>Get action URL response.</returns>
        Task<GetActionUrlResponse> SendAsync(GetActionUrlRequest request);
    }
}

