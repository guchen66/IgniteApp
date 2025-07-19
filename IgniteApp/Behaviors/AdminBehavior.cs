using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using IgniteShared.Globals.System;

namespace IgniteApp.Behaviors
{
    public class AdminOnlyBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            //只有管理员才允许删除
            var isAdmin = SysLoginInfo.Role == RoleType.管理员;
            AssociatedObject.IsEnabled = isAdmin;
            //AssociatedObject.Visibility = isAdmin
            //    ? Visibility.Visible
            //    : Visibility.Collapsed;
        }
    }
}