using Benraz.Infrastructure.Gateways.BenrazServices.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazServices
{
    /// <summary>
    /// Benraz services gateway.
    /// </summary>
    public interface IBenrazServicesGateway
    {
        /// <summary>
        /// Sends email request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        Task<EmailResponse> SendAsync(EmailV2Request request);

        /// <summary>
        /// Sends Phone request.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Response.</returns>
        Task<PhoneResponse> SendSmsAsync(PhoneRequest request);
    }
}



