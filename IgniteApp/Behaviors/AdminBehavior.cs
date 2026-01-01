using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace IgniteApp.Behaviors
{
    public class AdminOnlyBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            //只有管理员才允许删除
            // var isAdmin = SysLoginInfo.Role == RoleType.管理员;
            // AssociatedObject.IsEnabled = isAdmin;
            //AssociatedObject.Visibility = isAdmin
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;
        }
    }
}