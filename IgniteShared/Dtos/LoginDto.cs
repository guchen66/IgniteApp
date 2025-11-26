using IgniteShared.Entitys;
using IgniteShared.Enums;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Attributes;
using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Dtos
{
    public class LoginDto : DaoViewModelBase
    {
        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                if (_userName != null && _userName == "Admin")
                {
                    Role = RoleType.管理员;
                    IsAdmin = true;
                }
                else
                {
                    Role = RoleType.普通用户;
                    IsAdmin = false;
                }
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isRemember;

        public bool IsRemember
        {
            get => _isRemember;
            set => SetProperty(ref _isRemember, value);
        }

        private bool _isAdmin;

        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        public RoleType Role { get; set; }
        public string IP { get; set; }
    }
}