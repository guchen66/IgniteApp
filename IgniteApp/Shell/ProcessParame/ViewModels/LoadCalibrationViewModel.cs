using IgniteAdmin.Providers;
using IgniteApp.Shell.ProcessParame.Models;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Enums;
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
using IgniteApp.Extensions;
using IgniteApp.Dialogs.ViewModels;
using StyletIoC;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Common;
using IT.Tangdao.Framework;

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

        [Inject]
        private SaveFormatViewModel _saveFormatViewModel { get; set; }

        public ITaskController _taskController;
        public ITaskService _taskService;
        private IWindowManager _windowManager;

        public LoadCalibrationViewModel(ITaskController taskController, ITaskService taskService, IWindowManager windowManager)
        {
            _taskController = taskController;
            _taskService = taskService;
            _windowManager = windowManager;
        }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async Task StartCalibration()
        {
            try
            {
                //var back = _windowManager.ShowDialogEx(_saveFormatViewModel);

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
            // LoadCalibrationDataList?.Clear();
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

    public interface IProgress2<in T>
    {
        void Report(T value);
    }

    public class Progress2<T> : IProgress2<T>
    {
        private readonly SynchronizationContext m_synchronizationContext;

        private readonly Action<T> m_handler;

        private readonly SendOrPostCallback m_invokeHandlers;

        public event EventHandler<T> ProgressChanged;

        public Progress2()
        {
            // m_synchronizationContext = SynchronizationContext.CurrentNoFlow ?? ProgressStatics.DefaultContext;
            //m_invokeHandlers = InvokeHandlers;
        }

        public Progress2(Action<T> handler)
            : this()
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            m_handler = handler;
        }

        protected virtual void OnReport(T value)
        {
            Action<T> handler = m_handler;
            EventHandler<T> progressChanged = this.ProgressChanged;
            if (handler != null || progressChanged != null)
            {
                m_synchronizationContext.Post(m_invokeHandlers, value);
            }
        }

        void IProgress2<T>.Report(T value)
        {
            OnReport(value);
        }

        private void InvokeHandlers(object state)
        {
            T val = (T)state;
            Action<T> handler = m_handler;
            EventHandler<T> progressChanged = this.ProgressChanged;
            handler?.Invoke(val);
            progressChanged?.Invoke(this, val);
        }
    }
}