using IgniteShared.Entitys;
using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Dtos
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRemember { get; set; }
        public bool IsAdmin { get; set; }
        public RoleType Role { get; set; }
        public string IP { get; set; }
    }
}
