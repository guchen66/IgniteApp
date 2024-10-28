using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Local
{
    /// <summary>
    /// 保存在本地的登录用户
    /// 在不启用数据库的情况下，可以对外使用本地数据
    /// </summary>
    public class LoginInfoLocation
    {
        public const string LoginPath = "E://IgniteDatas//loginInfo.xml";

    }
}
