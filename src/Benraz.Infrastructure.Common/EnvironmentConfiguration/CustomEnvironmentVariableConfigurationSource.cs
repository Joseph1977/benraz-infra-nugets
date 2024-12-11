using Microsoft.Extensions.Configuration;

namespace Benraz.Infrastructure.Common.EnvironmentConfiguration
{
    /// <summary>
    /// Create custom environment variable configuration source
    /// </summary>
    public class CustomEnvironmentVariableConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Returns configuration provider.
        /// </summary>
        /// <param name="builder">Configuration builder.</param>
        /// <returns>Configuration provider.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomEnvironmentVariableConfigurationProvider();
        }
    }
}
