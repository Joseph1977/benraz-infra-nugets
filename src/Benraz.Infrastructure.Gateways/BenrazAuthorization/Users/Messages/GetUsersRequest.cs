using Benraz.Infrastructure.Common.Http;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Gateways.BenrazAuthorization.Users.Messages
{
    /// <summary>
    /// Get users request.
    /// </summary>
    public class GetUsersRequest : BenrazAuthorizationUsersRequestBase
    {
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email confirmed.
        /// </summary>
        public bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number confirmed.
        /// </summary>
        public bool? PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Status identifiers.
        /// </summary>
        public ICollection<int> StatusIds { get; set; }

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Returns endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = new EndpointBuilder()
                .SetBaseEndpoint($"v1/users")
                .AddQueryParameter("email", Email)
                .AddQueryParameter("phoneNumber", PhoneNumber)
                .AddQueryParameter("emailConfirmed", EmailConfirmed)
                .AddQueryParameter("phoneNumberConfirmed", PhoneNumberConfirmed)
                .AddQueryParameter("statusIds", StatusIds)
                .AddQueryParameter("pageNo", PageNo)
                .AddQueryParameter("pageSize", PageSize)
                .ToString();

            return endpoint;
        }
    }
}



