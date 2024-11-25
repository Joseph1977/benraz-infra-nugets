namespace Benraz.Infrastructure.Files.GCP;

/// <summary>
/// Google Cloud Platform bucket files service settings.
/// </summary>
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