using System;
using System.Text.RegularExpressions;

namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// Masking rule factory.
    /// </summary>
    public static class MaskingRuleFactory
    {
        /// Default mask string used to replace sensitive values when masked.
        private const string DefaultMaskString = "***";
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        /// <summary>
        /// Create key based masking rule.
        /// </summary>
        /// <param name="name">Masking rule name.</param>
        /// <param name="patternGroup">Pattern group.</param>
        /// <param name="mask">Mask pattern (like *** or ***-***-****).</param>
        /// <param name="customRegex">Custom regex.</param>
        /// <param name="maskFunc">Mask function.</param>
        /// <returns>Masking rule.</returns>
        public static MaskingRule CreateKeyRule(
            MaskingRuleName? name,
            string? patternGroup = null,
            string? mask = null,
            Regex? customRegex = null,
            Func<Match, string>? maskFunc = null)
        {
            Regex regex;

            if (customRegex != null)
            {
                // Use custom regex
                regex = customRegex;
            }
            else
            {
                regex = new Regex($@"(""?({patternGroup})""?\s*[:=]\s*)([""]?)([^""'\s,}}]+)([""]?)", DefaultOptions);
            }

            // Use maskFunc if provided, otherwise default mask function
            maskFunc ??= GetDefaultMaskFunc(regex, mask);

            return new MaskingRule(name, regex, maskFunc);
        }


        /// <summary>
        /// Creates the regex based masking rule (no key, just match → mask).
        /// </summary>
        /// <param name="regularExpression">Regular expression.</param>
        /// <param name="maskFunc">Mask function.</param>
        /// <param name="mask">Mask pattern (like *** or ***-***-****).</param>
        /// <returns>Masking rule.</returns>
        public static MaskingRule CreateRegexRule(
            string regularExpression,
            Func<Match, string>? maskFunc = null,
            string? mask = null)
        {
            Regex regex = new Regex(regularExpression, DefaultOptions);

            // Use maskFunc if provided, otherwise default mask function
            maskFunc ??= (_ => mask ?? DefaultMaskString);

            return new MaskingRule(null, regex, maskFunc);
        }

        private static Func<Match, string> GetDefaultMaskFunc(Regex regex, string? mask = null)
        {
            return match =>
            {
                // If regex has a capturing group (Group 1), keep key and mask value
                if (match.Groups.Count > 1)
                {
                    var keyPart = match.Groups[1].Value;       // key + separator + whitespace
                    var openQuote = match.Groups[3].Value;     // opening quote if present
                    var value = match.Groups[4].Value;         // actual value
                    var closeQuote = match.Groups[5].Value;    // closing quote if present

                    // If null → always wrap with quotes
                    return string.Equals(value, "null", StringComparison.OrdinalIgnoreCase)
                        ? $"{keyPart}\"{mask ?? DefaultMaskString}\""
                        : $"{keyPart}{openQuote}{mask ?? DefaultMaskString}{closeQuote}";
                }

                // Otherwise, mask entire match
                return mask ?? DefaultMaskString;
            };
        }
    }
}
