namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// Files service provider.
    /// </summary>
    public interface IFilesServiceProvider
    {
        /// <summary>
        /// Returns actual files service.
        /// </summary>
        /// <returns>Files service.</returns>
        IFilesService GetService();
    }
}



