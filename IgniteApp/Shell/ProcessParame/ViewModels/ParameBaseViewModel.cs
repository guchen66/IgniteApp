using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoDtos.Items;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class ParameBaseViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "ProcessParameters";

        private IReadOnlyCollection<IMenuItem> _parameMenuList;

        public IReadOnlyCollection<IMenuItem> ParameMenuList
        {
            get => _parameMenuList;
            set => SetAndNotify(ref _parameMenuList, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public IReadService _readService;

        public ParameBaseViewModel(IViewFactory viewFactory, IReadService readService)
        {
            _viewFactory = viewFactory;
            _readService = readService;
            ParameMenuList = ReadOnlyMenuItemManager.Create2(readService, HandlerName);
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

        private void DoNavigateToView()
        {
            //  NavigatRouteService.GetRoute(SelectedIndex,DisplayName);
            switch (SelectedIndex)
            {
                case 0: ActivateItem(LoadCalibrationViewModel ?? (LoadCalibrationViewModel = _viewFactory.LoadCalibrationViewModel())); break;
                case 1: ActivateItem(UnLoadCalibrationViewModel ?? (UnLoadCalibrationViewModel = _viewFactory.UnLoadCalibrationViewModel())); break;
                case 2: ActivateItem(AccuracyOffsetViewModel ?? (AccuracyOffsetViewModel = _viewFactory.AccuracyOffsetViewModel())); break;
                default:
                    break;
            }
        }

        public LoadCalibrationViewModel LoadCalibrationViewModel;
        public UnLoadCalibrationViewModel UnLoadCalibrationViewModel;
        public AccuracyOffsetViewModel AccuracyOffsetViewModel;
    }
}