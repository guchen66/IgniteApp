using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class AccountSetViewModel:ViewModelBase
    {
		private AccountSetItem _accountSetItem;

		public AccountSetItem AccountSetItem
        {
			get => _accountSetItem??(_accountSetItem=new AccountSetItem());
			set => SetAndNotify(ref _accountSetItem, value);
		}

        public AccountSetViewModel()
        {
            AccountSetItem.Account=SysLoginInfo.UserName;
            AccountSetItem.Password=SysLoginInfo.Password;
        }
    }
}
