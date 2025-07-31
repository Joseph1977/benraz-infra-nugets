namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// Custom role.
    /// </summary>
    public static class CustomRole
    {
        /// <summary>
        /// Admin role (check 'ADMIN' words include in user roles).
        /// </summary>
        public static readonly string ADMIN = "ADMIN";

        /// <summary>
        /// Employee role (check 'EMPLOYEE' words include in user roles).
        /// </summary>
        public static readonly string EMPLOYEE = "EMPLOYEE";
    }
}
