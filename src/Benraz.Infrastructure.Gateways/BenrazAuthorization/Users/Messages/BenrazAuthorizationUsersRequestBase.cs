namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Benraz Authorization users base request.
    /// </summary>
    public abstract class BenrazAuthorizationUsersRequestBase : HttpRequestBase
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}



