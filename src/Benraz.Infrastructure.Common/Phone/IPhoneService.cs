using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Phone
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
        /// <returns>Task.</returns>
        Task SendSms(string sender, List<string> recipients, string body);
    }
}


