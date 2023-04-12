using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace Benraz.Infrastructure.Authorization.PermissionsRequirement;

/// <summary>
/// Permissions requirement.
/// </summary>
public class PermissionsRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Permissions requirement.
    /// </summary>
    /// <param name="permissions">Permissions.</param>
    public PermissionsRequirement(params string[] permissions) =>
        Permissions = permissions.ToList();

    /// <summary>
    /// Permissions.
    /// </summary>
    public List<string> Permissions { get; }
}


