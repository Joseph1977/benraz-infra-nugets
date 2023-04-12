using Benraz.Infrastructure.Phone.BenrazPhoneService;

namespace Benraz.Infrastructure.Phone
{
    /// <summary>
    /// Phone service provider settings.
    /// </summary>
    public class PhoneServiceProviderSettings
    {
        /// <summary>
        /// Phone services type.
        /// </summary>
        public PhoneServiceType ServiceType { get; set; }

        /// <summary>
        /// Benraz phone service settings.
        /// </summary>
        public BenrazPhoneServiceSettings Benraz { get; set; }
    }
}


