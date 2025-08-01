namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// Contains shared claims constants used across multiple services.
    /// </summary>
    public static class SharedClaims
    {
        /// <summary>
        /// Read employee claim.
        /// </summary>
        public const string EMPLOYEE_READ = "authorization-employee-read";
    }
}
