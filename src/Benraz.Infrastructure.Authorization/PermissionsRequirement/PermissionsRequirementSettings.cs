namespace Benraz.Infrastructure.Authorization.PermissionsRequirement;

/// <summary>
/// Permissions requirement auth gateway settings.
/// </summary>
public class PermissionsRequirementAuthGatewaySettings
{
    /// <summary>
    /// Authorization base URL.
    /// </summary>
    public string AuthBaseUrl { get; set; }

    /// <summary>
    /// Authorization access token.
    /// </summary>
    public string AuthAccessToken { get; set; }
}


