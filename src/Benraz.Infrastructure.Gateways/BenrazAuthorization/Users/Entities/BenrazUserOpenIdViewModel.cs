using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Entities
{
    /// <summary>
    /// User open identifier view model.
    /// </summary>
    public class BenrazUserOpenIdViewModel
    {
        /// <summary>
        /// Sub (user identifier).
        /// </summary>
        [JsonProperty("sub")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Name (full name).
        /// </summary>
        [JsonProperty("name")]
        public string FullName { get; set; }

        /// <summary>
        /// Given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Family name.
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Picture.
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Email verified.
        /// </summary>
        public string EmailVerified { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Phone number verified.
        /// </summary>
        public string PhoneNumberVerified { get; set; }

        /// <summary>
        /// Locale.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Nick name.
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// Profile.
        /// </summary>
        [JsonProperty("profile")]
        public string Profile { get; set; }

        /// <summary>
        /// Zone information.
        /// </summary>
        [JsonProperty("zoneinfo")]
        public string ZoneInfo { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [JsonProperty("address")]
        public BenrazAddressViewModel Address { get; set; }

        /// <summary>
        /// Update time in UTC.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime? UpdateTimeUtc { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        [JsonProperty("role")]
        public List<string> Roles { get; set; }
    }
}


