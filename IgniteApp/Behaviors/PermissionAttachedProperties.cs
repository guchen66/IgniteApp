using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Behaviors
{
    public static class PermissionAttachedProperties
    {
        // 定义附加属性来标记管理员用户
        public static readonly DependencyProperty IsAdministratorProperty =
            DependencyProperty.RegisterAttached(
                "IsAdministrator",
                typeof(bool),
                typeof(PermissionAttachedProperties),
                new PropertyMetadata(false));

        public static void SetIsAdministrator(DependencyObject obj, bool value)
            => obj.SetValue(IsAdministratorProperty, value);

        public static bool GetIsAdministrator(DependencyObject obj)
            => (bool)obj.GetValue(IsAdministratorProperty);
    }

    public static class PermissionValidator
    {
        private static readonly HashSet<string> _adminUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Admin", "SuperUser", "SystemAdmin"
    };

        // 主要的权限验证方法
        public static bool ValidateAdminPermission(string userName)
        {
            return _adminUsers.Contains(userName?.Trim());
        }

        // 为UI元素附加管理员权限
        public static void AttachAdminPermission(FrameworkElement element, string userName)
        {
            bool isAdmin = ValidateAdminPermission(userName);
            PermissionAttachedProperties.SetIsAdministrator(element, isAdmin);
        }
    }
}