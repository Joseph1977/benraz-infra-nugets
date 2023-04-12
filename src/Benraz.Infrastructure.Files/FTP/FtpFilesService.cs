using FluentFTP;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.FTP
{
    /// <summary>
    /// FTP files service.
    /// </summary>
    public class FtpFilesService : IFilesService
    {
        private readonly FtpFilesServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public FtpFilesService(IOptions<FtpFilesServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Returns all files from location.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <param name="fileProperties">File properties to retrieve.</param>
        /// <returns>Files.</returns>
        public async Task<File[]> FindAllAsync(string path, FileProperties fileProperties)
        {
            var files = new File[] { };
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (await client.DirectoryExistsAsync(path, token))
                {
                    // get a list of files and directories in the path folder
                    var remoteFiles = (await client.GetListingAsync(path, token))?
                        .Where(x => x.Type == FtpFileSystemObjectType.File).ToArray();
                    files = await ToFilesAsync(remoteFiles, fileProperties, client, token);
                }
            }

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

            File file = null;
            var directoryPath = GetDirectoryPathFromFileNamePath(fileName);
            fileName = GetFileName(fileName);
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (await client.DirectoryExistsAsync(directoryPath, token))
                {
                    var remoteFile = (await client.GetListingAsync(directoryPath, token))?
                        .Where(x => x.Type == FtpFileSystemObjectType.File && x.Name == fileName).SingleOrDefault();
                    if (remoteFile != null)
                        file = await ToFileAsync(remoteFile, fileProperties, client, token);
                }
            }

            return file;
        }

        /// <summary>
        /// Creates file in location and returns file with URI updated.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>File.</returns>
        public async Task<File> CreateAsync(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var (client, token) = await GetFtpConnection();
            using (client)
            {
                client.RetryAttempts = 3;
                await client.UploadAsync(file.Content, $"{file.Name}", FtpRemoteExists.Overwrite, true, null, token);
            }
            file.Uri = GetFileUri(file.Name);

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

            if (string.IsNullOrEmpty(newFileName))
            {
                throw new ArgumentNullException(nameof(newFileName));
            }

            var (client, token) = await GetFtpConnection();
            using (client)
            {
                var isFileExist = await client.FileExistsAsync(file.Name, token);
                if (!isFileExist)
                {
                    throw new InvalidOperationException("File is not persistent.");
                }

                var directoryPath = GetDirectoryPathFromFileNamePath(newFileName);
                if (!await client.DirectoryExistsAsync(directoryPath, token))
                {
                    await client.CreateDirectoryAsync(directoryPath, token);
                }
                var status = await client.MoveFileAsync(file.Name, newFileName, FtpRemoteExists.Overwrite, token);
                if (status)
                {
                    file.Name = newFileName;
                    file.Uri = GetFileUri(newFileName);
                }
            }

            return file;
        }

        /// <summary>
        /// Deletes file with specified name from location.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Task.</returns>
        public async Task DeleteAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (await client.FileExistsAsync(fileName, token))
                {
                    await client.DeleteFileAsync(fileName, token);
                }
            }
        }

        /// <summary>
        /// Deletes all files in particular path.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <returns>Task.</returns>
        public async Task DeleteAllAsync(string path)
        {
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (await client.DirectoryExistsAsync(path, token))
                {
                    foreach (FtpListItem item in await client.GetListingAsync(path, token))
                    {
                        if (item.Type == FtpFileSystemObjectType.File)
                        {
                            await client.DeleteFileAsync(item.FullName, token);
                        }
                    }
                }
            }
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

            var (client, token) = await GetFtpConnection();
            using (client)
            {
                if (await client.DirectoryExistsAsync(path, token))
                {
                    await client.DeleteDirectoryAsync(path, token);
                }
            }
        }

        /// <summary>
        /// Returns file URI.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File URI.</returns>
        public string GetUri(string fileName)
        {
           return GetFileUri(fileName);
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

            bool status;
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                var sourceFilePath = Path.Combine(sourcePath, fileName);
                var destinationFilePath = Path.Combine(destinationPath, fileName);
                status = await client.MoveFileAsync(sourceFilePath, destinationFilePath, FtpRemoteExists.Overwrite, token);
            }

            return status;
        }

        /// <summary>
        /// Create new folder by path.
        /// </summary>
        /// <param name="folderName">Folder name.</param>
        /// <param name="path">Path.</param>
        /// <returns>Creation status.</returns>
        public async Task<bool> CreateFolderAsync(string folderName, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            var folderNamePath = Path.Combine(path, folderName);
            bool status;
            var (client, token) = await GetFtpConnection();
            using (client)
            {
                status = await client.CreateDirectoryAsync(folderNamePath, true, token);
            }

            return status;
        }

        /// <summary>
        /// Returns signed file URI.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="expiresInHours">Expires in hours.</param>
        /// <returns>Signed file URI.</returns>
        public string GetSignedUri(string fileName, int expiresInHours = 1)
        {
            throw new NotSupportedException($"{nameof(this.GetSignedUri)} method is not supported " +
                $"in the {nameof(FtpFilesService)} service.");
        }

        private async Task<(FtpClient client, CancellationToken token)> GetFtpConnection()
        {
            var token = new CancellationToken();
            FtpClient client = new FtpClient(new Uri(_settings.BaseUrl), _settings.UserName, _settings.Password);
            client.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateAnyCertificate = true;
            client.SocketKeepAlive = true;
            await client.ConnectAsync(token);
            if (!string.IsNullOrEmpty(_settings.RootDirectory))
            {
                if (!await client.DirectoryExistsAsync($"/{_settings.RootDirectory}", token))
                    await client.CreateDirectoryAsync($"/{_settings.RootDirectory}", token);
                await client.SetWorkingDirectoryAsync($"/{_settings.RootDirectory}", token);
            }

            return (client, token);
        }

        private async Task<File[]> ToFilesAsync(FtpListItem[] remoteFiles, FileProperties fileProperties,
            FtpClient ftpClient, CancellationToken token)
        {
            var files = new List<File>();
            foreach (var remoteFile in remoteFiles)
            {
                var file = await ToFileAsync(remoteFile, fileProperties, ftpClient, token);
                files.Add(file);
            }

            return files.ToArray();
        }

        private async Task<File> ToFileAsync(FtpListItem remoteFile, FileProperties fileProperties,
            FtpClient ftpClient, CancellationToken token)
        {
            var file = new File
            {
                CreateTimeUtc = remoteFile.RawCreated,
                Name = remoteFile.Name,
                Uri = GetFileUri(remoteFile.FullName),
                UpdateTimeUtc = remoteFile.RawModified,

            };
            if ((fileProperties & FileProperties.Content) == FileProperties.Content)
            {
                file.Content = await ftpClient.DownloadAsync(remoteFile.FullName, token);
            }

            return file;
        }

        private string GetFileUri(string fileNamePath)
        {
            var rootDirectory = _settings.RootDirectory;
            if (!fileNamePath.StartsWith(rootDirectory))
            {
                fileNamePath = Path.Combine(rootDirectory, fileNamePath);
            }
            return new Uri(new Uri(_settings.BaseUrl), fileNamePath).ToString();
        }

        private string GetDirectoryPathFromFileNamePath(string fileName)
        {
            fileName = fileName.Trim('/');
            var filePath = fileName.Split('/');
            if (filePath.Length < 2)
                return string.Empty;
            var directories = filePath.ToList();
            directories.RemoveAt(filePath.Length - 1);
            var directoryPath = string.Join("/", directories);

            return directoryPath;
        }

        private string GetFileName(string fileName)
        {
            fileName = fileName.Trim('/');
            var filePath = fileName.Split('/');
            if (filePath.Length < 2)
                return fileName;
            fileName = filePath[filePath.Length - 1];
            return fileName;
        }
    }
}


