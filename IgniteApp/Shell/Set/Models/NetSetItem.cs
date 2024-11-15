using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.Models
{
    public class NetSetItem:DaoViewModelBase
    {
		private string _account;

		public string Account
        {
			get => _account;
			set => SetProperty(ref _account, value);
		}

		private string _type;

		public string Type
		{
			get => _type;
			set => SetProperty(ref _type, value);
		}

		private string _port;

		public string Port
		{
			get => _port;
			set => SetProperty(ref _port, value);
		}

		private string _address;

		public string Address
		{
			get => _address;
			set => SetProperty(ref _address, value);
		}

	}
}
