using IgniteAdmin.Managers.Main;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.DaoAdmin;
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

        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(AsideViewModel));
        private DispatcherTimer timer;
        private IAutoRun _autoRun;

        public AsideViewModel()
        {
            _autoRun = ServiceLocator.GetService<IAutoRun>();
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
        public async void ExecuteStart()
        {
            await new WorkstationManager().StartAllAsync();
            await _autoRun.Run();
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
            SendDataToPlc();
            SysProcessInfo.IsInitFinish = IsInitFinish = true;
            CurrentStatus = RunStatus.Init;
            Logger.WriteLocal("初始化完成");
        }

        public static void SendDataToPlc()
        {
        }

        #endregion
    }
}