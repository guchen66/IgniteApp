using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using IgniteApp.ViewModels;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class SetViewModel: SectionViewModel
    {
        private BindableCollection<SetMenuItem> _setMenuList;

        public BindableCollection<SetMenuItem> SetMenuList
        {
            get => _setMenuList;
            set => SetAndNotify(ref _setMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public SetViewModel(IViewFactory viewFactory)
        {
            this._viewFactory = viewFactory;
            SetMenuItem setMenuItem = new SetMenuItem();
            //字典转列表
            var lists=setMenuItem.ReadAppConfigToDic("SetMenu").Select(kvp=>new SetMenuItem 
            {
                MenuName=kvp.Value,
                SetMenuToView=kvp.Value,
            }).ToList();
            SetMenuList = new BindableCollection<SetMenuItem>(lists);
            this.Bind(viewModel=>viewModel.SelectedIndex, (obj,args)=>DoNavigateToView());
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
