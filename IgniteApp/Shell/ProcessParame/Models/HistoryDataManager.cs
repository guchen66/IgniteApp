using IgniteApp.Shell.ProcessParame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public static class HistoryDataManager
    {
        // 数据存储
        public static List<MotionCalibrationModel> MotionCalibrationModels { get; set; }

        // 任务状态（使用WeakReference避免内存泄漏）
        private static WeakReference<LoadCalibrationViewModel> _activeVmRef;

        private static readonly object _syncLock = new object();

        public static void RegisterActiveVM(LoadCalibrationViewModel vm)
        {
            lock (_syncLock)
            {
                _activeVmRef = new WeakReference<LoadCalibrationViewModel>(vm);
            }
        }
    }

    // 全局任务管理器（单例）
    public class CalibrationService
    {
        private static readonly Lazy<CalibrationService> _instance = new Lazy<CalibrationService>(() => new CalibrationService());

        public static CalibrationService Instance => _instance.Value;
        public CancellationTokenSource Cts { get; private set; }
        public TaskCompletionSource<bool> PauseTcs { get; private set; }
        public bool IsPaused { get; set; }
        public ObservableCollection<MotionCalibrationModel> Data { get; } = new ObservableCollection<MotionCalibrationModel>();

        public void StartNewCalibration()
        {
            Cts = new CancellationTokenSource();
            PauseTcs = new TaskCompletionSource<bool>();
            IsPaused = false;
        }
    }
}