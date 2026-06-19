using IT.Tangdao.Framework.Abstractions.Messaging;
using IT.Tangdao.Framework.Events;
using Stylet;
using System;

namespace IgniteApp.Bases
{
    public abstract class BaseDeviceViewModel : Screen, IMessageObserver
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

        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsReceive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler<MessageEventArgs> MessageIntercepted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void UpdateMessage(MessageContext context)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }
    }
}