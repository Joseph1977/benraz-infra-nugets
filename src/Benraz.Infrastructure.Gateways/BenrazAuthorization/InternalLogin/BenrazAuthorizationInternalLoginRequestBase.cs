namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin
{
    /// <summary>
    /// Authorization internal login request base.
    /// </summary>
    public class BenrazAuthorizationInternalLoginRequestBase : HttpRequestBase
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}

