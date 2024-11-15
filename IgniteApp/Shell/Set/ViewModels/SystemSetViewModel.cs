using IgniteApp.Bases;
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
    public class SystemSetViewModel:ControlViewModelBase
    {
        #region--字段--
        #endregion
        #region--属性--
        #endregion
        #region--.ctor--
        #endregion
        #region--方法--
        #endregion
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

        public SystemSetViewModel()
        {
            SystemLists = new ObservableCollection<SystemMenuItem>()
            {
                new SystemMenuItem(){ Id=1,Name="通用设置",CurrentView=new CommonSetView() },
                new SystemMenuItem(){ Id=2,Name="账户设置",CurrentView=new AccountSetView()},
                new SystemMenuItem(){ Id=3,Name="网络设置",CurrentView=new NetSetView()},           
                new SystemMenuItem(){ Id=4,Name="辅助设置", CurrentView=new AssistSetView()},
            };       
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }
    }
}
