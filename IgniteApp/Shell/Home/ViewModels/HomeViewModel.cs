using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using IT.Tangdao.Framework.Abstractions.Loggers;
using Stylet.Logging;
using System.ComponentModel;
using static System.Collections.Specialized.BitVector32;
using System.IO;
using IT.Tangdao.Framework.Common;

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
        private readonly IContentAccess _contentAccess;

        private ITangdaoLogger _logger = TangdaoLogger.Get(typeof(HomeViewModel));
        #endregion

        #region--.ctor--

        public HomeViewModel(IViewFactory viewFactory, IContentAccess contentAccess)
        {
            _viewFactory = viewFactory;
            _contentAccess = contentAccess;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "unity.config");
            var responseResult = contentAccess.Default.Read(path).AsConfig().SelectSection("Tangdao")
            .ToList(kv => new HomeMenuItem()
            {
                Title = kv.Key,
                ViewModelName = kv.Value,
            });
            HomeMenuItems = new BindableCollection<HomeMenuItem>(responseResult);

            DataConfig.InitializeFromExcel();
            foreach (var item in DataConfig.SVData)
            {
                _logger.WriteLocal($"1:{item.Value}");
            }

            //  var ssssss = dicts.ToTangdaoSortedDictionary(x=>);
            List<string> plcDatas = new List<string>() { "123", "432", "34.4", "32.1" };

            DataConfig.UpdateSVReportValues(plcDatas);

            foreach (var item in DataConfig.SVData)
            {
                _logger.WriteLocal($"2:{item.Value}");
            }
        }

        #endregion

        #region--方法--

        /// <summary>
        /// 导航到具体页面
        /// </summary>
        /// <param name="viewModelName"></param>
        public void ExecuteNavigatToView(string viewModelName)
        {
            var dicts = HomeMenuItems.ToDictionary(obj => obj.ViewModelName, obj => obj.ViewModel);
            if (dicts.TryGetValue(viewModelName, out IScreen screen))
            {
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

        public void PlcData_Changed(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}