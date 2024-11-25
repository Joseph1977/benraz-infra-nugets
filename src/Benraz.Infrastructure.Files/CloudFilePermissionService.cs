using System.Net.Http;
using Azure.Storage.Sas;

namespace Benraz.Infrastructure.Files;

/// <summary>
/// Cloud file permission service.
/// </summary>
public class CloudFilePermissionService
{
    /// <summary>
    /// Get permission.
    /// </summary>
    /// <param name="permission">Permission.</param>
    /// <param name="provider">Provider.</param>
    /// <returns></returns>
    public static dynamic GetPermission(CloudFilePermission permission, FileType provider)
    {
        switch (provider)
        {
            case FileType.AzureBlob:
                return permission switch
                {
                    CloudFilePermission.Read => BlobSasPermissions.Read,
                    CloudFilePermission.Write => BlobSasPermissions.Write,
                    CloudFilePermission.Delete => BlobSasPermissions.Delete,
                    CloudFilePermission.List => BlobSasPermissions.List,
                    _ => null
                };
            case FileType.GcpBucket:
                return permission switch
                {
                    CloudFilePermission.Read => HttpMethod.Get,
                    CloudFilePermission.Write => HttpMethod.Post,
                    CloudFilePermission.Delete => HttpMethod.Delete,
                    CloudFilePermission.List => HttpMethod.Get,
                    _ => null
                };
        }
        return null;
    }
}