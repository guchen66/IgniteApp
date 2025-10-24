using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT.Tangdao.Framework.EventArg;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class ParameBaseViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "ProcessParameters";

        private IReadOnlyCollection<ITangdaoMenuItem> _parameMenuList;

        public IReadOnlyCollection<ITangdaoMenuItem> ParameMenuList
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
            //   ParameMenuList = ReadOnlyMenuItemManager.Create(readService, HandlerName);

            ParameMenuList = readService.Default.AsConfig().SelectAppConfig(HandlerName).ToList(v => new TangdaoMenuItem { MenuName = v }).ToObservableCollection();
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

        private void DoNavigateToView()
        {
            //ActionActivate.ExecuteActivation(ActivateItem, _viewFactory.CreateViewModel(HandlerName), SelectedIndex);
            //  NavigatRouteService.GetRoute(SelectedIndex,DisplayName);
            switch (SelectedIndex)
            {
                case 0: ActivateItem(LoadCalibrationViewModel ?? (LoadCalibrationViewModel = _viewFactory.LoadCalibrationViewModel())); break;
                case 1: ActivateItem(UnLoadCalibrationViewModel ?? (UnLoadCalibrationViewModel = _viewFactory.UnLoadCalibrationViewModel())); break;
                case 2: ActivateItem(AccuracyOffsetViewModel ?? (AccuracyOffsetViewModel = _viewFactory.AccuracyOffsetViewModel())); break;
                case 3: ActivateItem(TeachingViewModel ?? (TeachingViewModel = _viewFactory.TeachingViewModel())); break;
                default:
                    break;
            }
            Get(s1: "s", s2: "ss");
        }

        public LoadCalibrationViewModel LoadCalibrationViewModel;
        public UnLoadCalibrationViewModel UnLoadCalibrationViewModel;
        public AccuracyOffsetViewModel AccuracyOffsetViewModel;
        public TeachingViewModel TeachingViewModel;

        public void Get(string s1, string s2)
        {
        }

        public void Get(string s1, object s2)
        {
        }
    }

    public delegate void ActivateItemDelegate<T>(T item);

    public class ActionActivate
    {
        public static void ExecuteActivation<T>(ActivateItemDelegate<T> activator, T item, int i)
        {
            activator(item);
        }
    }
}