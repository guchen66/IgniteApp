using IgniteAdmin.Interfaces;
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
        public string Name { get; }
        private WorkstationStatus _status;
        private CancellationTokenSource _cts;

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

        protected WorkstationBase(string name)
        {
            Name = name;
            Status = WorkstationStatus.Idle;
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