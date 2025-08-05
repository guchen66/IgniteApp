using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Services
{
    public class DeviceMediator
    {
        private static readonly Lazy<DeviceMediator> _instance = new Lazy<DeviceMediator>(() => new DeviceMediator());

        public static DeviceMediator Instance => _instance.Value;

        private readonly List<IDeviceObserver> _observers = new List<IDeviceObserver>();
        private readonly object _lock = new object();

        private DeviceMediator()
        { }

        public void Register(IDeviceObserver observer)
        {
            lock (_lock)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }
            }
        }

        public void Unregister(IDeviceObserver observer)
        {
            lock (_lock)
            {
                _observers.Remove(observer);
            }
        }

        public void NotifyStateChange(DeviceState state)
        {
            List<IDeviceObserver> observersCopy;
            lock (_lock)
            {
                observersCopy = new List<IDeviceObserver>(_observers);
            }

            foreach (var observer in observersCopy)
            {
                // 只通知关心此设备类型的观察者
                if (observer.DeviceType == state.DeviceType)
                {
                    observer.UpdateState(state);
                }
            }
        }

        /// <summary>
        /// 一键通知所有设备
        /// </summary>
        /// <param name="newState">新的状态（true=连接，false=断开）</param>
        public void NotifyAllDevices(bool newState)
        {
            List<IDeviceObserver> observersCopy;
            lock (_lock)
            {
                observersCopy = new List<IDeviceObserver>(_observers);
            }

            foreach (var observer in observersCopy)
            {
                var state = new DeviceState
                {
                    IsActive = newState,
                    DeviceType = observer.DeviceType, // 保持设备类型不变
                    UpdateTime = DateTime.Now
                };
                observer.UpdateState(state);
            }
        }
    }
}