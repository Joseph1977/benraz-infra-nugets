using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.Local
{
    /// <summary>
    /// Local system files service.
    /// </summary>
    public class LocalFilesService : IFilesService
    {
        private readonly LocalFilesServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public LocalFilesService(IOptions<LocalFilesServiceSettings> settings)
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
            var directoryName = _settings.Directory;
            if (!string.IsNullOrEmpty(path))
            {
                directoryName = Path.Combine(directoryName, path);
            }

            var localDirectory = new DirectoryInfo(directoryName);
            if (!localDirectory.Exists)
            {
                return new File[] { };
            }

            var localFiles = localDirectory.GetFiles();

            var files = await ToFilesAsync(localFiles, path, fileProperties);
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

            var localDirectory = new DirectoryInfo(_settings.Directory);
            if (!localDirectory.Exists)
            {
                return null;
            }

            var localFile = localDirectory.GetFiles(fileName).FirstOrDefault();
            if (localFile == null)
            {
                return null;
            }

            var file = await ToFileAsync(localFile, null, fileProperties);
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

            var fullFileName = GetFullFileName(file.Name);

            var directory = new FileInfo(fullFileName).Directory;
            if (!directory.Exists)
            {
                directory.Create();
            }

            var localFile = new FileInfo(fullFileName);
            using (var stream = localFile.Create())
            {
                await stream.WriteAsync(file.Content, 0, file.Content.Length);
            }
            file.Uri = GetFileUri(fullFileName);

            return file;
        }

        /// <summary>
        /// Renames file and returns file with name and URL updated.
        /// </summary>
        /// <param name="file">File to rename.</param>
        /// <param name="newFileName">New file name.</param>
        /// <returns>Updated file.</returns>
        public Task<File> RenameAsync(File file, string newFileName)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var oldFullFileName = GetFullFileName(file.Name);
            var localFile = new FileInfo(oldFullFileName);
            if (localFile == null)
            {
                throw new InvalidOperationException("File is not persistent.");
            }

            var newFullFileName = GetFullFileName(newFileName);
            var newDirectory = new FileInfo(newFullFileName).Directory;
            if (!newDirectory.Exists)
            {
                newDirectory.Create();
            }
            localFile.MoveTo(newFullFileName);

            file.Name = newFileName;
            file.Uri = GetFileUri(newFileName);

            return Task.FromResult(file);
        }

        /// <summary>
        /// Deletes file with specified name from location.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Task.</returns>
        public Task DeleteAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var fullFileName = GetFullFileName(fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes all files in particular path.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <returns>Task.</returns>
        public Task DeleteAllAsync(string path)
        {
            var directoryName = _settings.Directory;
            if (!string.IsNullOrEmpty(path))
            {
                directoryName = Path.Combine(directoryName, path);
            }

            var directory = new DirectoryInfo(directoryName);
            if (!directory.Exists)
            {
                return Task.CompletedTask;
            }

            var files = directory.GetFiles();
            foreach (var file in files)
            {
                file.Delete();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes path.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Task.</returns>
        public Task DeletePathAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var directoryName = Path.Combine(_settings.Directory, path);
            var directory = new DirectoryInfo(directoryName);
            if (directory.Exists)
            {
                directory.Delete(true);
            }

            return Task.CompletedTask;
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
        public Task<bool> MoveFileAsync(string fileName, string sourcePath, string destinationPath)
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

            var sourceDirectoryName = _settings.Directory;
            var destinationDirectoryName = _settings.Directory;
            sourceDirectoryName = Path.Combine(sourceDirectoryName, sourcePath);
            destinationDirectoryName = Path.Combine(destinationDirectoryName, destinationPath);
            var sourceFile = Path.Combine(sourceDirectoryName, fileName);
            var destinationFile = Path.Combine(destinationDirectoryName, fileName);
            System.IO.File.Move(sourceFile, destinationFile);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Create new folder by path.
        /// </summary>
        /// <param name="folderName">Folder name.</param>
        /// <param name="path">Path.</param>
        /// <returns>Creation status.</returns>
        public Task<bool> CreateFolderAsync(string folderName, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            var directoryName = _settings.Directory;
            directoryName = Path.Combine(directoryName, path);
            var directory = new DirectoryInfo(directoryName);
            directory.CreateSubdirectory(folderName);

            return Task.FromResult(true);
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
                $"in the {nameof(LocalFilesService)} service.");
        }

        private async Task<File[]> ToFilesAsync(FileInfo[] localFiles, string path, FileProperties fileProperties)
        {
            var files = new List<File>();
            foreach (var localFile in localFiles)
            {
                var file = await ToFileAsync(localFile, path, fileProperties);
                files.Add(file);
            }

            return files.ToArray();
        }

        private Task<File> ToFileAsync(FileInfo localFile, string path, FileProperties fileProperties)
        {
            var fileName = localFile.Name;
            if (!string.IsNullOrEmpty(path))
            {
                fileName = Path.Combine(path, fileName);
            }

            var file = new File
            {
                Name = fileName,
                Uri = GetFileUri(fileName),
                CreateTimeUtc = localFile.CreationTimeUtc,
                UpdateTimeUtc = localFile.LastWriteTimeUtc
            };

            if ((fileProperties & FileProperties.Content) == FileProperties.Content)
            {
                file.Content = System.IO.File.ReadAllBytes(localFile.FullName);
            }

            return Task.FromResult(file);
        }

        private string GetFullFileName(string fileName)
        {
            return Path.Combine(_settings.Directory, fileName);
        }

        private string GetFileUri(string fileName)
        {
            if(string.IsNullOrEmpty(_settings.BaseUrl))
                return string.Empty;

            return new Uri(new Uri(_settings.BaseUrl), fileName).ToString();
        }
    }
}


