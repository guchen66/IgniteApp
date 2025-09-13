using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using IT.Tangdao.Framework.Abstractions.IServices;
using IT.Tangdao.Framework.Parameters.Infrastructure;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT.Tangdao.Framework.Parameters.EventArg;

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
        private readonly IReadService _readService;

        public MonitorViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute, IReadService readService)
        {
            _viewFactory = viewFactory;
            _navigatRoute = navigatRoute;
            _readService = readService;
            MonitorMenuList = ReadOnlyMenuItemManager.Create(readService, HandlerName);

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