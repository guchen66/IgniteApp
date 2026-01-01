using IgniteShared.Enums;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 数据库实体类
    /// </summary>
    public class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRemember { get; set; }
        public bool IsAdmin { get; set; }
        public RoleType Role { get; set; }
        public string IP { get; set; }
    }
}