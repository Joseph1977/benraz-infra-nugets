using Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users
{
    /// <summary>
    /// Benraz Authorization users gateway.
    /// </summary>
    public interface IBenrazAuthorizationUsersGateway
    {
        /// <summary>
        /// Sends get users request.
        /// </summary>
        /// <param name="request">Get users request.</param>
        /// <returns>Get users response.</returns>
        Task<GetUsersResponse> SendAsync(GetUsersRequest request);

        /// <summary>
        /// Sends get user request.
        /// </summary>
        /// <param name="request">Get user request.</param>
        /// <returns>Get user response.</returns>
        Task<GetUserResponse> SendAsync(GetUserRequest request);

        /// <summary>
        /// Sends get user info by email request.
        /// </summary>
        /// <param name="request">Get user info by email request.</param>
        /// <returns>Get user info by email response.</returns>
        Task<GetUserInfoByEmailResponse> SendAsync(GetUserInfoByEmailRequest request);

        /// <summary>
        /// Creates user request.
        /// </summary>
        /// <param name="request">Create user request.</param>
        /// <returns>Create user response.</returns>
        Task<CreateUserResponse> SendAsync(CreateUserRequest request);

        /// <summary>
        /// Sends get user roles request.
        /// </summary>
        /// <param name="request">Get user roles request.</param>
        /// <returns>Get user roles response.</returns>
        Task<GetUserRolesResponse> SendAsync(GetUserRolesRequest request);

        /// <summary>
        /// Sends set user roles request.
        /// </summary>
        /// <param name="request">Set user roles request.</param>
        /// <returns>Set user roles response.</returns>
        Task<SetUserRolesResponse> SendAsync(SetUserRolesRequest request);
    }
}



