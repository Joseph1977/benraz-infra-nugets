namespace Benraz.Infrastructure.Phone
{
    /// <summary>
    /// Phone service provider.
    /// </summary>
    public interface IPhoneServiceProvider
    {
        /// <summary>
        /// Returns actual phone service.
        /// </summary>
        /// <returns>Phone service.</returns>
        IPhoneService GetService();
    }
}


