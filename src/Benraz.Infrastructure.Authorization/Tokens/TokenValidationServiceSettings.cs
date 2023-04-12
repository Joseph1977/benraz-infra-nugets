namespace Benraz.Infrastructure.Authorization.Tokens
{
    /// <summary>
    /// Token validation service settings.
    /// </summary>
    public class TokenValidationServiceSettings
    {
        /// <summary>
        /// Audience.
        /// </summary>
        /// <remarks>
        /// Could accept several comma-separated audiences.
        /// </remarks>
        public string Audience { get; set; }
    }
}




