using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Workers
{
    public class SegmentWorkstation : WorkstationBase
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(SegmentWorkstation));

        //public SegmentWorkstation() : base("裂片工位")
        //{
        //}

        public new string WorkName => "裂片工位";

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 上料工位具体逻辑
                await Task.Delay(1000, token); // 模拟工作
                Logger.WriteLocal("完成裂片");
                // 更新状态、通知UI等
            }
        }
    }
}