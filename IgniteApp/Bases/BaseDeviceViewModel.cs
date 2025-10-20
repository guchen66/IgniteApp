using IgniteApp.Shell.Maintion.Services;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Bases
{
    public abstract class BaseDeviceViewModel : Screen, IDeviceObserver
    {
        private bool _boolValue;
        private string _statusText;

        protected BaseDeviceViewModel(string deviceType)
        {
            DeviceType = deviceType;
            //DeviceMediator.Instance.Register(this);
        }

        public string DeviceType { get; }

        public bool BoolValue
        {
            get => _boolValue;
            set
            {
                if (SetAndNotify(ref _boolValue, value))
                {
                    // 当本地状态变化时，通知中介者
                    var state = new DeviceState
                    {
                        IsActive = value,
                        DeviceType = DeviceType,
                        UpdateTime = DateTime.Now
                    };
                    DeviceMediator.Instance.NotifyStateChange(state);
                }
            }
        }

        public string StatusText
        {
            get => _statusText;
            set => SetAndNotify(ref _statusText, value);
        }

        public void UpdateState(DeviceState state)
        {
            // 确保在UI线程上更新
            Execute.OnUIThread(() =>
            {
                BoolValue = state.IsActive;
                StatusText = state.IsActive ? "Success" : "Failed";
            });
        }

        protected override void OnDeactivate()
        {
            DeviceMediator.Instance.Unregister(this);
            base.OnDeactivate();
        }
    }
}