using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Dialogs.Views;
using IgniteDevices.PLC;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Common
{
    public class AlarmPopupNotifier : IObserver<AlarmMessage>
    {
        private readonly IWindowManager _windowManager;

        //  [Inject]
        private AlarmPopupViewModel _popupVm;

        public AlarmPopupNotifier(IWindowManager windowManager, AlarmPopupViewModel alarmPopupViewModel)
        {
            _windowManager = windowManager;
            _popupVm = alarmPopupViewModel;
        }

        public void OnNext(AlarmMessage alarm)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 更新单例ViewModel的数据
                _popupVm.UpdateAlarm(alarm);
                var s1 = _popupVm.ScreenState;
                // 如果窗口未显示，则显示（Stylet的Show是非模态的）
                if (!_popupVm.IsActive)
                {
                    _windowManager.ShowWindow(_popupVm);
                }
                else
                {
                    // 显式调用恢复窗口
                    _popupVm.RequestBringToFront();
                }
            });
        }

        public void OnCompleted()
        { /* 清理资源 */ }

        public void OnError(Exception e)
        { /* 错误处理 */ }
    }
}