using IgniteShared.Events;
using IgniteShared.Globals.Common.Works;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Workers
{
    public class UnLoadWorkstation : WorkstationBase
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(UnLoadWorkstation));
        public new string WorkName => "下料工位";

        [Inject]
        public IEventAggregator eventAggregator;

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var wafer = await ProductionLine.CutToUnload.Reader.ReadAsync(token);
                Logger.WriteLocal($"[下料] 开始: {wafer.CellId}");
                eventAggregator.Publish(new ProductUpdateEvent()
                {
                    ProductId = wafer.CellId,
                    ProductName = WorkName,
                    Remark = "进入下料",
                    UpdateTime = DateTime.Now,
                });
                await Task.Delay(1800, token); // 下料时间

                var totalTime = DateTime.Now - wafer.EnterTime;
                Logger.WriteLocal($"[下料] {wafer.CellId} ✓ 完成! 总耗时: {totalTime.TotalSeconds:F1}秒");
            }
        }
    }
}