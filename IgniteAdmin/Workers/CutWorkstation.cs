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
    /// <summary>
    /// 切割工位
    /// </summary>
    public class CutWorkstation : WorkstationBase
    {
        public new string WorkName => "切割工位";

        [Inject]
        public IEventAggregator eventAggregator;

        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(CutWorkstation));

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var wafer = await ProductionLine.PreToCut.Reader.ReadAsync(token);
                Logger.WriteLocal($"{wafer.CellId} 开始切割");
                eventAggregator.Publish(new ProductUpdateEvent()
                {
                    ProductId = wafer.CellId,
                    ProductName = WorkName,
                    Remark = "进入切割",
                    UpdateTime = DateTime.Now,
                });
                await Task.Delay(3800, token);

                // 传递给切割工位
                await ProductionLine.CutToUnload.Writer.WriteAsync(wafer, token);
                Logger.WriteLocal($"{wafer.CellId} 完成切割 → 下料");
            }
        }
    }
}