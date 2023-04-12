namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.InternalLogin
{
    /// <summary>
    /// Authorization internal login gateway endpoints.
    /// </summary>
    public static class BenrazAuthorizationInternalLoginEndpoints
    {
        /// <summary>
        /// API version.
        /// </summary>
        public const string ApiVersion = "v1";

        /// <summary>
        /// Internal login endpoint.
        /// </summary>
        public static class InternalLogin
        {
            /// <summary>
            /// Base URL.
            /// </summary>
            private const string BaseUrl = ApiVersion + "/internal-login";

            /// <summary>
            /// Get.
            /// </summary>
            public const string Get = BaseUrl + "/{0}";

            /// <summary>
            /// Restore password.
            /// </summary>
            public const string RestorPassword = BaseUrl + "/restore-password";

            /// <summary>
            /// Send confirmation email.
            /// </summary>
            public const string SendConfirmationEmail = BaseUrl + "/send-confirmation-email";

            /// <summary>
            /// Get action url.
            /// </summary>
            public const string GetActionUrl = BaseUrl + "/get-action-url";
        }
    }
}

