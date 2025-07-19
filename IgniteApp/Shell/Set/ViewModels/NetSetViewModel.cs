using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class NetSetViewModel:ViewModelBase
    {
		private NetSetItem _netSetItem;

		public NetSetItem NetSetItem
        {
			get => _netSetItem;
			set => SetAndNotify(ref _netSetItem, value);
		}

	}
}
