using IgniteShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Common
{
    public class IgniteEventHandler
    {
        public static event EventHandler<StatisticUpdateEventArgs> StatisticUpdated;

        public static void RaiseStatisticUpdate(object sender, StaticticDto staticticDto)
        {
            var handler = StatisticUpdated;
            if (handler != null)
            {
                // 确保在主线程触发（WPF需要）
                Application.Current.Dispatcher.Invoke(() =>
                {
                    handler(sender, new StatisticUpdateEventArgs(staticticDto));
                });
            }
        }
    }

    public class StatisticUpdateEventArgs : EventArgs
    {
        public StaticticDto StatisticDto { get; set; }

        public StatisticUpdateEventArgs(StaticticDto statisticDto)
        {
            StatisticDto = statisticDto;
        }
    }
}