using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Phone
{
    /// <summary>
    /// Phone service.
    /// </summary>
    public interface IPhoneService
    {
        /// <summary>
        /// Sends sms.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="body">Body.</param>
        Task SendSmsAsync(string sender, List<string> recipients, string body);
    }
}


