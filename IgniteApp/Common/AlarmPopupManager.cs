using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Events;
using IT.Tangdao.Framework.DaoEvents;
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
        public IDaoEventAggregator _daoEventAggregator;

        public AlarmPopupManager(IEventAggregator eventAggregator, IDaoEventAggregator daoEventAggregator)
        {
            _eventAggregator = eventAggregator;
            _daoEventAggregator = daoEventAggregator;
        }

        public void OpenAlarmPopup()
        {
            _eventAggregator.Publish(new OpenAlarmPopupEvent());
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