using System;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Files.FileManipulators
{
    /// <summary>
    /// File types.
    /// </summary>
    public static class FileTypes
    {
        // Predefined MIME types
        public static readonly string Pdf = "application/pdf";
        public static readonly string Txt = "text/plain";

        // Dictionary for extension to MIME type mapping
        private static readonly Dictionary<string, string> MimeTypeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".pdf", Pdf },
            { ".txt", Txt }
        };

        /// <summary>
        /// Parses the MIME type based on the file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension, including the dot (e.g., ".pdf").</param>
        /// <returns>The MIME type as a string.</returns>
        /// <exception cref="ArgumentException">Thrown if the file extension is not recognized.</exception>
        public static string Parse(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new ArgumentException("File extension cannot be null or empty.", nameof(fileExtension));
            }

            if (MimeTypeMapping.TryGetValue(fileExtension, out var mimeType))
            {
                return mimeType;
            }

            throw new ArgumentException($"MIME type not found for the file extension: {fileExtension}", nameof(fileExtension));
        }
    }
}
