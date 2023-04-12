using Newtonsoft.Json;
using System;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Benraz Authorization service user.
    /// </summary>
    public class BenrazAuthorizationUser
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Is email confirmed.
        /// </summary>
        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Is phone number confirmed.
        /// </summary>
        [JsonProperty("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Status code.
        /// </summary>
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Is SSO logins only.
        /// </summary>
        [JsonProperty("isSsoOnly")]
        public bool IsSsoOnly { get; set; }

        /// <summary>
        /// Count of failed access attempts.
        /// </summary>
        [JsonProperty("accessFailedCount")]
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Possibility to be lockout.
        /// </summary>
        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Date when lockout ends.
        /// </summary>
        [JsonProperty("lockoutEnd")]
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}



