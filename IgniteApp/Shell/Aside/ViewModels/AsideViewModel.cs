using IgniteAdmin.Managers.Main;
using IgniteAdmin.Providers;
using IgniteAdmin.Workers;
using IgniteApp.Bases;
using IgniteApp.Common.Enums;
using IgniteShared.Dtos;
using IgniteShared.Enums;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Paths;
using IT.Tangdao.Framework.Threading;
using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Shell.Aside.ViewModels
{
    public class AsideViewModel : ViewModelBase
    {
        private bool _isInitFinish;

        public bool IsInitFinish
        {
            get => _isInitFinish;
            set => SetAndNotify(ref _isInitFinish, value);
        }

        private RunStatus _currentStatus = RunStatus.Stop;

        public RunStatus CurrentStatus
        {
            get => _currentStatus;
            set => SetAndNotify(ref _currentStatus, value);
        }

        private ProcessStatus _processStatus = ProcessStatus.Manual;

        public ProcessStatus ProcessStatus
        {
            get => _processStatus;
            set => SetAndNotify(ref _processStatus, value);
        }

        private LoginDto _loginDto;

        public LoginDto LoginDto
        {
            get => _loginDto;
            set => SetAndNotify(ref _loginDto, value);
        }

        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(AsideViewModel));
        private DispatcherTimer timer;
        private IAutoRun _autoRun;
        private readonly IContainer _container;
        private readonly WorkstationManager _workstationManager;
        private readonly IEventAggregator _eventAggregator;

        public AsideViewModel(WorkstationManager workstationManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            string projectDirectory = Directory.GetCurrentDirectory();
            _autoRun = ServiceLocator.GetService<IAutoRun>();
            _workstationManager = workstationManager;

            // var path = TangdaoPath.Instance.GetThisFilePath();
            //var sss = path.FileExists;

            //var s1 = imagePath.IsRooted;
            //var s2 = imagePath.FileExists;
        }

        #region--方法--

        public void ExecuteManual()
        {
            ProcessStatus = ProcessStatus.Manual;
            PlcManager.GetPlcSignal<int>(1);
        }

        public async void ExecuteAuto()
        {
            await Task.Delay(200);
            ProcessStatus = ProcessStatus.Auto;
            SysProcessInfo.IsAuto = true;
            SysProcessInfo.IsCannel = false;
        }

        public void ExecuteSemiAuto()
        {
            ProcessStatus = ProcessStatus.SemiAuto;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public async void ExecuteStart()
        {
            CurrentStatus = RunStatus.Running;
            await _workstationManager.StartAllAsync();
        }

        public async Task Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            await Task.Run(() =>
            {
                Console.WriteLine("开始");
                Task.Delay(1000, cts.Token).Wait();
                Console.WriteLine("结束");
            });
        }

        private CancellationTokenSource _runCts;

        /// <summary>
        /// 停止
        /// </summary>
        public async void ExecuteStop()
        {
            //SysProcessInfo.IsCannel = true;
            CurrentStatus = RunStatus.Stop;
            await _workstationManager.StopAllAsync();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void ExecutePause()
        {
            CurrentStatus = RunStatus.Pause;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void ExecuteInit()
        {
            //初始化的时候发送示教位置给PLC，下发配方给PLC，下发系统级参数给PLC
            // SendDataToPlc();
            //  SysProcessInfo.IsInitFinish = IsInitFinish = true;
            CurrentStatus = RunStatus.Init;
            Logger.WriteLocal("初始化完成");
        }

        public static void SendDataToPlc()
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            LoginDto = UIAmbientContext.GetObject<LoginDto>();
        }

        #endregion
    }
}