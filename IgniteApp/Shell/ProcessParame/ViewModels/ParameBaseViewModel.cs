using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoDtos.Items;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class ParameBaseViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "ProcessParameters";

        private BindableCollection<IMenuItem> _parameMenuList;

        public BindableCollection<IMenuItem> ParameMenuList
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

            //字典转列表
            var result = _readService.Current.SelectConfig(HandlerName).Result;

            if (result is Dictionary<string, string> d1)
            {
                var lists = d1.TryOrderBy().Select(kvp => new TangdaoMenuItem
                {
                    MenuName = kvp.Value,
                }).ToList();
                ParameMenuList = new BindableCollection<IMenuItem>(lists);
            }

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