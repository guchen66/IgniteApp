using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Shell.Aside.ViewModels;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Shell.Header.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using IgniteDevices.PLC;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IgniteApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region--属性--

        [Inject]
        public HeaderViewModel HeaderViewModel { get; set; }

        [Inject]
        public AsideViewModel AsideViewModel { get; set; }

        [Inject]
        public HomeViewModel HomeViewModel { get; set; }

        [Inject]
        public FooterViewModel FooterViewModel { get; set; }

        public IEventAggregator _eventAggregator;

        // [Inject]
        public IWindowManager _windowManager;

        private AlarmPopupNotifier _alarmPopupNotifier;

        // [Inject]
        public readonly AlarmPublisher _alarmPublisher;

        public AlarmPopupManager _alarmPopupManager;
        #endregion

        #region--ctor--

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, AlarmPublisher alarmPublisher, AlarmPopupNotifier alarmPopupNotifier, AlarmPopupManager alarmPopupManager)
        {
            // FooterViewModel = footerViewModel;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _alarmPublisher = alarmPublisher;
            _alarmPopupNotifier = alarmPopupNotifier;
            _alarmPopupManager = alarmPopupManager;
            // OmronManager.AlarmChenged += OnAlarmChanged;
            OmronManager.AlarmChenged += OnAlarmChangedPublish;
            OmronManager.AlarmErrorChenged += OnAlarmErrorChanged;
            _alarmPublisher.Subscribe(_alarmPopupNotifier);
        }

        private void OnAlarmErrorChanged(AlarmMessage message)
        {
            _alarmPopupManager.OpenAlarmPopup(message);
        }

        /// <summary>
        /// 全局接收PLC报警事件（使用观察者模式）
        /// </summary>
        /// <param name="name"></param>
        private void OnAlarmChanged(string name)
        {
            _alarmPublisher.NotifyAlarm(name);
        }

        /// <summary>
        /// 全局接收PLC报警事件（使用发布订阅模式）
        /// </summary>
        /// <param name="name"></param>
        private void OnAlarmChangedPublish(string name)
        {
            _alarmPopupManager.OpenAlarmPopup();
        }

        #endregion

        #region--方法--

        public void ShowLoginScreen()
        {
            // ActivateItem(new LoginViewModel(this));
        }

        public void ShowMainScreen()
        {
            // ActivateItem(new MainViewModel());
            // DeactivateItem(this.ActiveItem); // 关闭当前激活的ViewModel
        }

        #endregion

        #region--view--

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        #endregion
    }
}