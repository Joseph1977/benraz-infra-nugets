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
                if (await client.DirectoryExists(path, token))
                {
                    // get a list of files and directories in the path folder
                    var remoteFiles = (await client.GetListing(path, token))?
                        .Where(x => x.Type == FtpObjectType.File).ToArray();
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
                if (await client.DirectoryExists(directoryPath, token))
                {
                    var remoteFile = (await client.GetListing(directoryPath, token))?
                        .Where(x => x.Type == FtpObjectType.File && x.Name == fileName).SingleOrDefault();
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
                client.Config.RetryAttempts = 3;
                FtpStatus uploadStatus = await client.UploadBytes(file.Content, file.Name,
                                    FtpRemoteExists.Overwrite, true, token: token);

                if (uploadStatus != FtpStatus.Success)
                {
                    throw new Exception($"Failed to upload file: {file.Name}");
                }
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
                var isFileExist = await client.FileExists(file.Name, token);
                if (!isFileExist)
                {
                    throw new InvalidOperationException("File is not persistent.");
                }

                var directoryPath = GetDirectoryPathFromFileNamePath(newFileName);
                if (!await client.DirectoryExists(directoryPath, token))
                {
                    await client.CreateDirectory(directoryPath, token);
                }
                var status = await client.MoveFile(file.Name, newFileName, FtpRemoteExists.Overwrite, token);
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
                if (await client.FileExists(fileName, token))
                {
                    await client.DeleteFile(fileName, token);
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
                if (await client.DirectoryExists(path, token))
                {
                    foreach (FtpListItem item in await client.GetListing(path, token))
                    {
                        if (item.Type == FtpObjectType.File)
                        {
                            await client.DeleteFile(item.FullName, token);
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
                if (await client.DirectoryExists(path, token))
                {
                    await client.DeleteDirectory(path, token);
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
                status = await client.MoveFile(sourceFilePath, destinationFilePath, FtpRemoteExists.Overwrite, token);
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
                status = await client.CreateDirectory(folderNamePath, true, token);
            }

            return status;
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
            throw new NotSupportedException($"{nameof(this.GetSignedUri)} method is not supported " +
                $"in the {nameof(FtpFilesService)} service.");
        }

        private async Task<(AsyncFtpClient client, CancellationToken token)> GetFtpConnection()
        {
            var token = new CancellationToken();
            //FtpClient client = new FtpClient(new Uri(_settings.BaseUrl), _settings.UserName, _settings.Password);
            AsyncFtpClient client = new AsyncFtpClient(_settings.BaseUrl, _settings.UserName, _settings.Password);

            client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            client.Config.ValidateAnyCertificate = true;
            client.Config.SocketKeepAlive = true;
            await client.Connect(token);
            if (!string.IsNullOrEmpty(_settings.RootDirectory))
            {
                if (!await client.DirectoryExists($"/{_settings.RootDirectory}", token))
                    await client.CreateDirectory($"/{_settings.RootDirectory}", token);
                await client.SetWorkingDirectory($"/{_settings.RootDirectory}", token);
            }

            return (client, token);
        }

        private async Task<File[]> ToFilesAsync(FtpListItem[] remoteFiles, FileProperties fileProperties,
            AsyncFtpClient ftpClient, CancellationToken token)
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
            AsyncFtpClient ftpClient, CancellationToken token)
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
                file.Content = await ftpClient.DownloadBytes(remoteFile.FullName, token: token);
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


