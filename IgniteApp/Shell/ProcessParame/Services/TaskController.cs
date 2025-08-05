using IgniteApp.Shell.ProcessParame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Services
{
    public class TaskController : ITaskController, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private CancellationTokenSource _cts;
        private volatile bool _isPaused;
        private object lockObject = new object();

        public async Task StartAsync(IProgress<CalibrationProgress> progress)
        {
            lock (lockObject)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                // 确保信号量可用
                if (_semaphore.CurrentCount == 0)
                    _semaphore.Release();

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
                        await _semaphore.WaitAsync(_cts.Token); // 会阻塞在这里直到Resume()
                        _semaphore.Release();
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
                _semaphore.Wait(); // 占用信号量
            }
        }

        public void Resume()
        {
            lock (lockObject)
            {
                _isPaused = false;
                _semaphore.Release(); // 释放信号量
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
            _semaphore?.Dispose();
        }
    }
}