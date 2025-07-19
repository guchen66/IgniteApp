using IgniteDevices.PLC;
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
    public class AlarmPopupViewModel : Screen
    {
        public string Name { get; }
        public DateTime TriggerTime { get; }
        public ICommand AcknowledgeCommand { get; }
        private bool _isMinimized;

        public bool IsMinimized
        {
            get => _isMinimized;
            set => SetAndNotify(ref _isMinimized, value);
        }

        //public AlarmPopupViewModel(AlarmMessage alarm)
        //{
        //    Name = alarm.Name;
        //    TriggerTime = alarm.TriggerTime;
        //    //AcknowledgeCommand = new RelayCommand(ClosePopup);
        //}
        public AlarmPopupViewModel()
        {
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

        // 关闭弹窗时调用
        public void CloseAlarm()
        {
            this.RequestClose();
        }
    }
}