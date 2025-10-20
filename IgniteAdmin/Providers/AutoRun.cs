using IgniteShared.Extensions;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteAdmin.Providers
{
    public class AutoRun : IAutoRun
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(AutoRun));
        private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        private CancellationTokenSource cts = new CancellationTokenSource();

        public async Task Run()
        {
            if (!SysProcessInfo.IsInitFinish)
            {
                MessageBox.Show("初始化未完成");
                return;
            }
            if (!SysProcessInfo.IsAuto)
            {
                MessageBox.Show("请切换到自动模式");
                return;
            }

            CancellationToken token = cts.Token;

            Task loadTask = Task.Run(async () => await LoadStart(tcs), token);
            Task cutTask = Task.Run(async () => await CutStart(tcs), token);
            Task unitTask = Task.Run(async () => await UnitStart(tcs), token);
            Task unloadTask = Task.Run(async () => await UnLoadStart(tcs), token);
            //  等待所有任务完成
            await Task.WhenAll(loadTask, cutTask, unitTask, unloadTask);

            // 检查是否有任务被取消
            /*   if (loadTask.IsCanceled || cutTask.IsCanceled || unitTask.IsCanceled || unloadTask.IsCanceled)
               {
                   tcs.TrySetResult(false);
               }
               else
               {
                   tcs.TrySetResult(true);
               }*/
            //await loadTask;
            token.ThrowIfCancellationRequested();
        }

        private async Task LoadStart(TaskCompletionSource<bool> tcs)
        {
            try
            {
                int count = 0;
                // 如果收到PLC开始信号，开始执行上料
                while (!cts.IsCancellationRequested)
                {
                    count++;
                    Logger.WriteLocal($"开始上料{count}");
                    // 模拟上料操作
                    await Task.Delay(1000);
                }
            }
            catch (OperationCanceledException)
            {
                // 任务被取消
            }
        }

        private async Task CutStart(TaskCompletionSource<bool> tcs)
        {
            try
            {
                // 模拟切割操作
                int count = 0;
                while (!cts.IsCancellationRequested)
                {
                    count++;
                    Logger.WriteLocal($"开始切割{count}");
                    await Task.Delay(1000);
                }
            }
            catch (OperationCanceledException)
            {
                // 任务被取消
            }
        }

        private async Task UnitStart(TaskCompletionSource<bool> tcs)
        {
            try
            {
                // 模拟单元操作
                int count = 0;
                while (!cts.IsCancellationRequested)
                {
                    count++;
                    Logger.WriteLocal($"开始单元{count}");
                    await Task.Delay(1000);
                }
            }
            catch (OperationCanceledException)
            {
                // 任务被取消
            }
        }

        private async Task UnLoadStart(TaskCompletionSource<bool> tcs)
        {
            try
            {
                // 模拟卸料操作
                int count = 0;
                while (!cts.IsCancellationRequested)
                {
                    count++;
                    Logger.WriteLocal($"开始下料{count}");
                    await Task.Delay(1000);
                }
            }
            catch (OperationCanceledException)
            {
                // 任务被取消
            }
        }

        public Task<bool> GetResultAsync()
        {
            return tcs.Task;
        }

        public async Task Stop()
        {
            if (cts != null)
            {
                await Task.Delay(1000);
                cts.Cancel();
            }
        }
    }

    /*
        public class AutoRun : IAutoRun
        {
            private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(AutoRun));
            private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            private CancellationTokenSource cts = new CancellationTokenSource();

            public async Task Run()
            {
                if (!SysProcessInfo.IsInitFinish)
                {
                    MessageBox.Show("初始化未完成");
                    return;
                }
                if (!SysProcessInfo.IsAuto)
                {
                    MessageBox.Show("请切换到自动模式");
                    return;
                }

                CancellationToken token = cts.Token;

                Task loadTask = Task.Run(async () => await LoadStart(token), token);
                Task cutTask = Task.Run(async () => await CutStart(token), token);
                Task unitTask = Task.Run(async () => await UnitStart(token), token);
                Task unloadTask = Task.Run(async () => await UnLoadStart(token), token);

                // 等待所有任务完成
                await Task.WhenAll(loadTask, cutTask, unitTask, unloadTask);

                // 检查是否有任务被取消
                if (loadTask.IsCanceled || cutTask.IsCanceled || unitTask.IsCanceled || unloadTask.IsCanceled)
                {
                    tcs.TrySetResult(false);
                }
                else
                {
                    tcs.TrySetResult(true);
                }

                // 取消所有任务
                token.Register(async () =>
                {
                    if (SysProcessInfo.IsCannel)
                    {
                        await tcs.Task;
                        cts.Cancel();
                        // GetResultAsync();
                    }
                });
            }

            private async Task LoadStart(CancellationToken token)
            {
                try
                {
                    int count = 0;
                    // 如果收到PLC开始信号，开始执行上料
                    while (!token.IsCancellationRequested)
                    {
                        count++;
                        Logger.WriteLocal($"开始上料{count}");
                        // 模拟上料操作
                        await Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消
                }
            }

            private async Task CutStart(CancellationToken token)
            {
                try
                {
                    // 模拟切割操作
                    int count = 0;
                    while (!token.IsCancellationRequested)
                    {
                        count++;
                        Logger.WriteLocal($"开始切割{count}");
                        await Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消
                }
            }

            private async Task UnitStart(CancellationToken token)
            {
                try
                {
                    // 模拟单元操作
                    int count = 0;
                    while (!token.IsCancellationRequested)
                    {
                        count++;
                        Logger.WriteLocal($"开始单元{count}");
                        await Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消
                }
            }

            private async Task UnLoadStart(CancellationToken token)
            {
                try
                {
                    // 模拟卸料操作
                    int count = 0;
                    while (!token.IsCancellationRequested)
                    {
                        count++;
                        Logger.WriteLocal($"开始下料{count}");
                        await Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消
                }
            }

            public Task<bool> GetResultAsync()
            {
                return tcs.Task;
            }
        }*/
}