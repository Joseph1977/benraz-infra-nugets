using System.Net.Http;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// Files service.
    /// </summary>
    public interface IFilesService
    {
        /// <summary>
        /// Returns all files from location.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <param name="fileProperties">File properties to retrieve.</param>
        /// <returns>Files.</returns>
        Task<File[]> FindAllAsync(string path, FileProperties fileProperties);

        /// <summary>
        /// Returns file with specified name from location.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="fileProperties">File properties to retrieve.</param>
        /// <returns>File.</returns>
        Task<File> FindByNameAsync(string fileName, FileProperties fileProperties);

        /// <summary>
        /// Creates file in location and returns file with URL updated.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>File.</returns>
        Task<File> CreateAsync(File file);

        /// <summary>
        /// Renames file and returns file with name and URL updated.
        /// </summary>
        /// <param name="file">File to rename.</param>
        /// <param name="newFileName">New file name.</param>
        /// <returns>Updated file.</returns>
        Task<File> RenameAsync(File file, string newFileName);

        /// <summary>
        /// Deletes file with specified name from location.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(string fileName);

        /// <summary>
        /// Deletes all files in particular path.
        /// </summary>
        /// <param name="path">Path to files.</param>
        /// <returns>Task.</returns>
        Task DeleteAllAsync(string path);

        /// <summary>
        /// Deletes path.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Task.</returns>
        Task DeletePathAsync(string path);

        /// <summary>
        /// Returns file URL.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File URL.</returns>
        string GetUri(string fileName);

        /// <summary>
        /// Move file from source to destination.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="sourcePath">Source path.</param>
        /// <param name="destinationPath">Destination path.</param>
        /// <returns>Transfer status.</returns>
        Task<bool> MoveFileAsync(string fileName, string sourcePath, string destinationPath);

        /// <summary>
        /// Create new folder by path.
        /// </summary>
        /// <param name="folderName">Folder name.</param>
        /// <param name="path">Path.</param>
        /// <returns>Creation status.</returns>
        Task<bool> CreateFolderAsync(string folderName, string path);

        /// <summary>
        /// Returns signed file URI.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="expiresInHours">Expires in hours.</param>
        /// <param name="permission">File permission.</param>
        /// <returns>Signed file URI.</returns>
        string GetSignedUri(string fileName, int expiresInHours = 1, CloudFilePermission permission = CloudFilePermission.Read);
    }
}



