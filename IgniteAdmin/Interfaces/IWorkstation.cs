using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Interfaces
{
    // 工位基础接口
    public interface IWorkstation
    {
        string Name { get; }

        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync();

        WorkstationStatus Status { get; }
    }

    public enum WorkstationStatus
    { Idle, Running, Paused, Faulted }
}