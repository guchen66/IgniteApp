using IgniteShared.Events;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteAdmin.Workers
{
    public class SegmentWorkstation //: WorkstationBase
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(SegmentWorkstation));

        [Inject]
        public IEventAggregator eventAggregator;

        public new string WorkName => "裂片工位";

        protected async Task ExecuteWorkAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 上料工位具体逻辑
                await Task.Delay(1000, token); // 模拟工作
                Logger.WriteLocal("完成裂片");
                eventAggregator.Publish(new ProductUpdateEvent()
                {
                    //ProductId = wafer.CellId,
                    ProductName = WorkName,
                    Remark = "进入切割",
                    UpdateTime = DateTime.Now,
                });
                // 更新状态、通知UI等
            }
        }
    }
}