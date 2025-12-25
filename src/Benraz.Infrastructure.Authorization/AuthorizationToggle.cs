using System;

namespace Benraz.Infrastructure.Authorization
{
    /// <summary>
    /// Provides a centralized switch that enables temporarily bypassing authorization checks.
    /// </summary>
    public static class AuthorizationToggle
    {
        private const string DisableAuthorizationVariableName = "DISABLE_AUTHORIZATION";

        /// <summary>
        /// Gets a value indicating whether authorization logic is enabled.
        /// Defaults to true when the environment variable is missing or invalid, so auth stays on by default.
        /// </summary>
        public static bool IsAuthorizationEnabled => !IsAuthorizationDisabled;

        /// <summary>
        /// Gets a value indicating whether authorization logic is disabled via environment configuration.
        /// </summary>
        public static bool IsAuthorizationDisabled => EvaluateAuthorizationDisabled();

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
