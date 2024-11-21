using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class SetViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get ; set ; }= "Setting";

        private BindableCollection<IMenuItem> _setMenuList;

        public BindableCollection<IMenuItem> SetMenuList
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
            //字典转列表
            var lists = this.ReadAppConfigToDic(HandlerName).Select(kvp => new MenuChildItem
            {
                MenuName = kvp.Value,
              //  SetMenuToView = kvp.Value,
            }).ToList();
            SetMenuList = new BindableCollection<IMenuItem>(lists);
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
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
        private Screen _defaultScreen;

        public Screen DefaultScreen
        {
            get => _defaultScreen;
            set => SetAndNotify(ref _defaultScreen, value);
        }
       
    }
}
