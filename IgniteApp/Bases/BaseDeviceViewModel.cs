using IT.Tangdao.Framework.Abstractions.Notices;
using Stylet;
using System;

namespace IgniteApp.Bases
{
    public abstract class BaseDeviceViewModel : Screen, INoticeObserver
    {
        private bool _boolValue;
        private string _statusText;

        protected BaseDeviceViewModel(string deviceType)
        {
            DeviceType = deviceType;
        }

        public string DeviceType { get; }

        public bool BoolValue
        {
            get => _boolValue;
            set
            {
                SetAndNotify(ref _boolValue, value);
            }
        }

        public string StatusText
        {
            get => _statusText;
            set => SetAndNotify(ref _statusText, value);
        }

        public void UpdateNotice(NoticeContext context)
        {
            BoolValue = context.CurrentState;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }
    }
}