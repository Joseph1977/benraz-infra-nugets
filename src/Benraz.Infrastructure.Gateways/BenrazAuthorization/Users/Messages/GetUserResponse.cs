namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user response.
    /// </summary>
    public class GetUserResponse : HttpResponseBase
    {
        /// <summary>
        /// User.
        /// </summary>
        public BenrazAuthorizationUser User { get; set; }
    }
}



