using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.Models
{
    public class AccountSetItem:DaoViewModelBase
    {
		private string _account;

		public string Account
		{
			get => _account;
			set => SetProperty(ref _account, value);
		}

		private string _password;

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		private bool _isEnabled;

		public bool IsEnabled
        {
			get => _isEnabled;
			set => SetProperty(ref _isEnabled, value);
		}

	}
}
