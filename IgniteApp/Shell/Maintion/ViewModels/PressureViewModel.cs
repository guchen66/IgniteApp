using IgniteApp.Bases;
using IT.Tangdao.Framework.Abstractions.Navigation;
using IT.Tangdao.Framework.Abstractions.Notices;
using StyletIoC;
using System.Windows.Input;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class PressureViewModel : BaseDeviceViewModel
    {
        public ITangdaoRouter Router { get; set; }
        public IContainer _container;
        private string _tag;

        public string Tag
        {
            get => _tag;
            set => SetAndNotify(ref _tag, value);
        }

        public PressureViewModel() : base("Pressure")
        {
        }

        private void ExecuteGoForward()
        {
            Router.GoForward();
        }

        private void ExecuteGoBack()
        {
            Router.GoBack();
        }

        public ICommand GoBackCommand { get; set; }
        public ICommand GoForwardCommand { get; set; }

        public void GoToDigitalSmartGaugeView()
        {
            Router.NavigateTo<DigitalSmartGaugeViewModel>();
        }

        public void GoToVacuumGaugeView()
        {
            Router.NavigateTo<VacuumGaugeViewModel>();
        }

        public void GoToDifferentialGaugeView()
        {
            Router.NavigateTo<DifferentialGaugeViewModel>();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        public ITangdaoPage ResolvePage(string route)
        {
            var result = _container.Get<ITangdaoPage>(route);
            return result;
        }
    }
}