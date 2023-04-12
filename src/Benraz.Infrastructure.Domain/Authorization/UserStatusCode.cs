namespace Benraz.Infrastructure.Domain.Authorization
{
    /// <summary>
    /// User status code.
    /// </summary>
    public enum UserStatusCode
    {
        /// <summary>
        /// Active.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Suspended.
        /// </summary>
        Suspended = 2,

        /// <summary>
        /// Blocked.
        /// </summary>
        Blocked = 3,

        /// <summary>
        /// Payment service suspended.
        /// </summary>
        PaymentServiceSuspended = 4
    }
}




