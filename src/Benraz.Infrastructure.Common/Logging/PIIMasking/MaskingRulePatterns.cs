using System.Collections.Generic;

namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// Mapped key based masking rule patterns.
    /// </summary>
    public static class MaskingRulePatterns
    {
        /// <summary>
        /// Patterns.
        /// </summary>
        public static readonly Dictionary<MaskingRuleName, string> Patterns = new()
        {
            { MaskingRuleName.AccessToken, "AccessToken|token|authToken" },
            { MaskingRuleName.ApiKey, "ApiKey|projectApiKey" },
            { MaskingRuleName.Email, "Email|EmailAddress|NormalizedEmail" },
            { MaskingRuleName.Phone, "Phone|PhoneNumber|tel" },
            { MaskingRuleName.UserName, "UserName|NormalizedUserName" },
            { MaskingRuleName.TaxId, "TaxId|TIN" },
            { MaskingRuleName.Fax, "Fax" },
            { MaskingRuleName.PostalCode, "PostalCode|ZipCode|PinCode" },
            { MaskingRuleName.SignedUrl, "SignedUrl|Url" }
        };
    }
}
