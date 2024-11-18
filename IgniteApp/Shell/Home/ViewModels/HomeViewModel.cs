using HandyControl.Controls;
using HandyControl.Tools.Extension;
using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoCommands;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class HomeViewModel: NavigatViewModel
    {      
        private BindableCollection<HomeMenuItem> _homeMenuItems;

        public BindableCollection<HomeMenuItem> HomeMenuItems
        {
            get => _homeMenuItems ?? (_homeMenuItems = new BindableCollection<HomeMenuItem>());
            set => SetAndNotify(ref _homeMenuItems, value);
        }
        private Screen _defaultScreen;

        public Screen DefaultScreen
        {
            get => _defaultScreen;
            set => SetAndNotify(ref _defaultScreen, value);
        }

        private readonly IViewFactory _viewFactory;
        public HomeViewModel(IViewFactory viewFactory)
        {
            InitMenuData();
         
            _viewFactory = viewFactory;
        }

        private void InitMenuData()
        {           
            HomeMenuItem menuItem = new HomeMenuItem();
            var dicts = menuItem.ReadUnityConfig("MenuConfiguration");
            List<HomeMenuItem> menuItems = dicts.Select(kvp => new HomeMenuItem
            {
                Title = kvp.Key,
                ViewModelName =  kvp.Value

            }).ToList();

            HomeMenuItems.AddRange(menuItems);
            
        }

        public void ExecuteNavigatToView(string viewModelName)
        {
            var dicts=HomeMenuItems.ToDictionary(obj=>obj.ViewModelName, obj=>obj.ViewModel);
            if (dicts.ContainsKey(viewModelName))
            {
                dicts.TryGetValue(viewModelName, out IScreen screen);
                ActivateItem(screen ?? (screen = _viewFactory.CreateViewModel(viewModelName)));
            }
        }

        /// <summary>
        /// 默认打开首页
        /// </summary>
        /// <param name="screen"></param>
        public void ExecuteLoad(Screen screen)
        {
            ActivateItem(screen ?? (screen = _viewFactory.DefaultViewModel()));
        }
    }
}
