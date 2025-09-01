using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using IT.Tangdao.Framework.DaoCommands;
using IT.Tangdao.Framework.DaoIoc;
using StyletIoC;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class PressureViewModel : BaseDeviceViewModel, IRouteComponent
    {
        public ITangdaoRouter Router { get; set; }
        public IContainer _container;

        public PressureViewModel(ITangdaoRouter router, IContainer container) : base("Pressure")
        {
            Router = router;
            _container = container;
            Router.RouteComponent = this;
            Router.RegisterPage<DigitalSmartGaugeViewModel>();
            Router.RegisterPage<DifferentialGaugeViewModel>();
            Router.RegisterPage<VacuumGaugeViewModel>();
            GoBackCommand = MinidaoCommand.Create(ExecuteGoBack);
            GoForwardCommand = MinidaoCommand.Create(ExecuteGoForward);
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