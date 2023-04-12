using System;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Domain.Common
{
    /// <summary>
    /// Profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Organization identifier.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Organization name.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Is agent.
        /// </summary>
        public bool IsAgent { get; set; }

        /// <summary>
        /// Organization KYC status.
        /// </summary>
        public bool OrgKycStatus { get; set; }

        /// <summary>
        /// Create time UTC.
        /// </summary>
        public DateTime CreateTimeUtc { get; set; }
    }
}
