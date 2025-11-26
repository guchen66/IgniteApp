using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT.Tangdao.Framework.EventArg;
using IT.Tangdao.Framework.Abstractions;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class MaintainViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "MaintainMenu";
        private IReadOnlyCollection<ITangdaoMenuItem> _maintainMenuList;

        public IReadOnlyCollection<ITangdaoMenuItem> MaintainMenuList
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

        private readonly IViewFactory _viewFactory;
        private readonly INavigateRoute _navigatRoute;
        private readonly IContentReader _readService;

        public MaintainViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute, IContentReader readService)
        {
            _viewFactory = viewFactory;
            _navigatRoute = navigatRoute;
            _readService = readService;
            //MaintainMenuList = ReadOnlyMenuItemManager.Create(readService, HandlerName);
            //MaintainMenuList = ReadOnlyMenuItemManager.Create(readService, HandlerName);
            MaintainMenuList = readService.Default.AsConfig().SelectAppConfig(HandlerName).ToList(v => new TangdaoMenuItem { MenuName = v }).ToObservableCollection();
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