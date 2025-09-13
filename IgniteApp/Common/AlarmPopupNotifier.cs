using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Dialogs.Views;
using IgniteDevices.PLC;
using IgniteShared.Extensions;
using IT.Tangdao.Framework.Abstractions;
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
        private AlarmPopupViewModel _alarmPopupViewModel;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(AlarmPopupNotifier));

        public AlarmPopupNotifier(IWindowManager windowManager, AlarmPopupViewModel alarmPopupViewModel)
        {
            _windowManager = windowManager;
            _alarmPopupViewModel = alarmPopupViewModel;
        }

        public void OnNext(AlarmMessage alarm)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 更新单例ViewModel的数据
                Logger.WriteLocal("更新单例ViewModel的数据");
                _alarmPopupViewModel.UpdateAlarm(alarm);

                Logger.WriteLocal($"打开前AlarmPopupViewModel状态：{_alarmPopupViewModel.ScreenState}");
                // 如果窗口未显示，则显示（Stylet的Show是非模态的）
                if (!_alarmPopupViewModel.IsActive)
                {
                    _windowManager.ShowWindow(_alarmPopupViewModel);
                    Logger.WriteLocal($"打开后AlarmPopupViewModel状态：{_alarmPopupViewModel.ScreenState}");
                }
                else
                {
                    // 显式调用恢复窗口
                    _alarmPopupViewModel.RequestBringToFront();
                }
            });
        }

        public void OnCompleted()
        { /* 清理资源 */ }

        public void OnError(Exception e)
        { /* 错误处理 */ }
    }
}