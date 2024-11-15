using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Maintion.Models;
using IgniteApp.Shell.Set.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class MaintainViewModel : NavigatViewModel
    {
        public override string Name => "维护";
        private BindableCollection<MaintainMenuItem> _maintainMenuList;

        public BindableCollection<MaintainMenuItem> MaintainMenuList
        {
            get => _maintainMenuList;
            set => SetAndNotify(ref _maintainMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public MaintainViewModel(IViewFactory viewFactory)
        {
            this._viewFactory = viewFactory;
            MaintainMenuItem Item = new MaintainMenuItem();
            
            var lists = Item.ReadAppConfigToDic("MaintainMenu").Select(kvp => new MaintainMenuItem
            {
                MenuName = kvp.Value,
               
            }).ToList();
            MaintainMenuList = new BindableCollection<MaintainMenuItem>(lists);
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

    }
}
