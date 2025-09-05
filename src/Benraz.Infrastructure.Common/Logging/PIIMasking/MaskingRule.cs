using System;
using System.Text.RegularExpressions;

namespace Benraz.Infrastructure.Common.Logging.PIIMasking
{
    /// <summary>
    /// Defines a rule for detecting and masking sensitive data.
    /// (e.g., emails, accesstoken etc...) in log messages.
    /// </summary>
    public class MaskingRule
    {
        /// <summary>
        /// Descriptive name of the masking rule (e.g., "Email", "Phone").
        /// </summary>
        public MaskingRuleName? Name { get; }

        /// <summary>
        /// Compiled regular expression used to detect sensitive information.
        /// </summary>
        public Regex Pattern { get; }

        /// <summary>
        /// Masking function applied to each regex match to return the masked value.
        /// </summary>
        public Func<Match, string> MaskFunc { get; }

        /// <summary>
        /// Masking rule.
        /// </summary>
        /// <param name="name">Descriptive name of the rule.</param>
        /// <param name="pattern">Regex pattern used to detect sensitive data.</param>
        /// <param name="maskFunc">Function that produces the masked replacement for each match.</param>
        public MaskingRule(MaskingRuleName? name, Regex pattern, Func<Match, string> maskFunc)
        {
            Name = name;
            Pattern = pattern;
            MaskFunc = maskFunc;
        }

        /// <summary>
        /// Applies the masking rule to the specified input string.
        /// </summary>
        /// <param name="input">Input text that may contain sensitive information.</param>
        /// <returns>Input string with matches replaced by masked values.</returns>
        public string Apply(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return Pattern.Replace(input, m => MaskFunc(m));
        }
    }
}
