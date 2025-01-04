namespace Benraz.Infrastructure.Files.FileManipulators
{
    /// <summary>
    /// File information.
    /// </summary>
    public class FileInformation
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// File name without extension.
        /// </summary>
        public string FileNameWithoutExtension { get; set; }
    }
}
