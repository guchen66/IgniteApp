using IgniteApp.Events;
using IgniteDevices.PLC;
using IgniteShared.Extensions;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.DaoEvents;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace IgniteApp.Dialogs.ViewModels
{
    public class AlarmPopupViewModel : Screen, IHandle<OpenAlarmPopupEvent>//, IHandle<CloseAlarmPopupEvent>, IHandle<OpenAlarmPopupEvent>
    {
        public string Name { get; }
        public DateTime TriggerTime { get; }
        public ICommand AcknowledgeCommand { get; }
        private bool _isMinimized;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(AlarmPopupViewModel));

        public bool IsMinimized
        {
            get => _isMinimized;
            set => SetAndNotify(ref _isMinimized, value);
        }

        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;
        private IDaoEventAggregator _daoEventAggregator;

        public AlarmPopupViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, IDaoEventAggregator daoEventAggregator)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _daoEventAggregator = daoEventAggregator;
            _daoEventAggregator.SubscribeAsync<CloseAlarmPopupEvent>(ExecuteHandlerAlarm);
            _eventAggregator.Subscribe(this);
        }

        private async Task ExecuteHandlerAlarm(CloseAlarmPopupEvent @event)
        {
            Logger.WriteLocal($"关闭前AlarmPopupViewModel状态：{this.ScreenState}");
            this.RequestClose();
            Logger.WriteLocal($"关闭后AlarmPopupViewModel状态：{this.ScreenState}");
            await Task.CompletedTask;
        }

        private string _currentAlarm;

        public string CurrentAlarm
        {
            get => _currentAlarm;
            set => SetAndNotify(ref _currentAlarm, value);
        }

        public void UpdateAlarm(AlarmMessage alarm)
        {
            CurrentAlarm = alarm.Name;
            // 恢复窗口状态
            if (IsMinimized)
            {
                IsMinimized = false;
                RequestBringToFront();
            }
            // 其他属性更新...
        }

        public void RequestBringToFront()
        {
            // 通过 Stylet 的 WindowManager 控制窗口
            (this as IViewAware)?.View?.Dispatcher.Invoke(() =>
            {
                var window = (this as IViewAware).View as Window;
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
                window.Topmost = true;  // 短暂置顶确保显示
                //window.Topmost = false; // 恢复原有层级
            });
        }

        public void Handle(CloseAlarmPopupEvent message)
        {
            Logger.WriteLocal($"关闭前AlarmPopupViewModel状态：{this.ScreenState}");
            this.RequestClose();
            Logger.WriteLocal($"关闭后AlarmPopupViewModel状态：{this.ScreenState}");
        }

        public void Handle(OpenAlarmPopupEvent message)
        {
            // 确保弹窗未激活时才打开
            if (this.ScreenState != ScreenState.Active)
            {
                _windowManager.ShowWindow(this);
            }
        }

        protected override void OnClose()
        {
            // _eventAggregator.Unsubscribe(this);
            base.OnClose();
        }
    }
}