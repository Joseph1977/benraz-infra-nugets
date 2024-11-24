using Google.Apis.Auth.OAuth2;
using System;

namespace Benraz.Infrastructure.Gateways.GCP
{
    /// <summary>
    /// Google credentials loader.
    /// </summary>
    public class GoogleCredentialsLoader
    {
        /// <summary>
        /// Get google credential from environment.
        /// </summary>
        /// <param name="envVarName">Environment variable name.</param>
        /// <returns>Google credential.</returns>
        public static GoogleCredential GetGoogleCredentialFromEnv(string envVarName = "GcpApplicationCredentials")
        {
            // Get the credentials JSON content from the environment variable
            var credentialsJson = Environment.GetEnvironmentVariable(envVarName);

            if (string.IsNullOrEmpty(credentialsJson))
            {
                try
                {
                    return GoogleCredential.GetApplicationDefault();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("$Not getting credential from google.", ex);
                }
            }

            // Parse the JSON content into a GoogleCredential object
            return GoogleCredential.FromJson(credentialsJson);
        }
    }
}
