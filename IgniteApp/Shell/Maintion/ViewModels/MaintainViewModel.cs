using IgniteApp.Bases;
using IgniteApp.Extensions;
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
    public class MaintainViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "MaintainMenu";
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
        private readonly INavigateRoute _navigatRoute;
        public MaintainViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute)
        {
            _viewFactory = viewFactory;
            _navigatRoute = navigatRoute;
            var lists = this.ReadAppConfigToDic(HandlerName).Select(kvp => new MaintainMenuItem
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
                case 0: ActivateItem(ResistiveViewModel ?? (ResistiveViewModel = _viewFactory.ResistiveViewModel())); break;
                case 1: ActivateItem(PressureViewModel ?? (PressureViewModel = _viewFactory.PressureViewModel())); break;
                case 2: ActivateItem(ElectViewModel ?? (ElectViewModel = _viewFactory.ElectViewModel())); break;
                case 3: ActivateItem(LightViewModel ?? (LightViewModel = _viewFactory.LightViewModel())); break;
                case 4: ActivateItem(TempAndHumViewModel ?? (TempAndHumViewModel = _viewFactory.TempAndHumViewModel())); break;
                default:
                    break;
            }
        }
        public ResistiveViewModel ResistiveViewModel;
        public PressureViewModel PressureViewModel;
        public LightViewModel LightViewModel;
        public ElectViewModel ElectViewModel;
        public TempAndHumViewModel TempAndHumViewModel;

    }
}
