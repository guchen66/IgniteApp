using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Extensions;
using IgniteDevices;
using IgniteDevices.TempAndHum;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Devices;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class TempAndHumViewModel : ViewModelBase
    {
        private double _temp;

        public double Temp
        {
            get => _temp;
            set => SetAndNotify(ref _temp, value);
        }

        private double _hum;

        public double Hum
        {
            get => _hum;
            set => SetAndNotify(ref _hum, value);
        }

        private bool _isConnTemp = false;

        public bool IsConnTemp
        {
            get => _isConnTemp;
            set => SetAndNotify(ref _isConnTemp, value);
        }

        private bool _isConnHum = false;

        public bool IsConnHum
        {
            get => _isConnHum;
            set => SetAndNotify(ref _isConnHum, value);
        }

        private string _content;

        public string Content
        {
            get => _content;
            set => SetAndNotify(ref _content, value);
        }

        private TempAndHumClient _tempAndHumClient;

        public IReadService _readService;
        public IDeviceProvider _deviceProvider;

        public TempAndHumViewModel()
        {
            //_readService = readService;

            // var s1 = _deviceProvider.DefaultTemp;
            //  var s2 = ServiceLocator.GetService<Temperature>();
            // Init();
        }

        private ManualResetEventSlim _manual = new ManualResetEventSlim(false);

        public void Init()
        {
            Task.Run(() =>
            {
                // _readService.Device.Read();
                while (!_manual.IsSet)
                {
                    _tempAndHumClient.Initinalized(_manual);
                    Execute.OnUIThread(() =>
                    {
                        Content = SysTempAndHum.Content;
                        Temp = SysTempAndHum.Temp;
                        Hum = SysTempAndHum.Hum;
                        IsConnTemp = SysTempAndHum.IsConnTemp;
                        IsConnHum = SysTempAndHum.IsConnHum;
                    });
                }
                MessageBox.Show("数据读取完成");
            });
        }

        public void ExecuteConn()
        {
            _tempAndHumClient.GetConnection();
        }

        public void Read()
        {
            _manual.Set();
        }
    }
}