﻿using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.ViewModels
{
   public class MonitorViewModel : SectionViewModel
    {
        private BindableCollection<MonitorMenuItem> _monitorMenuList;

        public BindableCollection<MonitorMenuItem> MonitorMenuList
        {
            get => _monitorMenuList;
            set => SetAndNotify(ref _monitorMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public MonitorViewModel(IViewFactory viewFactory)
        {
            this._viewFactory = viewFactory;
            MonitorMenuItem Item = new MonitorMenuItem();
            //字典转列表
            var lists = Item.ReadAppConfigToDic("MonitorMenu").Select(kvp => new MonitorMenuItem
            {
                MenuName = kvp.Value,
               
            }).ToList();
            MonitorMenuList = new BindableCollection<MonitorMenuItem>(lists);
            this.Bind(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

        private void DoNavigateToView()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(ProcessViewModel ?? (ProcessViewModel = _viewFactory.ProcessViewModel())); break;
                case 1: ActivateItem(AxisArgsViewModel ?? (AxisArgsViewModel = _viewFactory.AxisArgsViewModel())); break;
                case 2: ActivateItem(SystemSetViewModel ?? (SystemSetViewModel = _viewFactory.SystemSetViewModel())); break;
                default:
                    break;
            }
        }
        public ProcessViewModel ProcessViewModel;
        public AxisArgsViewModel AxisArgsViewModel;
        public SystemSetViewModel SystemSetViewModel;

    }
}
