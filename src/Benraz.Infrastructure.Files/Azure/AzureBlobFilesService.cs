using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.Azure
{
    /// <summary>
    /// Azure BLOB files service.
    /// </summary>
    public class AzureBlobFilesService : IFilesService
    {
        private readonly AzureBlobFilesServiceSettings _settings;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public AzureBlobFilesService(IOptions<AzureBlobFilesServiceSettings> settings)
        {
            _settings = settings.Value;
            if (_settings != null && string.IsNullOrWhiteSpace(_settings.RootDirectory))
            {
                _settings.RootDirectory = string.Empty;
            }
        }

        /// <summary>
        /// Returns all files from location.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <param name="fileProperties">File properties to retrieve.</param>
        /// <returns>Files.</returns>
        public async Task<File[]> FindAllAsync(string path, FileProperties fileProperties)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClients = this.GetAllBlobClients(containerClient, this.GetFullBlobPath(path));
            var files = await ToFilesAsync(blobClients, fileProperties);
            return files;
        }

        /// <summary>
        /// Returns file with specified name from location.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="fileProperties">File properties to retrieve.</param>
        /// <returns>File.</returns>
        public async Task<File> FindByNameAsync(string fileName, FileProperties fileProperties)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);

            var fullBlobPath = this.GetFullBlobPath(this.GetBlobName(fileName));
            var blobClient = containerClient.GetBlobClient(fullBlobPath);

            if (!await blobClient.ExistsAsync())
            {
                throw new ArgumentException($"Unable to find blob with name:{fileName}");
            }

            var file = await ToFileAsync(blobClient, fileProperties);

            return file;
        }

        /// <summary>
        /// Creates BLOB in container and returns file with URI updated.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>File.</returns>
        public async Task<File> CreateAsync(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);

            if (!await containerClient.ExistsAsync())
            {
                throw new ArgumentException("Container hasn't been created.");
            }
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(this.GetBlobName(file.Name)));
            using (var stream = new MemoryStream(file.Content))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders()
                {
                    ContentType = GetContentType(file.Name)
                });
            }

            file.Uri = blobClient.Uri.ToString();
            return file;
        }

        /// <summary>
        /// Renames file and returns file with name and URL updated.
        /// </summary>
        /// <param name="file">File to rename.</param>
        /// <param name="newFileName">New file name.</param>
        /// <returns>Updated file.</returns>
        public async Task<File> RenameAsync(File file, string newFileName)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (string.IsNullOrEmpty(file.Name))
            {
                throw new ArgumentException("File has no name.", nameof(file.Name));
            }

            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var oldBlob = containerClient.GetBlobClient(this.GetFullBlobPath(GetBlobName(file.Name)));
            var isOldBlobPersistent = await oldBlob.ExistsAsync();
            if (!isOldBlobPersistent)
            {
                throw new InvalidOperationException("File is not persistent.");
            }

            var newBlob = containerClient.GetBlobClient(this.GetFullBlobPath(GetBlobName(newFileName)));
            await newBlob.StartCopyFromUriAsync(oldBlob.Uri);
            await oldBlob.DeleteAsync();

            file.Name = newFileName;
            file.Uri = newBlob.Uri.ToString();

            return file;
        }

        /// <summary>
        /// Deletes file with specified name from container.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Task.</returns>
        public async Task DeleteAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(this.GetBlobName(fileName)));
            await blobClient.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Deletes all files in particular path.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <returns>Task.</returns>
        public async Task DeleteAllAsync(string path)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClients = this.GetAllBlobClients(containerClient, path);
            await DeleteBlobsAsync(blobClients);
        }

        /// <summary>
        /// Deletes path.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Task.</returns>
        public async Task DeletePathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClients = this.GetAllBlobClients(containerClient, this.GetFullBlobPath(path));
            await DeleteBlobsAsync(blobClients);
        }

        /// <summary>
        /// Returns file URI.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File URI.</returns>
        public string GetUri(string fileName)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(this.GetBlobName(fileName)));

            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Move file from source to destination.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="sourcePath">Source path.</param>
        /// <param name="destinationPath">Destination path.</param>
        /// <returns>Transfer status.</returns>
        public async Task<bool> MoveFileAsync(string fileName, string sourcePath, string destinationPath)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (string.IsNullOrEmpty(destinationPath))
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            var sourceFile = Path.Combine(sourcePath, fileName);
            var destinationFile = Path.Combine(destinationPath, fileName);

            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var srcBlob = containerClient.GetBlobClient(this.GetFullBlobPath(GetBlobName(sourceFile)));
            if (srcBlob is null)
            {
                throw new ArgumentException("Source blob cannot be null.");
            }

            var destBlob = containerClient.GetBlobClient(this.GetFullBlobPath(GetBlobName(destinationFile)));
            if (destBlob == null)
            {
                throw new ArgumentException("Destination blob cannot be null.");
            }

            await destBlob.StartCopyFromUriAsync(srcBlob.Uri);
            await srcBlob.DeleteAsync();

            return true;
        }

        /// <summary>
        /// Create new folder by path.
        /// </summary>
        /// <param name="folderName">Folder name.</param>
        /// <param name="path">Path.</param>
        /// <returns>Creation status.</returns>
        public async Task<bool> CreateFolderAsync(string folderName, string path)
        {
            throw new NotSupportedException($"{nameof(this.CreateFolderAsync)} method is not supported " +
                $"in the {nameof(AzureBlobFilesService)} service.");
        }

        /// <summary>
        /// Returns signed file URI.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="expiresInHours">Expires in hours.</param>
        /// <param name="permission">File permission.</param>
        /// <returns>Signed file URI.</returns>
        public string GetSignedUri(string fileName, int expiresInHours = 1, CloudFilePermission permission = CloudFilePermission.Read)
        {
            var blobServiceClient = this.GetBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.BlobContainer);
            var blobClient = containerClient.GetBlobClient(this.GetFullBlobPath(this.GetBlobName(fileName)));
            var sasUri = blobClient.GenerateSasUri(CloudFilePermissionService.GetPermission(permission, FileType.AzureBlob), DateTimeOffset.UtcNow.AddHours(expiresInHours));

            return sasUri.ToString();
        }

        private string GetFullBlobPath(string fileName) => $"{_settings.RootDirectory}/{fileName}";

        private BlobServiceClient GetBlobServiceClient() => new (_settings.ConnectionString);

        private IEnumerable<BlobClient> GetAllBlobClients(BlobContainerClient container, string prefix)
        {
            foreach (BlobItem blob in container.GetBlobs(BlobTraits.None, BlobStates.None, prefix))
            {
                yield return container.GetBlobClient(blob.Name);
            }
        }

        private async Task DeleteBlobsAsync(IEnumerable<BlobClient> blobs)
        {
            foreach (var blob in blobs)
            {
                await blob.DeleteIfExistsAsync();
            }
        }

        private async Task<File[]> ToFilesAsync(IEnumerable<BlobClient> blobClients, FileProperties fileProperties)
        {
            var files = new List<File>();
            foreach (var blobClient in blobClients)
            {
                var file = await ToFileAsync(blobClient, fileProperties);
                files.Add(file);
            }

            return files.ToArray();
        }

        private async Task<File> ToFileAsync(BlobClient blobClient, FileProperties fileProperties)
        {
            var blobClientProperties = await blobClient.GetPropertiesAsync();
            var file = new File
            {
                Name = blobClient.Name,
                Uri = blobClient.Uri.ToString(),
                CreateTimeUtc = blobClientProperties.Value.CreatedOn.UtcDateTime,
                UpdateTimeUtc = blobClientProperties.Value.LastModified.UtcDateTime
            };

            if ((fileProperties & FileProperties.Content) == FileProperties.Content)
            {
                file.Content = await GetBlobContentAsync(blobClient);
            }

            return file;
        }

        private async Task<byte[]> GetBlobContentAsync(BlobClient blobClient)
        {
            using var blobStream = new MemoryStream();
            await blobClient.DownloadToAsync(blobStream);
            return blobStream.ToArray();
        }

        private string GetBlobName(string fileName)
        {
            return fileName.ToLowerInvariant();
        }

        private string GetContentType(string fileName)
        {
            if (!new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            if (contentType == "text/html")
            {
                contentType = "text/html; charset=utf-8";
            }

            return contentType;
        }
    }
}


