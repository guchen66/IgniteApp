using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.Shell.ProcessParame.Services;
using IgniteShared.Enums;
using IgniteShared.Globals.Common;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Common;
using IT.Tangdao.Framework.Events;
using IT.Tangdao.Framework.Extensions;
using MiniExcelLibs;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class LoadCalibrationViewModel : Screen
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(LoadCalibrationViewModel));
        private ObservableCollection<MotionCalibrationModel> _loadCalibrationDataList;

        public ObservableCollection<MotionCalibrationModel> LoadCalibrationDataList
        {
            get => _loadCalibrationDataList ?? (_loadCalibrationDataList = new ObservableCollection<MotionCalibrationModel>());
            set => SetAndNotify(ref _loadCalibrationDataList, value);
        }

        private int _calibrationValue;

        public int CalibrationValue
        {
            get => _calibrationValue;
            set => SetAndNotify(ref _calibrationValue, value);
        }

        [Inject]
        private SaveFormatViewModel _saveFormatViewModel { get; set; }

        public ITaskController _taskController;
        public ITaskService _taskService;
        private IWindowManager _windowManager;
        private CaliStatus CurrentCaliStatus { get; set; }

        public LoadCalibrationViewModel(ITaskController taskController, ITaskService taskService, IWindowManager windowManager)
        {
            _taskController = taskController;
            _taskService = taskService;
            _windowManager = windowManager;

            // 使用弱事件订阅消息
            //  WeakEventManager<MessageBus, MessageEventArgs>.AddHandler(MessageBus.Instance, "MessageReceived", OnMessageReceived);
        }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async Task StartCalibration()
        {
            try
            {
                //var back = _windowManager.ShowDialogEx(_saveFormatViewModel);
                // 1. 先获取当前状态
                var currentStatus = _taskService.TaskStatus;

                // 2. 根据状态决定行为
                switch (currentStatus)
                {
                    case CaliStatus.Run:
                        // 已经在运行，点击应该暂停
                        _taskService.Pause();
                        return;

                    case CaliStatus.Pause:
                        // 已经暂停，点击应该继续
                        _taskService.Resume();
                        return;

                    case CaliStatus.Init:
                    case CaliStatus.Sucess:
                    case CaliStatus.Failure:
                        // 这3种状态才需要询问"是否继续"
                        Continue(_taskService);
                        break;
                }

                Logger.WriteLocal("开始上料标定");
                var progress = new Progress<CalibrationProgress>(p =>
                {
                    LoadCalibrationDataList.Add(p.NewItem);
                    CalibrationValue = p.NewItem.Id;
                });
                //await _taskController.StartAsync(progress);
                await _taskService.StartAsync(progress);
                Logger.WriteLocal("上料标定完成");
            }
            catch (OperationCanceledException)
            {
                // 处理取消
            }
        }

        private void Continue(ITaskService taskService)
        {
            if (taskService.CaliCount > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                "检测到已有存在的标定记录，是否从上次标定继续执行？",
                "标定继续确认",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _taskService.Resume();
                }
                else if (result == MessageBoxResult.No)
                {
                    LoadCalibrationDataList.Clear();
                    _taskService.CaliCount = 0;
                    _taskService.Stop();
                }
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
            // LoadCalibrationDataList?.Clear();
        }

        public void SaveCalibrationData()
        {
            var result = LocalCalibrationService.CreateExcel(LoadCalibrationDataList);
            if (result)
            {
                HandyControl.Controls.MessageBox.Success("保存成功");
            }
            else
            {
                HandyControl.Controls.MessageBox.Error("保存失败");
            }
        }

        public void SendDataToUnLoad()
        {
            //TangdaoContext.SetLocalValue();
            TangdaoParameter tangdaoParameter = new TangdaoParameter();
            tangdaoParameter.Add("上料数据", LoadCalibrationDataList);
            TangdaoContext.SetTangdaoParameter("上料", tangdaoParameter);
            //  TangdaoContext
        }

        public void WriteDataReport()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            // 规范输出文件路径
            string outputPath = Path.Combine(IgniteInfoLocation.Framework, "001.xlsx");

            // 规范模板文件路径（假设模板放在程序目录下）
            //  string templatePath = Path.Combine(filePath, "temp.xlsx");
            string templatePath = Path.Combine(IgniteInfoLocation.Framework, "tempframework.xlsx");

            var TemplateData = new TemplateData() { Data = LoadCalibrationDataList.ToList() };
            // MiniExcel.SaveAsByTemplate(outputPath, templatePath, new { datas });
            MiniExcel.SaveAsByTemplate(outputPath, templatePath, TemplateData);
        }

        private class TemplateData
        {
            public List<MotionCalibrationModel> Data { get; set; } = new List<MotionCalibrationModel>();
        }
    }
}