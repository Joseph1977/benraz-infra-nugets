using System.Collections.Generic;
using System.Linq;

namespace Benraz.Infrastructure.Common.AccessControl
{
    /// <summary>
    /// User access validator to check whether the user has access to employee and admin data.
    /// </summary>
    public static class UserAccessValidator
    {
        /// <summary>
        /// Determines whether the user has access to employee and admin data.
        /// </summary>
        /// <param name="isIncludeEmployee">Indicates whether employees should be included (True: include employees; False: exclude employees).</param>
        /// <param name="isIncludeAdmin">Indicates whether admin should be included (True: include admin; False: exclude admin).</param>
        /// <param name="userRoles">User roles.</param>
        /// <returns>True/False</returns>
        public static bool IsUserAccessAllowed(bool isIncludeEmployee, bool isIncludeAdmin, List<string> userRoles)
        {
            var upperRoles = userRoles.Select(r => r.ToUpperInvariant()).ToList();
            bool isTargetEmployee = upperRoles.Any(r => r.Contains(CustomRole.EMPLOYEE));
            bool isTargetAdmin = upperRoles.Any(r => r.Contains(CustomRole.ADMIN));

            // If neither claim is present and user is ADMIN or EMPLOYEE, deny access
            if (!isIncludeEmployee && !isIncludeAdmin)
            {
                if (isTargetEmployee || isTargetAdmin)
                    return false;
            }
            else
            {
                // Check specific claim/role combinations
                bool hasOnlyEmployeeClaim = isIncludeEmployee && !isIncludeAdmin;
                bool hasOnlyAdminClaim = !isIncludeEmployee && isIncludeAdmin;
                bool hasBothClaims = isIncludeEmployee && isIncludeAdmin;
                bool targetHasBothRoles = isTargetEmployee && isTargetAdmin;

                if (hasBothClaims)
                {
                    // If user has both claims and target user does NOT have admin or employee roles → Allow
                    if (!isTargetEmployee && !isTargetAdmin)
                        return true;
                    // If user has both claims → Allow any combination (existing logic)
                    return true;
                }
                else if (hasOnlyEmployeeClaim)
                {
                    // If user has only employee claim and target user is employee+admin role → Not allowed
                    if (targetHasBothRoles)
                        return false;
                    // If user has only employee claim and target user is admin role only → Not allowed
                    if (isTargetAdmin && !isTargetEmployee)
                        return false;
                    // If user has only employee claim and target user is employee role or other role (not admin) → Allow
                }
                else if (hasOnlyAdminClaim)
                {
                    // If user has only admin claim and target user is employee+admin role → Not allowed
                    if (targetHasBothRoles)
                        return false;
                    // If user has only admin claim and target user is employee role only → Not allowed
                    if (isTargetEmployee && !isTargetAdmin)
                        return false;
                    // If user has only admin claim and target user is admin role or other role (not employee) → Allow
                }
            }

            return true;
        }
    }
}
