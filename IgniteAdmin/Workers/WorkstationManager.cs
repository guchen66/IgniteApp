using IgniteAdmin.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using IgniteShared.Globals.Common.Works;

namespace IgniteAdmin.Workers
{
    // 工位管理器
    public class WorkstationManager //: INotifyPropertyChanged
    {
        private readonly List<IWorkstation> _workstations;
        private CancellationTokenSource _globalCts;

        public IReadOnlyCollection<IWorkstation> Workstations => _workstations.AsReadOnly();

        public WorkstationManager()
        {
            _workstations = new List<IWorkstation>
            {
                new LoadWorkstation(),
                new PreWorkstation(),
                new CutWorkstation(),
                new UnLoadWorkstation(),
                // 其他工位...
            };
        }

        public async Task StartAllAsync()
        {
            await StopAllAsync();
            _globalCts = new CancellationTokenSource();
            var startTasks = _workstations.Select(ws => ws.StartAsync(_globalCts.Token));
            await Task.WhenAll(startTasks);
        }

        public async Task StopAllAsync()
        {
            _globalCts?.Cancel();
            var stopTasks = _workstations.Select(ws => ws.StopAsync());
            await Task.WhenAll(stopTasks);
        }
    }

    public static class Conveyor
    {
        // 顺序队列，支持 async/await
        private static readonly ConcurrentQueue<WaferMessage> _queue = new ConcurrentQueue<WaferMessage>();

        private static readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public static void Post(WaferMessage msg)
        {
            _queue.Enqueue(msg);
            _signal.Release();
        }

        public static async Task<WaferMessage> TakeAsync(CancellationToken token)
        {
            await _signal.WaitAsync(token);
            _queue.TryDequeue(out var msg);
            return msg;
        }
    }
}