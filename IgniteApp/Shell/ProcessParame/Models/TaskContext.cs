using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public class TaskContext : IDisposable
    {
        private readonly Dictionary<string, ManualResetEventSlim> _taskEvents = new Dictionary<string, ManualResetEventSlim>();
        private readonly Dictionary<string, TaskState> _taskStates = new Dictionary<string, TaskState>();
        private readonly object _syncLock = new object();

        public void RegisterTask(string taskId)
        {
            lock (_syncLock)
            {
                _taskEvents[taskId] = new ManualResetEventSlim(true); // 初始为可运行
                _taskStates[taskId] = TaskState.Init;
            }
        }

        public void UnregisterTask(string taskId)
        {
            lock (_syncLock)
            {
                if (_taskEvents.TryGetValue(taskId, out var mre))
                {
                    mre.Dispose();
                    _taskEvents.Remove(taskId);
                    _taskStates.Remove(taskId);
                }
            }
        }

        public void Pause(string taskId)
        {
            lock (_syncLock)
            {
                if (_taskEvents.TryGetValue(taskId, out var mre))
                {
                    mre.Reset();
                    _taskStates[taskId] = TaskState.Pause;
                }
            }
        }

        public void Resume(string taskId)
        {
            lock (_syncLock)
            {
                if (_taskEvents.TryGetValue(taskId, out var mre))
                {
                    mre.Set();
                    _taskStates[taskId] = TaskState.Running;
                }
            }
        }

        public void Stop(string taskId)
        {
            lock (_syncLock)
            {
                if (_taskEvents.TryGetValue(taskId, out var mre))
                {
                    _taskStates[taskId] = TaskState.Stop;
                    mre.Set(); // 确保任务退出等待
                }
            }
        }

        public TaskState GetState(string taskId)
        {
            lock (_syncLock)
            {
                return _taskStates.TryGetValue(taskId, out var state) ? state : TaskState.Init;
            }
        }

        public ManualResetEventSlim GetPauseEvent(string taskId)
        {
            lock (_syncLock)
            {
                return _taskEvents.TryGetValue(taskId, out var mre) ? mre : null;
            }
        }

        public void Dispose()
        {
            lock (_syncLock)
            {
                foreach (var mre in _taskEvents.Values)
                    mre.Dispose();
                _taskEvents.Clear();
                _taskStates.Clear();
            }
        }
    }

    public static class ManualResetEventSlimExtensions
    {
        public static async Task WaitAsync(this ManualResetEventSlim mre, CancellationToken ct)
        {
            while (!mre.IsSet)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(50, ct).ConfigureAwait(false);
            }
        }
    }
}