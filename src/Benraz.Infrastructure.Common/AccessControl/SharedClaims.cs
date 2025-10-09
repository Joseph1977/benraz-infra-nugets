namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// Contains shared claims constants used across multiple services.
    /// </summary>
    public static class SharedClaims
    {
        /// <summary>
        /// Read employee user claim.
        /// </summary>
        public const string EMPLOYEE_USER_READ = "authorization-user-read-employee";

        /// <summary>
        /// Read admin user claim.
        /// </summary>
        public const string ADMIN_USER_READ = "authorization-user-read-admin";
    }
}
