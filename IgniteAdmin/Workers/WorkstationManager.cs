using IgniteAdmin.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                new CutWorkstation(),
                new SegmentWorkstation(),
                new UnLoadWorkstation(),
                // 其他工位...
            };
        }

        public async Task StartAllAsync()
        {
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
}