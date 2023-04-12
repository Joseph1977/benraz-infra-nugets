namespace Benraz.Infrastructure.Phone.BenrazPhoneService
{
    /// <summary>
    /// Benraz Phone service settings.
    /// </summary>
    public class BenrazPhoneServiceSettings
    {
        /// <summary>
        /// Sms API key.
        /// </summary>
        public string AccountSID { get; set; }

        /// <summary>
        /// Auth Token.
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Out Twillio number.
        /// </summary>
        public string OutTwillionumber { get; set; }

        /// <summary>
        /// Base url.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}


