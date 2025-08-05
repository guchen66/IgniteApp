using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Events;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
    /// <summary>
    /// 使用事件聚合器打开报警操作类
    /// </summary>
    public class AlarmPopupManager
    {
        public IEventAggregator _eventAggregator;

        public AlarmPopupManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void OpenAlarmPopup()
        {
            _eventAggregator.Publish(new OpenAlarmPopupEvent());
        }

        public void CloseAlarmPopup()
        {
            _eventAggregator.Publish(new CloseAlarmPopupEvent());
        }
    }
}