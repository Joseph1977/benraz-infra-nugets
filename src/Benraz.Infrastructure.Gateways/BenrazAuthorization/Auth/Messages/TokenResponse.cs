namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages
{
    /// <summary>
    /// Token response.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        public string Error { get; set; }
    }
}



