using Benraz.Infrastructure.Common.Logging.PIIMasking;
using NLog.LayoutRenderers;
using NLog.LayoutRenderers.Wrappers;

namespace Benraz.Infrastructure.Common.Logging
{
    /// <summary>
    /// NLog layout renderer wrapper that masks personally identifiable information (PII) 
    /// such as phone numbers, access token, token, phone and email addresses 
    /// from log messages. 
    /// Usage in NLog config:
    /// ${piimask:${message}}
    /// </summary>
    [LayoutRenderer("piimask")]
    public class PiiMaskingLayoutRenderer : WrapperLayoutRendererBase
    {
        /// <summary>
        /// Transforms the input log message by applying all configured PII masking rules.
        /// If the text is null or empty, it is returned unchanged.
        /// </summary>
        /// <param name="logMessage">Log Message.</param>
        /// <returns>Log Message.</returns>
        protected override string Transform(string logMessage)
        {
            if (string.IsNullOrEmpty(logMessage))
                return logMessage;

            // First priority: apply key-based masking rules 
            logMessage = MaskingRuleRegistry.KeyRules.ApplyAll(logMessage);

            // Second priority: apply regex-based masking rules 
            logMessage = MaskingRuleRegistry.RegexRules.ApplyAll(logMessage);

            return logMessage;
        }
    }
}
