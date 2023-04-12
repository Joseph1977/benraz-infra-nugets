using Benraz.Infrastructure.Emails.BenrazEmailsService;

namespace Benraz.Infrastructure.Emails
{
    /// <summary>
    /// Emails service provider settings.
    /// </summary>
    public class EmailsServiceProviderSettings
    {
        /// <summary>
        /// Emails services type.
        /// </summary>
        public EmailsServiceType ServiceType { get; set; }

        /// <summary>
        /// Benraz emails service settings.
        /// </summary>
        public BenrazEmailsServiceSettings Benraz { get; set; }
    }
}



