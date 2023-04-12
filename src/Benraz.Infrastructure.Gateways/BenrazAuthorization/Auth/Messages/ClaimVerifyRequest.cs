using Newtonsoft.Json;
using Benraz.Infrastructure.Common.Http;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;

/// <summary>
/// Claim verify request.
/// </summary>
public class ClaimVerifyRequest : HttpRequestBase
{
    /// <summary>
    /// Access token.
    /// </summary>
    [JsonIgnore]
    public string AccessToken { get; set; }

    /// <summary>
    /// Claims.
    /// </summary>
    public List<string> Claims { get; set; }

    /// <summary>
    /// Roles.
    /// </summary>
    public List<string> Roles { get; set; }

    /// <summary>
    /// Returns endpoint.
    /// </summary>
    /// <returns>Endpoint.</returns>
    public string GetEndpoint()
    {
        var endpoint = new EndpointBuilder()
            .SetBaseEndpoint("v1/auth/claim-verify")
            .AddQueryParameter("claims", Claims)
            .AddQueryParameter("roles", Roles)
            .ToString();

        return endpoint;
    }
}

