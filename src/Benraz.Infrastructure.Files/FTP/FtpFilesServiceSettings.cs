namespace Benraz.Infrastructure.Files.FTP
{
    /// <summary>
    /// FTP files service settings.
    /// </summary>
    public class FtpFilesServiceSettings
    {
        /// <summary>
        /// Server base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// root directory name.
        /// </summary>
        public string RootDirectory { get; set; }
    }
}


