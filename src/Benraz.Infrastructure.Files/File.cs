using System;
using System.Linq;

namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// File.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URI.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        public DateTime? CreateTimeUtc { get; set; }

        /// <summary>
        /// Update time in UTC.
        /// </summary>
        public DateTime? UpdateTimeUtc { get; set; }

        /// <summary>
        /// Makes and returns a copy of the file.
        /// </summary>
        /// <returns>File copy.</returns>
        public File Copy()
        {
            return new File
            {
                Name = Name,
                Uri = Uri,
                Content = Content.ToArray(),
                CreateTimeUtc = CreateTimeUtc,
                UpdateTimeUtc = UpdateTimeUtc
            };
        }
    }
}



