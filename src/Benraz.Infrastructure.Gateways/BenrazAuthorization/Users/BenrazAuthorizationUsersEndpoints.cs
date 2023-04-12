namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users
{
    /// <summary>
    /// Users gateway endpoints.
    /// </summary>
    public static class BenrazAuthorizationUsersEndpoints
    {
        /// <summary>
        /// API version.
        /// </summary>
        public const string ApiVersion = "v1";

        /// <summary>
        /// Users endpoint.
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// Base URL.
            /// </summary>
            private const string BaseUrl = ApiVersion + "/users";

            /// <summary>
            /// Get user by identifier.
            /// </summary>
            public const string GetByUserId = BaseUrl + "/{0}";

            /// <summary>
            ///Get user by email.
            /// </summary>
            public const string GetByEmail = BaseUrl + "/userInfoByEmail";
        }
    }
}


