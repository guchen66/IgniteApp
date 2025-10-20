using IgniteApp.Bases;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Shell.ProcessParame.Models;
using IT.Tangdao.Framework.Common;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class UnLoadCalibrationViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        private ObservableCollection<MotionCalibrationModel> _unLoadCalibrationDataList;

        public ObservableCollection<MotionCalibrationModel> UnLoadCalibrationDataList
        {
            get => _unLoadCalibrationDataList ?? (_unLoadCalibrationDataList = new ObservableCollection<MotionCalibrationModel>());
            set => SetAndNotify(ref _unLoadCalibrationDataList, value);
        }

        public IWindowManager _windowManager;

        public UnLoadCalibrationViewModel(IWindowManager windowManager)
        {
            // var status = this.ScreenState;
            // this.ScreenState = ScreenState.Active;
            _windowManager = windowManager;
        }

        [Inject]
        public SetCalibrationParameterViewModel setCalibrationParameterViewModel { get; set; }

        /// <summary>
        /// 模拟开始标定
        /// </summary>
        /// <returns></returns>
        public async Task StartCalibration()
        {
            _cts = new CancellationTokenSource();
            _pauseTcs = new TaskCompletionSource<bool>();
            _isPaused = false;

            try
            {
                var Count = 100; //定义100条数据，100秒内完成
                List<string> caliTypes = Enumerable.Repeat("运动系标定", Count).ToList();

                for (int i = 0; i < caliTypes.Count; i++)
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    if (_isPaused)
                    {
                        await _pauseTcs.Task; // 阻塞直到恢复
                        _pauseTcs = new TaskCompletionSource<bool>(); // 重置
                    }
                    UnLoadCalibrationDataList.Add(new MotionCalibrationModel()
                    {
                        Id = i + 1,
                        StartValue = i,
                        EndValue = i + 100,
                    });
                    var updates = UnLoadCalibrationDataList.Zip(caliTypes, (left, right) =>
                    {
                        left.CaliType = right;
                        return left;
                    }).ToList();
                    UnLoadCalibrationDataList = updates.ToObservableCollection();

                    // 模拟工作（带进度更新）
                    await Task.Delay(1000, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("标定已取消");
            }
        }

        private CancellationTokenSource _cts;
        private TaskCompletionSource<bool> _pauseTcs;
        private bool _isPaused;

        public void StopCalibration()
        {
            _pauseTcs?.TrySetCanceled(); // 强制释放暂停
            _cts?.Cancel();
            UnLoadCalibrationDataList?.Clear();
        }

        /// <summary>
        /// 如果快速连续点击 暂停/恢复，
        /// 可能导致：_pauseTcs被多次设置
        /// 状态竞争（_isPaused与_pauseTcs不一致）
        /// </summary>
        private readonly object _syncLock = new object();

        public void PauseCalibration()
        {
            lock (_syncLock)
            {
                _isPaused = true;
            }
        }

        public void ResumeCalibration()
        {
            lock (_syncLock)
            {
                _isPaused = false;
                _pauseTcs?.TrySetResult(true);
                //每次暂停都应使用全新的TaskCompletionSource
                //避免不同暂停周期之间的状态污染，两种保护机制不一样，防止死锁
                _pauseTcs = new TaskCompletionSource<bool>();
            }
        }

        public void UpdateData()
        {
        }

        public void Open()
        {
            setCalibrationParameterViewModel.Parent = this;
            //  setCalibrationParameterViewModel.OnSaved += name => this.Name = name;
            var result = _windowManager.ShowDialog(setCalibrationParameterViewModel);
            if (result.GetValueOrDefault())
            {
                // setCalibrationParameterViewModel.ScreenState=
                // setCalibrationParameterViewModel.OnSaved += name => this.Name = name;
            }
        }

        public void ReviceDataFromLoad()
        {
            var parameter = TangdaoContext.GetTangdaoParameter("上料");
            UnLoadCalibrationDataList = parameter.Get<ObservableCollection<MotionCalibrationModel>>("上料数据");
        }

        protected override void OnDeactivate()
        {
        }
    }
}