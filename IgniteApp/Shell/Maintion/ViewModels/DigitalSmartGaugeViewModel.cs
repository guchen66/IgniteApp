using HandyControl.Controls;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
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
    public class DigitalSmartGaugeViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "数字智能测量仪222";
        private IContentReader _readService;

        public DigitalSmartGaugeViewModel(IContentReader readService)
        {
            _readService = readService;
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            DisplayName = "数字智能测量仪";
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}