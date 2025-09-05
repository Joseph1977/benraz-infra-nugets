namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// PII key based Masking rule name.
    /// </summary>
    public enum MaskingRuleName
    {
        /// <summary>
        /// Access token.
        /// </summary>
        AccessToken,

        /// <summary>
        /// API key.
        /// </summary>
        ApiKey,

        /// <summary>
        /// Email
        /// </summary>
        Email,

        /// <summary>
        /// Phone
        /// </summary>
        Phone,

        /// <summary>
        /// User name
        /// </summary>
        UserName,

        /// <summary>
        /// Tax identifier.
        /// </summary>
        TaxId,

        /// <summary>
        /// Fax.
        /// </summary>
        Fax,

        /// <summary>
        /// Postal code.
        /// </summary>
        PostalCode,

        /// <summary>
        /// Signed URL.
        /// </summary>
        SignedUrl
    }
}
