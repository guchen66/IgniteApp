using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Set.Models;
using IgniteApp.Shell.Set.Views;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class SystemSetViewModel : ViewModelBase
    {
        #region--字段--
        #endregion

        #region--属性--
        private ObservableCollection<SystemMenuItem> _systemLists;

        public ObservableCollection<SystemMenuItem> SystemLists
        {
            get => _systemLists;
            set => SetAndNotify(ref _systemLists, value);
        }

        private SystemMenuItem _systemItem;

        public SystemMenuItem SystemItem
        {
            get => _systemItem;
            set => SetAndNotify(ref _systemItem, value);
        }

        #endregion

        #region--.ctor--

        public SystemSetViewModel()
        {
            SystemLists = new ObservableCollection<SystemMenuItem>()
            {
                new SystemMenuItem(){ Id=1,Name="通用设置",CurrentView=SystemSetViewFactory.CreateView(1) },
                new SystemMenuItem(){ Id=2,Name="账户设置",CurrentView=SystemSetViewFactory.CreateView(2)},
                new SystemMenuItem(){ Id=3,Name="网络设置",CurrentView=SystemSetViewFactory.CreateView(3)},
                new SystemMenuItem(){ Id=4,Name="硬件设置", CurrentView=SystemSetViewFactory.CreateView(4)},
                new SystemMenuItem(){ Id=5,Name="辅助设置", CurrentView=SystemSetViewFactory.CreateView(5)},
            };
        }

        #endregion

        #region--方法--

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        #endregion
    }
}