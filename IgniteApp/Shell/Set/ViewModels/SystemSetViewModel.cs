using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
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
        private ObservableCollection<SystemMenuItem> _systems;

        public ObservableCollection<SystemMenuItem> Systems
        {
            get => _systems;
            set => SetAndNotify(ref _systems, value);
        }

        private ObservableCollection<SystemMenuItem> _item;

        public ObservableCollection<SystemMenuItem> Item
        {
            get => _item;
            set => SetAndNotify(ref _item, value);
        }
        private SystemMenuItem _systemItem;

        public SystemMenuItem SystemItem
        {
            get => _systemItem;
            set => SetAndNotify(ref _systemItem, value);
        }

        public SystemSetViewModel()
        {
            Systems = new ObservableCollection<SystemMenuItem>()
            {
                new SystemMenuItem(){ Id=1,Name="展示",Type="11", },
                new SystemMenuItem(){ Id=1,Name="展示",Type="11",},
                new SystemMenuItem(){ Id=1,Name="展示",Type="11", },
            };

            Item = new ObservableCollection<SystemMenuItem>()
            {
                new SystemMenuItem(){ Id=1,Name="展示",Type="11" },
                new SystemMenuItem(){ Id=2,Name="展示",Type="11"},
                new SystemMenuItem(){ Id=3,Name="展示",Type="11"},
            };
        }
    }
}
