using HandyControl.Controls;
using HandyControl.Tools.Extension;
using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoAdmin.IServices;
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
    public class HomeViewModel : NavigatViewModel
    {
        #region--属性--
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
        private readonly IReadService _readService;
        #endregion

        #region--.ctor--

        public HomeViewModel(IViewFactory viewFactory, IReadService readService)
        {
            _viewFactory = viewFactory;
            _readService = readService;
            InitMenuData();
        }

        #endregion

        #region--方法--

        private void InitMenuData()
        {
            var model = _readService.Current.SelectCustomConfig("unity.config", "Tangdao").Result;
            if (model is Dictionary<string, string> dicts)
            {
                List<HomeMenuItem> menuItems = dicts.Select(kvp => new HomeMenuItem
                {
                    Title = kvp.Key,
                    ViewModelName = kvp.Value
                }).ToList();
                HomeMenuItems.AddRange(menuItems);
            }
        }

        /// <summary>
        /// 导航到具体页面
        /// </summary>
        /// <param name="viewModelName"></param>
        public void ExecuteNavigatToView(string viewModelName)
        {
            var dicts = HomeMenuItems.ToDictionary(obj => obj.ViewModelName, obj => obj.ViewModel);
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

        #endregion
    }
}