using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Infrastructure;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class MonitorViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "MonitorMenu";
        private IReadOnlyCollection<ITangdaoMenuItem> _monitorMenuList;

        public IReadOnlyCollection<ITangdaoMenuItem> MonitorMenuList
        {
            get => _monitorMenuList;
            set => SetAndNotify(ref _monitorMenuList, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public readonly IViewFactory _viewFactory;
        private readonly INavigateRoute _navigatRoute;
        private readonly IContentAccess _contentAccess;

        public MonitorViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute, IContentAccess contentAccess)
        {
            _viewFactory = viewFactory;
            _navigatRoute = navigatRoute;
            _contentAccess = contentAccess;

            MonitorMenuList = contentAccess.Default.Empty().AsConfig().SelectAppSection(HandlerName)
                .ToList(kv => new TangdaoMenuItem
                {
                    MenuName = kv.Value
                }).ToObservableCollection();
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "unity.config");
            //MonitorMenuList = contentAccess.Default.Read(path).AsConfig().SelectSection(HandlerName)
            //    .ToList(kv => new TangdaoMenuItem
            //    {
            //        MenuName = kv.Value
            //    }).ToObservableCollection();

            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

        private void DoNavigateToView()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(IOMonViewModel ?? (IOMonViewModel = _viewFactory.IOMonViewModel())); break;
                case 1: ActivateItem(AxisMonViewModel ?? (AxisMonViewModel = _viewFactory.AxisMonViewModel())); break;
                case 2: ActivateItem(PlcMonViewModel ?? (PlcMonViewModel = _viewFactory.PlcMonViewModel())); break;
                case 3: ActivateItem(ReportViewModel ?? (ReportViewModel = _viewFactory.ReportViewModel())); break;
                default:
                    break;
            }
        }

        public IOMonViewModel IOMonViewModel;
        public AxisMonViewModel AxisMonViewModel;
        public PlcMonViewModel PlcMonViewModel;
        public ReportViewModel ReportViewModel;
    }
}