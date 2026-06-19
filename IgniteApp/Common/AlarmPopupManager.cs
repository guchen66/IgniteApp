using IgniteApp.Events;
using IgniteDevices.PLC;
using TangdaoEvents = IT.Tangdao.Framework.Events;
using Stylet;
using System;

namespace IgniteApp.Common
{
    /// <summary>
    /// 使用事件聚合器打开报警操作类
    /// </summary>
    public class AlarmPopupManager
    {
        public IEventAggregator _eventAggregator;
        public TangdaoEvents.IEventAggregator _daoEventAggregator;

        public AlarmPopupManager(IEventAggregator eventAggregator, TangdaoEvents.IEventAggregator daoEventAggregator)
        {
            _eventAggregator = eventAggregator;
            _daoEventAggregator = daoEventAggregator;
        }

        public void OpenAlarmPopup()
        {
            _eventAggregator.Publish(new OpenAlarmPopupEvent());
        }

        public void OpenAlarmPopup(AlarmMessage alarmMessage)
        {
            _eventAggregator.Publish(new OpenAlarmPopupEvent() { AlarmMessage = alarmMessage });
        }

        public void CloseAlarmPopup()
        {
            // _eventAggregator.Publish(new CloseAlarmPopupEvent());
            _daoEventAggregator.PublishAsync(new CloseAlarmPopupEvent()
            {
                CurrentTime = DateTime.Now,
            });
        }
    }
}