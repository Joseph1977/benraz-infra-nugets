using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Files.FileManipulators
{
    /// <summary>
    /// File manipulator service.
    /// </summary>
    public class FileManipulatorService : IFileManipulatorService
    {
        /// <summary>
        /// File manipulator service.
        /// </summary>
        public FileManipulatorService()
        {
        }

        /// <summary>
        /// Extract pdf pages.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="startPage">Start page.</param>
        /// <param name="endPage">End page.</param>
        /// <returns>Byte array.</returns>
        public async Task<byte[]> ExtractPdfPages(File file, int startPage, int endPage)
        {
            // Read the existing PDF
            using (var pdfDoc = PdfReader.Open(new MemoryStream(file.Content), PdfDocumentOpenMode.Import))
            {
                int totalPages = pdfDoc.PageCount;
                int extractedPagesCount = 0;

                var chunkDoc = new PdfDocument();
                int mEndPage = Math.Min(endPage, totalPages);

                // Copy pages from startPage to endPage (inclusive)
                for (int i = startPage; i < mEndPage; i++) // Adjust for 0-based index
                {
                    chunkDoc.AddPage(pdfDoc.Pages[i]);
                    extractedPagesCount++;
                }

                // Return the PDF as byte array
                using (var memoryStream = new MemoryStream())
                {
                    chunkDoc.Save(memoryStream, false);
                    return await Task.FromResult(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// Get file name and extension.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>File information.</returns>
        public async Task<FileInformation> GetFileNameAndExtension(File file)
        {
            string fileName = Path.GetFileName(file.Name); // Gets the file name with extension
            string extension = Path.GetExtension(file.Name); // Gets only the extension
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name); // Gets the file name without extension

            var fileInfo = new FileInformation
            {
                FileName = fileName,
                Extension = extension,
                FileNameWithoutExtension = nameWithoutExtension
            };

            return await Task.FromResult(fileInfo);
        }

        /// <summary>
        /// Get pdf page count.
        /// </summary>
        /// <param name="file">File.</param>
        /// <returns>Total pdf page count.</returns>
        public async Task<int> GetPdfPageCount(File file)
        {
            // Read the existing PDF
            using (var pdfDoc = PdfReader.Open(new MemoryStream(file.Content), PdfDocumentOpenMode.Import))
            {
                return await Task.FromResult(pdfDoc.PageCount);
            }
        }

        /// <summary>
        /// Check is it text file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>True / False.</returns>
        public async Task<bool> IsTextFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            string fileType = FileTypes.Parse(extension);

            return await Task.FromResult(fileType == FileTypes.Txt);
        }

        /// <summary>
        /// Read text file in chunks.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="chunkSize">Chunk size.</param>
        /// <returns>List of chunks.</returns>
        public async Task<List<string>> ReadFileInChunks(File file, int chunkSize)
        {
            string data = "";
            using (var memoryStream = new MemoryStream(file.Content))
            using (var reader = new StreamReader(memoryStream))
            {
                // Read the entire content as a string
                data = reader.ReadToEnd();
            }
            var chunks = new List<string>();

            for (int i = 0; i < data.Length; i += chunkSize)
            {
                chunks.Add(data.Substring(i, Math.Min(chunkSize, data.Length - i)));
            }

            return await Task.FromResult(chunks);
        }
    }
}
