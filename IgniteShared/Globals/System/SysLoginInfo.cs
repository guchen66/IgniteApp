using IgniteShared.Enums;

namespace IgniteShared.Globals.System
{
    /// <summary>
    /// 全局静态变量
    /// </summary>
    public class SysLoginInfo
    {
        public static long Id { get; set; }

        //用户名
        public static string UserName { get; set; }

        //角色
        public static RoleType Role { get; set; }

        public static string Password { get; set; }

        public static string IP { get; set; }
    }
}