using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Set.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class MonitorViewModel : NavigatViewModel,IAppConfigProvider
    {
        public string HandlerName { get; set; } = "MonitorMenu";
        private BindableCollection<IMenuItem> _monitorMenuList;

        public BindableCollection<IMenuItem> MonitorMenuList
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

        public IViewFactory _viewFactory;
        private readonly INavigateRoute _navigatRoute;
        public MonitorViewModel(IViewFactory viewFactory, INavigateRoute navigatRoute)
        {
            this._viewFactory = viewFactory;
            //字典转列表
            var lists = this.ReadAppConfigToDic(HandlerName).Select(kvp => new MenuChildItem
            {
                MenuName = kvp.Value,

            }).ToList();
            MonitorMenuList = new BindableCollection<IMenuItem>(lists);
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
            _navigatRoute = navigatRoute;
           
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
