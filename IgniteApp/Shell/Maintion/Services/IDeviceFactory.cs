using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Shell.Set.Views;
using IT.Tangdao.Framework.Ioc;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Services
{
    /// <summary>
    /// 设备ViewModel工厂接口
    /// </summary>
    public interface IDeviceFactory
    {
        /// <summary>
        /// 创建光源ViewModel
        /// </summary>
      //  IDeviceObserver CreateLightViewModel();

        /// <summary>
        /// 创建电流ViewModel
        /// </summary>
      //  IDeviceObserver CreateCurrentViewModel();

        /// <summary>
        /// 创建电阻ViewModel
        /// </summary>
       // IDeviceObserver CreateResistanceViewModel();

        /// <summary>
        /// 创建指定类型的设备ViewModel
        /// </summary>
        IDeviceObserver CreateDeviceViewModel(string deviceType);
    }

    /// <summary>
    /// 设备观察者提供者接口
    /// </summary>
    public interface IDeviceProvider
    {
        /// <summary>
        /// 获取所有设备观察者
        /// </summary>
        IEnumerable<IDeviceObserver> GetDeviceObservers();

        /// <summary>
        /// 获取指定类型的设备观察者
        /// </summary>
        IDeviceObserver GetDeviceObserver(string deviceType);
    }

    /// <summary>
    /// 默认设备工厂实现
    /// </summary>
    public class DefaultDeviceFactory : IDeviceFactory
    {
        private readonly IContainer Container;

        public DefaultDeviceFactory(IContainer container = null)
        {
            Container = container;
        }

        public IDeviceObserver CreateLightViewModel()
        {
            return CreateViewModel<LightViewModel>();
        }

        public IDeviceObserver CreateCurrentViewModel()
        {
            return CreateViewModel<ElectViewModel>();
        }

        public IDeviceObserver CreateResistanceViewModel()
        {
            return CreateViewModel<ResistiveViewModel>();
        }

        public IDeviceObserver CreateDeviceViewModel(string deviceType)
        {
            switch (deviceType)
            {
                case "Light":
                    return CreateLightViewModel();
                //   break;

                case "Current":
                    return CreateCurrentViewModel();
                //  break;

                case "Resistance":
                    return CreateResistanceViewModel();
                // break;

                default:
                    throw new ArgumentException($"未知的: {deviceType}");
            }
        }

        private IDeviceObserver CreateViewModel<T>() where T : IDeviceObserver
        {
            // 如果有IOC容器，使用容器创建
            if (Container != null)
            {
                return Container.Get<T>();
            }

            // 否则使用反射创建（可以处理带参数的构造函数）
            return Activator.CreateInstance<T>();
        }
    }

    /// <summary>
    /// 默认设备提供者实现
    /// </summary>
    public class DefaultDeviceProvider : IDeviceProvider
    {
        private readonly IDeviceFactory _deviceFactory;
        private readonly List<string> _supportedDevices;

        public DefaultDeviceProvider(IDeviceFactory deviceFactory)
        {
            _deviceFactory = deviceFactory;
            _supportedDevices = new List<string>
        {
            "Light",
            "Current",
            "Resistance"
        };
        }

        public IEnumerable<IDeviceObserver> GetDeviceObservers()
        {
            var observers = new List<IDeviceObserver>();

            foreach (var deviceType in _supportedDevices)
            {
                var observer = GetDeviceObserver(deviceType);
                if (observer != null)
                {
                    observers.Add(observer);
                }
            }

            return observers;
        }

        public IDeviceObserver GetDeviceObserver(string deviceType)
        {
            try
            {
                return _deviceFactory.CreateDeviceViewModel(deviceType);
            }
            catch (Exception ex)
            {
                // 记录日志
                System.Diagnostics.Debug.WriteLine($"创建设备 {deviceType} 失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 注册支持的设备类型
        /// </summary>
        public void RegisterDeviceType(string deviceType)
        {
            if (!_supportedDevices.Contains(deviceType))
            {
                _supportedDevices.Add(deviceType);
            }
        }
    }

    /// <summary>
    /// 设备注册表实现
    /// </summary>
    public class DeviceRegistry : IDeviceRegistry
    {
        private readonly List<IDeviceObserver> _observers = new List<IDeviceObserver>();
        private readonly IDeviceProvider _deviceProvider;
        private readonly object _lock = new object();

        public DeviceRegistry(IDeviceProvider deviceProvider)
        {
            _deviceProvider = deviceProvider ?? throw new ArgumentNullException(nameof(deviceProvider));
        }

        public void RegisterAll()
        {
            lock (_lock)
            {
                // 通过设备提供者获取所有观察者
                var devices = _deviceProvider.GetDeviceObservers();

                foreach (var device in devices)
                {
                    if (device != null)
                    {
                        RegisterObserver(device);
                    }
                }
            }
        }

        public void RegisterObserver(IDeviceObserver observer)
        {
            lock (_lock)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                    DeviceMediator.Instance.Register(observer);
                }
            }
        }

        public IList<IDeviceObserver> GetAllObservers()
        {
            lock (_lock)
            {
                return new List<IDeviceObserver>(_observers);
            }
        }

        /// <summary>
        /// 注册指定类型的设备
        /// </summary>
        public void RegisterDevice(string deviceType)
        {
            var observer = _deviceProvider.GetDeviceObserver(deviceType);
            if (observer != null)
            {
                RegisterObserver(observer);
            }
        }
    }

    /// <summary>
    /// 设备注册表接口
    /// </summary>
    public interface IDeviceRegistry
    {
        /// <summary>
        /// 注册所有设备观察者
        /// </summary>
        void RegisterAll();

        /// <summary>
        /// 获取所有设备观察者
        /// </summary>
        IList<IDeviceObserver> GetAllObservers();

        /// <summary>
        /// 注册单个设备观察者
        /// </summary>
        void RegisterObserver(IDeviceObserver observer);
    }
}