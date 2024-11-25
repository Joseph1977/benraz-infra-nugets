namespace Benraz.Infrastructure.Files
{
    /// <summary>
    /// File type.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Local.
        /// </summary>
        Local = 1,

        /// <summary>
        /// Azure BLOB.
        /// </summary>
        AzureBlob = 2,

        /// <summary>
        /// FTP.
        /// </summary>
        Ftp = 3,
        
        /// <summary>
        /// Google Cloud Platform Bucket.
        /// </summary>
        GcpBucket = 4
    }
}



