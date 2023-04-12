namespace Benraz.Infrastructure.Emails
{
    /// <summary>
    /// Emails service provider.
    /// </summary>
    public interface IEmailsServiceProvider
    {
        /// <summary>
        /// Returns actual emails service.
        /// </summary>
        /// <returns>Emails service.</returns>
        IEmailsService GetService();
    }
}



