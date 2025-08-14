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
using IgniteApp.Shell.ProcessParame.Services;
using MiniExcelLibs;
using System.IO;
using IgniteShared.Globals.Local;
using HandyControl.Controls;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class LoadCalibrationViewModel : Screen
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(LoadCalibrationViewModel));
        private ObservableCollection<MotionCalibrationModel> _loadCalibrationDataList;

        public ObservableCollection<MotionCalibrationModel> LoadCalibrationDataList
        {
            get => _loadCalibrationDataList ?? (_loadCalibrationDataList = new ObservableCollection<MotionCalibrationModel>());
            set => SetAndNotify(ref _loadCalibrationDataList, value);
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
                Logger.WriteLocal("开始上料标定");
                var progress = new Progress<CalibrationProgress>(p =>
                {
                    LoadCalibrationDataList.Add(p.NewItem);
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
            LoadCalibrationDataList?.Clear();
        }

        public void SaveCalibrationData()
        {
            var result = LocalCalibrationService.CreateExcel(LoadCalibrationDataList);
            if (result)
            {
                MessageBox.Success("保存成功");
            }
            else
            {
                MessageBox.Error("保存失败");
            }
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