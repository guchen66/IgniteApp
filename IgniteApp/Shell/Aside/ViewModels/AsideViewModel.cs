using IgniteAdmin.Managers.Main;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using IgniteShared.Extensions;
using IgniteApp.Common.Enums;
using IgniteAdmin.Workers;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Paths;
using IT.Tangdao.Framework.Helpers;
using IgniteApp.Shell.Monitor.ViewModels;
using IgniteShared.Dtos;
using IT.Tangdao.Framework.Common;
using IT.Tangdao.Framework.Threading;

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

        private LoginDto _loginDto;

        public LoginDto LoginDto
        {
            get => _loginDto;
            set => SetAndNotify(ref _loginDto, value);
        }

        private TangdaoPath _tangdaoPath;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(AsideViewModel));
        private DispatcherTimer timer;
        private IAutoRun _autoRun;

        public AsideViewModel()
        {
            //_tangdaoPath = tangdaoPath;
            string projectDirectory = Directory.GetCurrentDirectory();
            _autoRun = ServiceLocator.GetService<IAutoRun>();

            var path = TangdaoPath.Instance.GetThisFilePath();
            var sss = path.FileExists;

            //var s1 = imagePath.IsRooted;
            //var s2 = imagePath.FileExists;
        }

        #region--方法--

        public static void ExecuteManual()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public static void ExecuteAuto()
        {
            SysProcessInfo.IsAuto = true;
            SysProcessInfo.IsCannel = false;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void ExecuteStart()
        {
            // await new WorkstationManager().StartAllAsync();
            // await _autoRun.Run();
            CurrentStatus = RunStatus.Running;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void ExecuteStop()
        {
            //SysProcessInfo.IsCannel = true;
            CurrentStatus = RunStatus.Stop;
            _autoRun.Stop();
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
            LoginDto = AmbientContext.GetCurrent<LoginDto>();
        }

        #endregion
    }
}