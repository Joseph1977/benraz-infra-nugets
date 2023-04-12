using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Authorization.PermissionsRequirement;

/// <summary>
/// Permission requirement handler.
/// </summary>
public class PermissionRequirementHandler : AuthorizationHandler<PermissionsRequirement>
{
    private readonly IBenrazAuthorizationAuthGateway _authGateway;
    private readonly PermissionsRequirementAuthGatewaySettings _authSettings;

    /// <summary>
    /// Permission requirement handler.
    /// </summary>
    /// <param name="authGateway">Auth gateway.</param>
    /// <param name="authSettings">Auth settings.</param>
    public PermissionRequirementHandler(IBenrazAuthorizationAuthGateway authGateway,
        IOptions<PermissionsRequirementAuthGatewaySettings> authSettings)
    {
        _authGateway = authGateway;
        _authSettings = authSettings.Value;
    }

    /// <summary>
    /// Handle requirement.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="requirement">Permissions requirement.</param>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            context.Fail();
        else
        {
            var permissions = requirement.Permissions;
            var isVerified = await IsVerifiedPermissionsAsync(context, permissions);
            if (isVerified)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }

    private async Task<bool> IsVerifiedPermissionsAsync(AuthorizationHandlerContext context, List<string> permissions)
    {
        if (permissions.Any(x => context.User.HasClaim(CommonClaimTypes.CLAIM, x)))
            return true;

        var roles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
        if (!roles.Any())
            return false;

        var claimVerifyRequest = new ClaimVerifyRequest()
        {
            Roles = roles.Select(x => x.Value).ToList(),
            Claims = permissions,
            AccessToken = _authSettings.AuthAccessToken
        };

        var claimVerifyResponse = await _authGateway.SendAsync(claimVerifyRequest);

        return claimVerifyResponse.IsSuccessHttpStatusCode && claimVerifyResponse.IsVerified;
    }
}


