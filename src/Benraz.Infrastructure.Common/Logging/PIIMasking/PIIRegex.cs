namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// Provides regular expressions for detecting and masking 
    /// personally identifiable information (PII) such as emails, 
    /// JWT, or MFA Code in log messages.
    /// </summary>
    public static class PIIRegex
    {
        /// <summary>
        /// JWT.
        /// </summary>
        public static string JWTRegex = @"\b[A-Za-z0-9_-]{10,}\.[A-Za-z0-9_-]{10,}\.[A-Za-z0-9_-]{10,}\b";

        /// <summary>
        /// Email regex.
        /// </summary>
        public static string EmailRegex = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";

        /// <summary>
        /// MFA code regex.
        /// </summary>
        public static string MFACodeRegex = @"\b\d{6}\b";
    }
}
