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
    // 具体工位实现示例
    public class LoadWorkstation : WorkstationBase
    {
        private static long waferId = 0;
        public new string WorkName => "上料工位";
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(LoadWorkstation));

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 生成新产品，确保ID递增
                var id = Interlocked.Increment(ref waferId);
                var wafer = new WaferMessage($"Wafer-{id:D4}");

                Logger.WriteLocal($"生成产品: {wafer.CellId}");
                await Task.Delay(500, token);

                // 传递给预校工位
                await ProductionLine.LoadToPre.Writer.WriteAsync(wafer, token);
                Logger.WriteLocal($"{wafer.CellId} 完成上料 准备 预校");
            }
        }
    }
}