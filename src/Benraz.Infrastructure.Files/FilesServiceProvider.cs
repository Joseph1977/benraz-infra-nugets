using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Files.Azure;
using Benraz.Infrastructure.Files.FTP;
using Benraz.Infrastructure.Files.Local;
using System;

namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// Files service provider.
    /// </summary>
    public class FilesServiceProvider : IFilesServiceProvider
    {
        private readonly FilesServiceProviderSettings _settings;

        /// <summary>
        /// Creates files service provider.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public FilesServiceProvider(IOptions<FilesServiceProviderSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Returns actual files service.
        /// </summary>
        /// <returns>Files service.</returns>
        public IFilesService GetService()
        {
            switch (_settings.FilesType)
            {
                case FileType.Local:
                    return new LocalFilesService(Options.Create(_settings.LocalFile));
                case FileType.AzureBlob:
                    return new AzureBlobFilesService(Options.Create(_settings.AzureBlob));
                case FileType.Ftp:
                    return new FtpFilesService(Options.Create(_settings.Ftp));
                default:
                    throw new NotSupportedException("Not supported storage type.");
            }
        }
    }
}



