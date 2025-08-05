using IgniteApp.Shell.ProcessParame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Services
{
    public interface ITaskService
    {
        Task StartAsync(IProgress<CalibrationProgress> progress);

        void Pause();

        void Resume();

        void Stop();
    }
}