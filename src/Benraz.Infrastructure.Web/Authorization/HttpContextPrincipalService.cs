using Microsoft.AspNetCore.Http;
using Benraz.Infrastructure.Common.AccessControl;
using System.Security.Claims;

namespace Benraz.Infrastructure.Web.Authorization
{
    /// <summary>
    /// HTTP context principal service.
    /// </summary>
    public class HttpContextPrincipalService : IPrincipalService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor.</param>
        public HttpContextPrincipalService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Returns current principal.
        /// </summary>
        /// <returns>Current principal.</returns>
        public ClaimsPrincipal GetPrincipal()
        {
            return _httpContextAccessor.HttpContext.User;
        }
    }
}




