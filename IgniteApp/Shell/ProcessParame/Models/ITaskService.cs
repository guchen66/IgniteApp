using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public interface ITaskService
    {
        Task StartAsync(IProgress<CalibrationProgress> progress);

        void Pause();

        void Resume();

        void Stop();
    }

    public class TaskService : ITaskService, IDisposable
    {
        private readonly ManualResetEventSlim _manual = new ManualResetEventSlim();
        private CancellationTokenSource _cts;
        private volatile bool _isPaused;
        private object lockObject = new object();

        public async Task StartAsync(IProgress<CalibrationProgress> progress)
        {
            lock (lockObject)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                _manual.Set();

                _isPaused = false;
            }

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    // 检查暂停状态

                    _cts.Token.ThrowIfCancellationRequested();

                    progress.Report(new CalibrationProgress()
                    {
                        NewItem = new MotionCalibrationModel()
                        {
                            Id = i + 1,
                            StartValue = i,
                            EndValue = i + 100
                        }
                    });

                    await Task.Delay(1000, _cts.Token);
                    if (_isPaused)
                    {
                        await Task.Run(() => { _manual.Wait(_cts.Token); });
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("任务已取消");
            }
        }

        public void Pause()
        {
            lock (lockObject)
            {
                _isPaused = true;
                _manual.Reset();
            }
        }

        public void Resume()
        {
            lock (lockObject)
            {
                _isPaused = false;
                _manual.Set();
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                _cts?.Cancel();
            }
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _manual?.Dispose();
        }
    }
}