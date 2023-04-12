using Benraz.Infrastructure.Gateways.BenrazSso.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazSso
{
    /// <summary>
    /// Benraz SSO gateway.
    /// </summary>
    public interface IBenrazSsoGateway
    {
        /// <summary>
        /// Sends login request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        Task<LoginResponse> SendAsync(LoginRequest request);
    }
}



