﻿using IgniteApp.Bases;
using IgniteApp.Shell.Monitor.Models;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class IOMonViewModel:ControlViewModelBase
    {
		private string[] _selectedMode;

		public string[] SelectedMode
        {
			get => _selectedMode;
			set => SetAndNotify(ref _selectedMode, value);
        }

        private string _selectItem;

        public string SelectItem
        {
            get => _selectItem;
            set => SetAndNotify(ref _selectItem, value);
        }

        private BindableCollection<IOMonItem> _iOMonItems;

        public BindableCollection<IOMonItem> IOMonItems
        {
            get => _iOMonItems;
            set => SetAndNotify(ref _iOMonItems, value);
        }

        public IOMonViewModel()
        {
            SelectedMode=new string[] 
            {
                "全部","Load","UpLoad"
            };

            this.Bind(viewModel=>viewModel.SelectItem,(obj,sender)=>DoExecute());
        }

        private void DoExecute()
        {
            var s1 = SelectItem;
        }

    
    }
}
