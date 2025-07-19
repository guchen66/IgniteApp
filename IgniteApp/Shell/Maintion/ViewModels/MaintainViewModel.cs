using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoDtos.Items;
using IT.Tangdao.Framework.Extensions;
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
        private BindableCollection<IMenuItem> _maintainMenuList;

        public BindableCollection<IMenuItem> MaintainMenuList
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
        private readonly IReadService _readService;

        public MaintainViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute, IReadService readService)
        {
            _viewFactory = viewFactory;
            _navigatRoute = navigatRoute;
            _readService = readService;

            //读取AppConfig文件
            var result = _readService.Current.SelectConfig(HandlerName).Result;

            if (result is Dictionary<string, string> d1)
            {
                var lists = d1.TryOrderBy().Select(kvp => new TangdaoMenuItem
                {
                    MenuName = kvp.Value,
                }).ToList();
                MaintainMenuList = new BindableCollection<IMenuItem>(lists);
            }

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