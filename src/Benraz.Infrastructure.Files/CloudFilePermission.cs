namespace Benraz.Infrastructure.Files;

/// <summary>
/// Cloud file permission.
/// </summary>
public enum CloudFilePermission
{
    /// <summary>
    /// Read permission.
    /// </summary>
    Read = 1,
    
    /// <summary>
    /// Write permission.
    /// </summary>
    Write = 2,
    
    /// <summary>
    /// Delete permission.
    /// </summary>
    Delete = 4,
    
    /// <summary>
    /// List permission.
    /// </summary>
    List = 8,
}