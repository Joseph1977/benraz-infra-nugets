namespace Benraz.Infrastructure.Common.Emails
{
    /// <summary>
    /// Email service settings.
    /// </summary>
    public class EmailServiceSettings
    {
        /// <summary>
        /// SMTP host name.
        /// </summary>
        public string SmtpHostName { get; set; }

        /// <summary>
        /// SMTP port.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Is SSL enabled.
        /// </summary>
        public bool IsSslEnabled { get; set; }

        /// <summary>
        /// Default message sender.
        /// </summary>
        public string DefaultSenderAddress { get; set; }
    }
}




