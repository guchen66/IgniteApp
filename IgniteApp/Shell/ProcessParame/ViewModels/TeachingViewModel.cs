using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Tests;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Ioc;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class TeachingViewModel : Screen, IRouteComponent
    {
        public ITangdaoRouter Router { get; set; }

        public IContainer _container;
        public string RouteName { get; set; }

        public TeachingViewModel(ITangdaoRouter router, IContainer container)
        {
            Router = router;
            _container = container;
            Router.RouteComponent = this;
            Router.RegisterPage<CO2TeachViewModel>();
            Router.RegisterPage<UVTeachViewModel>();
            Router.RegisterPage<IRTeachViewModel>();
        }

        public void OpenTeachView(string navigateName)
        {
            ITangdaoParameter tangdaoParameter = new TangdaoParameter();
            tangdaoParameter.Add("Name", "张三");
            Router.NavigateTo(navigateName, tangdaoParameter);
            //  Router.NavigateTo<CO2TeachViewModel>();
        }

        public ITangdaoPage ResolvePage(string route)
        {
            var result = _container.Get<ITangdaoPage>(route);
            return result;
        }

        private string _name = "未设置";

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public string PageTitle => "";
    }
}