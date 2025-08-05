using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.Shell.ProcessParame.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
    public sealed class AsyncTaskManager
    {
        private readonly ConcurrentDictionary<string, ITaskController> _tasks = new ConcurrentDictionary<string, ITaskController>();
        private readonly object _syncLock = new object();

        public ITaskController RegisterTask(string taskId, Func<CancellationToken, Task> taskFunc)
        {
            lock (_syncLock)
            {
                if (_tasks.ContainsKey(taskId))
                    throw new InvalidOperationException($"Task {taskId} already exists");

                var controller = new TaskController();
                _tasks[taskId] = controller;

                // 启动任务但不阻塞

                return controller;
            }
        }

        public ITaskController GetTask(string taskId)
        {
            lock (_syncLock)
                return _tasks.TryGetValue(taskId, out var controller) ? controller : null;
        }
    }
}