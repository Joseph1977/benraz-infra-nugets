using System;

namespace Benraz.Infrastructure.Authorization
{
    /// <summary>
    /// Provides a centralized, environment-based switch that can disable or enable authorization checks.
    /// </summary>
    public static class AuthorizationToggle
    {
        private const string DisableAuthorizationVariableName = "DISABLE_AUTHORIZATION";
        private static readonly Lazy<bool> IsAuthorizationDisabledLazy =
            new Lazy<bool>(EvaluateAuthorizationDisabled, isThreadSafe: true);

        /// <summary>
        /// Gets a value indicating whether authorization logic is enabled.
        /// Defaults to true when the environment variable is missing or invalid, so auth stays on by default.
        /// </summary>
        public static bool IsAuthorizationEnabled => !IsAuthorizationDisabled;

        /// <summary>
        /// Gets a value indicating whether authorization logic is disabled via environment configuration.
        /// </summary>
        public static bool IsAuthorizationDisabled => IsAuthorizationDisabledLazy.Value;

        private static bool EvaluateAuthorizationDisabled()
        {
            var rawValue = Environment.GetEnvironmentVariable(DisableAuthorizationVariableName);
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return false;
            }

            rawValue = rawValue.Trim();
            if (bool.TryParse(rawValue, out var parsedBool))
            {
                return parsedBool;
            }

            return string.Equals(rawValue, "1", StringComparison.OrdinalIgnoreCase);
        }
    }
}
