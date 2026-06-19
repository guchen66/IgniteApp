using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Shell.Aside.ViewModels;
using IgniteApp.Shell.Footer.Models;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Shell.Header.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using IgniteDevices.PLC;
using IgniteShared.Globals.Common;
using IT.Tangdao.Framework.Abstractions.Messaging;
using TangdaoEvents = IT.Tangdao.Framework.Events;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private ITangdaoNotifier _tangdaoNotifier;

        private ITangdaoPublisher _tangdaoPublisher;

        public AlarmPopupManager _alarmPopupManager;
        #endregion

        #region--ctor--

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, AlarmPopupManager alarmPopupManager, ITangdaoPublisher tangdaoPublisher, ITangdaoNotifier tangdaoNotifier)
        {
            // FooterViewModel = footerViewModel;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            _alarmPopupManager = alarmPopupManager;
            // OmronManager.AlarmChenged += OnAlarmChanged;
            OmronManager.AlarmChenged += OnAlarmChangedPublish;
            OmronManager.AlarmErrorChenged += OnAlarmErrorChanged;

            TangdaoEvents.TangdaoWeakEvent.Instance.OnMessageReceived += OnMessageReceived;
            TangdaoEvents.TangdaoWeakEvent.Instance.OnKeyMessageReceived += OnKeyMessageReceived;
            _tangdaoPublisher = tangdaoPublisher;
            _tangdaoNotifier = tangdaoNotifier;
            _tangdaoPublisher.Subscribe(_tangdaoNotifier);
        }

        /// <summary>
        /// 带Key的弱引用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyMessageReceived(object sender, TangdaoEvents.KeyMessageEventArgs e)
        {
            Console.WriteLine(e.MessageEventArgs);
        }

        /// <summary>
        /// 普通的弱引用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessageReceived(object sender, TangdaoEvents.MessageEventArgs e)
        {
            Console.WriteLine(e.NowTime);
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