using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Common.EnvironmentConfiguration
{
    /// <summary>
    /// Create custom environment variable configuration provider
    /// </summary>
    public class CustomEnvironmentVariableConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Loads configuration from environment variable.
        /// </summary>
        public override void Load()
        {
            var envVariables = Environment.GetEnvironmentVariables();
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (DictionaryEntry envVar in envVariables)
            {
                var key = envVar.Key.ToString();
                var value = envVar.Value?.ToString();
                if (key.Contains("_"))
                {
                    key = key.Replace("_", ":");
                }
                data[key] = value;
            }
            Data = data;
        }
    }
}
