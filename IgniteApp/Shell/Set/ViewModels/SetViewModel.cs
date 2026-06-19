using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Messaging;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Infrastructure;
using Stylet;
using System.Collections.ObjectModel;
using System.Linq;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class SetViewModel : NavigatViewModel, IAppConfigProvider
    {
        public string HandlerName { get; set; } = "Setting";

        private ObservableCollection<TangdaoMenuItem> _setMenuList;

        public ObservableCollection<TangdaoMenuItem> SetMenuList
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
        public IContentAccess _contentAccess;
        public ObserverBuilder _noticeBuilder;

        public SetViewModel(IViewFactory viewFactory, IContentAccess contentAccess, ObserverBuilder noticeBuilder)
        {
            _viewFactory = viewFactory;
            _contentAccess = contentAccess;
            _noticeBuilder = noticeBuilder;
            SetMenuList = contentAccess.Default.Empty().AsConfig().SelectAppSection(HandlerName).ToList(kv => new TangdaoMenuItem { MenuName = kv.Value }).ToObservableCollection();
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