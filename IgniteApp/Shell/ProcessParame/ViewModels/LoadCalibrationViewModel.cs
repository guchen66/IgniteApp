using IgniteAdmin.Providers;
using IgniteApp.Shell.ProcessParame.Models;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoEnums;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IgniteShared.Extensions;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class LoadCalibrationViewModel : Screen
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(LoadCalibrationViewModel));
        private ObservableCollection<MotionCalibrationModel> _unLoadCalibrationDataList;

        public ObservableCollection<MotionCalibrationModel> UnLoadCalibrationDataList
        {
            get => _unLoadCalibrationDataList ?? (_unLoadCalibrationDataList = new ObservableCollection<MotionCalibrationModel>());
            set => SetAndNotify(ref _unLoadCalibrationDataList, value);
        }

        public ITaskController _taskController;
        public ITaskService _taskService;

        public LoadCalibrationViewModel(ITaskController taskController, ITaskService taskService)
        {
            _taskController = taskController;
            _taskService = taskService;
        }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async Task StartCalibration()
        {
            try
            {
                Logger.WriteLocal("开始标定");
                var progress = new Progress<CalibrationProgress>(p =>
                {
                    UnLoadCalibrationDataList.Add(p.NewItem);
                });
                //await _taskController.StartAsync(progress);
                await _taskService.StartAsync(progress);
            }
            catch (OperationCanceledException)
            {
                // 处理取消
            }
        }

        public void PauseCalibration()
        {
            //_taskController.Pause();
            _taskService.Pause();
        }

        public void ResumeCalibration()
        {
            //_taskController.Resume();
            _taskService.Resume();
        }

        public void StopCalibration()
        {
            // _taskController.Stop();
            _taskService.Stop();
            //  _cts.Cancel();
            // _taskController.Stop();
            UnLoadCalibrationDataList?.Clear();
        }
    }
}