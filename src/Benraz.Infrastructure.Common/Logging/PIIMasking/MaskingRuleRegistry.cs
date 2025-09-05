using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// Provides a central registry of predefined masking rules for
    /// common types of personally identifiable information (PII).
    /// </summary>
    public static class MaskingRuleRegistry
    {
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        /// <summary>
        /// Key based masking rules for PII information.
        /// </summary>
        public static readonly List<MaskingRule> KeyRules = new()
        {
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.AccessToken, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.AccessToken]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.ApiKey, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.ApiKey]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.Email, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.Email]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.UserName, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.UserName]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.TaxId, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.TaxId]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.Fax, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.Fax]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.PostalCode, patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.PostalCode]),
            MaskingRuleFactory.CreateKeyRule(MaskingRuleName.Phone, null,null,
                customRegex: new Regex(@"""?(?<key>" + MaskingRulePatterns.Patterns[MaskingRuleName.Phone] + @")""?(\s*[:=]\s*)([""']?)(?<value>[^""'\s,}]+(?:[\s\-]?[0-9()+]*)*)([""']?)",
                DefaultOptions),
                match =>{
                            if (match.Groups.Count >= 1)
                            {
                                var keyPart    = match.Groups["key"].Value;
                                var separator  = match.Groups[1].Value;
                                var openQuote  = match.Groups[2].Value;
                                var closeQuote = match.Groups[3].Value;
                                return $"{openQuote}{keyPart}{closeQuote}{separator}{openQuote}***-***-****{closeQuote}";
                            }
                            return "***-***-****";
                        }
                ),
            
            MaskingRuleFactory.CreateKeyRule(
                MaskingRuleName.SignedUrl,
                patternGroup: MaskingRulePatterns.Patterns[MaskingRuleName.SignedUrl],
                mask: null,
                customRegex: new Regex(
                    @"(?<url>https?://[^\s""]+)\?(X-Goog-[^""\s]*)",
                    DefaultOptions
                ),
                maskFunc: m => m.Groups["url"].Value
            ),
        };

        /// <summary>
        /// Regex based masking rules for PII information (no key, just match → mask).
        /// </summary>
        public static readonly List<MaskingRule> RegexRules = new()
        {
            MaskingRuleFactory.CreateRegexRule(PIIRegex.JWTRegex),
            MaskingRuleFactory.CreateRegexRule(PIIRegex.EmailRegex),
            MaskingRuleFactory.CreateRegexRule(PIIRegex.MFACodeRegex)
        };

        /// <summary>
        /// Applies all making rules.
        /// </summary>
        /// <param name="rules">Rules.</param>
        /// <param name="input">Input.</param>
        /// <returns>String.</returns>
        public static string ApplyAll(this IEnumerable<MaskingRule> rules, string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string message = input;
            foreach (var rule in rules)
            {
                try
                {
                    message = rule.Apply(message);
                }
                catch
                {
                    // Continue masking with remaining rules
                }
            }
            return message;
        }
    }
}
