namespace Benraz.Infrastructure.Files.GCP;

public class GcpBucketFilesServiceSettings
{
    /// <summary>
    /// Bucket name.
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// Google Cloud Platform application credentials.
    /// </summary>
    public string GcpApplicationCredentials { get; set; }
}