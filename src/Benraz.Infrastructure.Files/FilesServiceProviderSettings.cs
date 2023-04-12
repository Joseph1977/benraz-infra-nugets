using Benraz.Infrastructure.Files.Azure;
using Benraz.Infrastructure.Files.FTP;
using Benraz.Infrastructure.Files.Local;

namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// Files service provider settings.
    /// </summary>
    public class FilesServiceProviderSettings
    {
        /// <summary>
        /// Files type.
        /// </summary>
        public FileType FilesType { get; set; }

        /// <summary>
        /// Local system files service settings.
        /// </summary>
        public LocalFilesServiceSettings LocalFile { get; set; }

        /// <summary>
        /// Azure BLOB files service settings.
        /// </summary>
        public AzureBlobFilesServiceSettings AzureBlob { get; set; }

        /// <summary>
        /// FTP files service settings.
        /// </summary>
        public FtpFilesServiceSettings Ftp { get; set; }
    }
}



