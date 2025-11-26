using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class LightViewModel : BaseDeviceViewModel, INavigateEntry
    {
        private double _progress;

        public double Progress
        {
            get => _progress;
            set => SetAndNotify(ref _progress, value);
        }

        private double _mark;

        public double Mark
        {
            get => _mark;
            set => SetAndNotify(ref _mark, value);
        }

        private bool _content;

        public bool Content
        {
            get => _content;
            set => SetAndNotify(ref _content, value);
        }

        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(LightViewModel));

        public LightViewModel() : base("Light")
        {
        }

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationTokenSource _progressCts;
        private bool _isRunning;

        public async Task ExecuteGetProcess()
        {
            // 如果正在运行，先取消之前的任务
            if (_isRunning)
            {
                _progressCts?.Cancel();
                await Task.Delay(50); // 给一点时间让上一个任务结束
            }

            // 创建新的取消令牌
            _progressCts = new CancellationTokenSource();
            _isRunning = true;
            Logger.WriteLocal("开始运行");
            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    // 检查是否被取消
                    _progressCts.Token.ThrowIfCancellationRequested();
                    Logger.WriteLocal($"打印结果：{i}");
                    await Task.Delay(100, _progressCts.Token);
                    Progress = i;
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消，不需要处理
            }
            finally
            {
                _isRunning = false;
                Progress = 0; // 重置进度
                Logger.WriteLocal($"重置进度：{Progress}");
            }
        }

        public void Execute()
        {
            //Content = !Content;
            //int index = 1;
            //Task.Run(() =>
            //{
            //    var id = Interlocked.Increment(ref index);
            //});
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //});

            Mark = 100;
        }
    }
}