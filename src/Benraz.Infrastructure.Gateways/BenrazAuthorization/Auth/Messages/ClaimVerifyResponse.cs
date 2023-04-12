namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth.Messages;

/// <summary>
/// Claim verify response.
/// </summary>
public class ClaimVerifyResponse : HttpResponseBase
{
    /// <summary>
    /// Is verified.
    /// </summary>
    public bool IsVerified { get; set; }
}

