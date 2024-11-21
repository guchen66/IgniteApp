using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Monitor.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Calibration.ViewModels
{
    public class CalibrationViewModel : NavigatViewModel,IAppConfigProvider
    {
        public string HandlerName { get; set; } = "Calibration";

        private BindableCollection<IMenuItem> _calibrationMenuList;

        public BindableCollection<IMenuItem> CalibrationMenuList
        {
            get => _calibrationMenuList;
            set => SetAndNotify(ref _calibrationMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;

        public CalibrationViewModel(IViewFactory viewFactory)
        {
            this._viewFactory = viewFactory;
            //字典转列表
            var lists = this.ReadAppConfigToDic(HandlerName).Select(kvp => new MenuChildItem
            {
                MenuName = kvp.Value,

            }).ToList();
            CalibrationMenuList = new BindableCollection<IMenuItem>(lists);
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
    

        }

        private void DoNavigateToView()
        {
          //  NavigatRouteService.GetRoute(SelectedIndex,DisplayName);
            switch (SelectedIndex)
            {
                case 0: ActivateItem(LoadCalibrationViewModel ?? (LoadCalibrationViewModel = _viewFactory.LoadCalibrationViewModel())); break;
                case 1: ActivateItem(UnLoadCalibrationViewModel ?? (UnLoadCalibrationViewModel = _viewFactory.UnLoadCalibrationViewModel())); break;
                default:
                    break;
            }
        }

        public LoadCalibrationViewModel LoadCalibrationViewModel;
        public UnLoadCalibrationViewModel UnLoadCalibrationViewModel;
    }
}
