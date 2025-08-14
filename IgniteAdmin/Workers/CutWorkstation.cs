using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Workers
{
    /// <summary>
    /// 切割工位
    /// </summary>
    public class CutWorkstation : WorkstationBase
    {
        public CutWorkstation() : base("切割工位")
        {
        }

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 上料工位具体逻辑
                await Task.Delay(1000, token); // 模拟工作

                // 更新状态、通知UI等
            }
        }
    }
}