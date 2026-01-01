using HandyControl.Controls;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigation;
using Stylet;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class DifferentialGaugeViewModel : Screen, ITangdaoPage
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            private set => SetAndNotify(ref _currentView, value);
        }

        public string PageTitle => "";
        private string _name;

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public DifferentialGaugeViewModel()
        {
            Name = "Hello";
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedTo(ITangdaoParameter parameter)
        {
        }

        public void OnNavigatedFrom()
        {
        }

        public void ExecuteSet()
        {
            MessageBox.Success("成功");
        }

        //private void RefreshNavigationState()
        //{
        //    NotifyOfPropertyChange(() => CanPrevious);
        //    NotifyOfPropertyChange(() => CanNext);
        //}
    }
}