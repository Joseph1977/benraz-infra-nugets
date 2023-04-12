namespace Benraz.Infrastructure.Files.Azure
{
    /// <summary>
    /// Azure BLOB files service settings.
    /// </summary>
    public class AzureBlobFilesServiceSettings
    {
        /// <summary>
        /// Azure storage connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Azure BLOB container name.
        /// </summary>
        public string BlobContainer { get; set; }

        /// <summary>
        /// Root directory name.
        /// </summary>
        public string RootDirectory { get; set; }
    }
}


