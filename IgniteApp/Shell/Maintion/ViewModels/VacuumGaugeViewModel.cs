using HandyControl.Controls;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Ioc;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class VacuumGaugeViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "真空压力表2222";

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            DisplayName = "真空压力表";
        }
    }
}