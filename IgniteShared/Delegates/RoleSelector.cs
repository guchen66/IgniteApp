using IgniteShared.Enums;
using System;

namespace IgniteShared.Delegates
{
    public delegate RoleType RoleSelector(RoleType adminRole, RoleType normalRole);

    public delegate Func<RoleType, RoleType, RoleType> RoleSelector2(bool isAdmin);

    public class RoleSelectors
    {
        public static Func<RoleType, RoleType, RoleType> GetRoleSelector(bool isAdmin)
        {
            return (adminRole, normalRole) => isAdmin ? adminRole : normalRole;
        }

        // 判断是否是 Admin（可复用）
        public static bool DetermineIfAdmin(string userName)
        {
            return userName.Contains("Admin");
        }
    }
}