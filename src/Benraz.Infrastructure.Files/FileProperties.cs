using System;

namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// File properties.
    /// </summary>
    [Flags]
    public enum FileProperties
    {
        /// <summary>
        /// Metadata.
        /// </summary>
        Metadata = 1,

        /// <summary>
        /// Content.
        /// </summary>
        Content = 2,

        /// <summary>
        /// All properties.
        /// </summary>
        All = 3
    }
}



