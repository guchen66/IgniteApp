using IgniteApp.Shell.ProcessParame.Models;
using IgniteShared.Enums;
using System;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Services
{
    public interface ITaskService
    {
        CaliStatus TaskStatus { get; set; }
        int CaliCount { get; set; }

        Task StartAsync(IProgress<CalibrationProgress> progress);

        void Pause();

        void Resume();

        void Stop();
    }
}