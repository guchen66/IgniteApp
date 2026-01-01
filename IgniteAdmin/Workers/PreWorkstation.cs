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
    /// 预校工位
    /// </summary>
    public class PreWorkstation : WorkstationBase
    {
        public new string WorkName => "预校工位";
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(PreWorkstation));

        [Inject]
        public IEventAggregator eventAggregator;

        protected override async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 从上料工位获取产品
                var wafer = await ProductionLine.LoadToPre.Reader.ReadAsync(token);
                Logger.WriteLocal($"{wafer.CellId} 开始预校");
                eventAggregator.Publish(new ProductUpdateEvent()
                {
                    ProductId = wafer.CellId,
                    ProductName = WorkName,
                    Remark = "进入预校",
                    UpdateTime = DateTime.Now,
                });
                await Task.Delay(1800, token);

                // 传递给切割工位
                await ProductionLine.PreToCut.Writer.WriteAsync(wafer, token);
                Logger.WriteLocal($"{wafer.CellId} 完成预校 准备 切割");
            }
        }
    }
}