namespace Benraz.Infrastructure.Common.Configuration
{
    /// <summary>
    /// Manages configuration settings.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Gets value from config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="key">Config key.</param>
        /// <returns>Config settings value.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets value from config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>Config settings value.</returns>
        T Get<T>();

        /// <summary>
        /// Saves value to config.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="settings">Settings value.</param>
        /// <param name="archiveConfig">Archive config.</param>
        void Save<T>(T settings, bool archiveConfig = false);
    }
}




