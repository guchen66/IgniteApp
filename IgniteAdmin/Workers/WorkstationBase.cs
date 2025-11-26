using IgniteAdmin.Interfaces;
using IgniteShared.Globals.Common.Works;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Workers
{
    // 工位基类
    public abstract class WorkstationBase : IWorkstation, INotifyPropertyChanged
    {
        public virtual string WorkName { get; }
        private WorkstationStatus _status;
        private CancellationTokenSource _cts;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(WorkstationBase));

        public WorkstationStatus Status
        {
            get => _status;
            protected set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task StartAsync1(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var wafer = await Conveyor.TakeAsync(token);
                await ProcessAsync(wafer, token);
                Conveyor.Post(wafer);               // 传给下游
            }
        }

        private async Task ProcessAsync(WaferMessage wafer, CancellationToken token)
        {
            await Task.Delay(500, token);   // 模拟节拍
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            await ExecuteWorkAsync(_cts.Token);
            Logger.WriteLocal($"{WorkName} 处理完成 {wafer.CellId}");
        }

        public async Task StartAsync(CancellationToken parentToken)
        {
            if (Status == WorkstationStatus.Running)
                return;

            Status = WorkstationStatus.Running;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(parentToken);

            try
            {
                await ExecuteWorkAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                Status = WorkstationStatus.Idle;
            }
            catch (Exception ex)
            {
                Status = WorkstationStatus.Faulted;
                // 记录错误
            }
        }

        public async Task StopAsync()
        {
            _cts?.Cancel();
            await CleanupAsync();
            Status = WorkstationStatus.Idle;
        }

        protected abstract Task ExecuteWorkAsync(CancellationToken token);

        protected virtual Task CleanupAsync() => Task.CompletedTask;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}