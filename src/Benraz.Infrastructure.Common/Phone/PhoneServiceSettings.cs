namespace Benraz.Infrastructure.Common.Phone
{
    /// <summary>
    /// Phone service settings.
    /// </summary>
    public class PhoneServiceSettings
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
    }
}


