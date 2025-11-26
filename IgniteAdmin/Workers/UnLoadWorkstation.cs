using IgniteShared.Globals.Common.Works;
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
    public class UnLoadWorkstation : WorkstationBase
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(UnLoadWorkstation));
        public new string WorkName => "下料工位";

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var wafer = await ProductionLine.CutToUnload.Reader.ReadAsync(token);
                Logger.WriteLocal($"[下料] 开始: {wafer.CellId}");

                await Task.Delay(800, token); // 下料时间

                var totalTime = DateTime.Now - wafer.EnterTime;
                Logger.WriteLocal($"[下料] {wafer.CellId} ✓ 完成! 总耗时: {totalTime.TotalSeconds:F1}秒");
            }
        }
    }
}