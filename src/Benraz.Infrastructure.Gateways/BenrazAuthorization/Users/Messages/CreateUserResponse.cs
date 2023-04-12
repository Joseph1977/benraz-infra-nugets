namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Create user response.
    /// </summary>
    public class CreateUserResponse : HttpResponseBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }
    }
}



