namespace Benraz.Infrastructure.Gateways.BenrazSso.Messages
{
    /// <summary>
    /// Login response.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Callback URL.
        /// </summary>
        public string CallbackUrl { get; set; }
    }
}



