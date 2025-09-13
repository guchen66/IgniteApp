using HandyControl.Controls;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Tests;
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
using System.Windows.Threading;

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