using Newtonsoft.Json;

namespace Benraz.Infrastructure.Gateways.BenrazServices.Messages
{
    /// <summary>
    /// Email basic information.
    /// </summary>
    public class EmailBasicInfo
    {
        /// <summary>
        /// From email address.
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        /// <summary>
        /// From display name.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// To email address.
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// To email addresses.
        /// </summary>
        [JsonProperty("tos")]
        public string[] Tos { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Template identifier.
        /// </summary>
        [JsonProperty("tempId")]
        public string TemplateId { get; set; }

        /// <summary>
        /// Skip opt out check.
        /// </summary>
        [JsonProperty("skipOptOutCheck")]
        public bool SkipOptOutCheck { get; set; }
    }
}



