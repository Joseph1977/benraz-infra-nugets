namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get user roles request.
    /// </summary>
    public class GetUserRolesRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = $"v1/users/{UserId}/roles";
            return endpoint;
        }
    }
}



