using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT.Tangdao.Framework.EventArg;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class SetViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "Setting";

        private IReadOnlyCollection<ITangdaoMenuItem> _setMenuList;

        public IReadOnlyCollection<ITangdaoMenuItem> SetMenuList
        {
            get => _setMenuList;
            set => SetAndNotify(ref _setMenuList, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public IReadService _readService;

        public SetViewModel(IViewFactory viewFactory, IReadService readService)
        {
            this._viewFactory = viewFactory;
            this._readService = readService;
            SetMenuList = ReadOnlyMenuItemManager.Create(readService, HandlerName);
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

        private void DoNavigateToView()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(ProcessViewModel ?? (ProcessViewModel = _viewFactory.ProcessViewModel())); break;
                case 1: ActivateItem(AxisArgsViewModel ?? (AxisArgsViewModel = _viewFactory.AxisArgsViewModel())); break;
                case 2: ActivateItem(SystemSetViewModel ?? (SystemSetViewModel = _viewFactory.SystemSetViewModel())); break;
                default:
                    break;
            }
        }

        public ProcessViewModel ProcessViewModel;
        public AxisArgsViewModel AxisArgsViewModel;
        public SystemSetViewModel SystemSetViewModel;
        private Screen _defaultScreen;

        public Screen DefaultScreen
        {
            get => _defaultScreen;
            set => SetAndNotify(ref _defaultScreen, value);
        }
    }
}