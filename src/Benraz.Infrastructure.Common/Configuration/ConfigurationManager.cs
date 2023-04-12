using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Benraz.Infrastructure.Common.Configuration
{
    /// <summary>
    /// Manages configuration settings.
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Creates new Configuration Manager.
        /// </summary>
        /// <param name="config">Real config source.</param>
        public ConfigurationManager(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets value from config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="key">Config key.</param>
        /// <returns>Config settings value.</returns>
        public T Get<T>(string key)
        {
            return _config.GetSection(key).Get<T>();
        }

        /// <summary>
        /// Gets value from config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>Config settings value.</returns>
        public T Get<T>()
        {
            return _config.Get<T>();
        }

        /// <summary>
        /// Saves value to config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="settings">Settings value.</param>
        /// <param name="archiveConfig">Archive config.</param>
        public void Save<T>(T settings, bool archiveConfig = false)
        {
            var s = JsonConvert.SerializeObject(settings);
            var configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");
            if (archiveConfig)
            {
                File.Move(configPath, Path.Combine(
                    Path.GetDirectoryName(configPath),
                    $"appsettings.{DateTime.UtcNow.ToString("yyyy-MM-dd_HHmmss")}.json"));
            }
            File.WriteAllText(configPath, s);
        }
    }
}




