using System.Security.Claims;

namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// Principal service.
    /// </summary>
    public interface IPrincipalService
    {
        /// <summary>
        /// Returns current principal.
        /// </summary>
        /// <returns>Current principal.</returns>
        ClaimsPrincipal GetPrincipal();
    }
}



