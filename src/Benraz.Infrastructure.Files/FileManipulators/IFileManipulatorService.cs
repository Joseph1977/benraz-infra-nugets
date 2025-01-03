using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.FileManipulators
{
    /// <summary>
    /// File manipulator service.
    /// </summary>
    public interface IFileManipulatorService
    {
        /// <summary>
        /// Get file name and extension.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>File information.</returns>
        public Task<FileInformation> GetFileNameAndExtension(File file);


        /// <summary>
        /// Check is it text file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>True / False.</returns>
        public Task<bool> IsTextFile(string filePath);

        /// <summary>
        /// Get pdf page count.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>Total pdf page count.</returns>
        public Task<int> GetPdfPageCount(File file);

        /// <summary>
        /// Extract pdf pages.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="startPage">Start page.</param>
        /// <param name="endPage">End page.</param>
        /// <returns>Byte array</returns>
        public Task<byte[]> ExtractPdfPages(File file, int startPage, int endPage);

        /// <summary>
        /// Read text file in chunks.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="chunkSize">Chunk size.</param>
        /// <returns>List of chunks.</returns>
        public Task<List<string>> ReadFileInChunks(File file, int chunkSize);
    }
}
