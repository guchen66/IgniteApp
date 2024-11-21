using IgniteApp.Common;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoMvvm;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public class SetMenuItem:DaoViewModelBase,IMenuItem
    {
        public int Id { get; set; }
		private string _setMenuName;

		public string SetMenuName
        {
			get => _setMenuName;
			set => SetProperty(ref _setMenuName, value);
		}

        private string _setMenuToView;

        public string SetMenuToView
        {
            get => _setMenuToView;
            set => SetProperty(ref _setMenuToView, value);
        }
        public string MenuName { get ; set ; }
    }

    
}
