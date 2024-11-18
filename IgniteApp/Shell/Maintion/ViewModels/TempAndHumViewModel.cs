using IgniteApp.Bases;
using IgniteDevices.TempAndHum;
using IgniteShared.Globals.System;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class TempAndHumViewModel : ControlViewModelBase
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

		private bool _isConnTemp=false;

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
        TempAndHumClient _tempAndHumClient;
        public TempAndHumViewModel()
        {
            _tempAndHumClient=ServiceLocator.GetService<TempAndHumClient>();
            Init();
          
        }
      
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
		public void Init()
		{
           
            Task.Run(async () => 
            {
                await _tempAndHumClient.Modify();
                while (!_manualResetEvent.WaitOne(500))
                {
                    Temp = SysTempAndHum.Temp;
                    Hum = SysTempAndHum.Hum;
                    IsConnTemp = SysTempAndHum.IsConnTemp;
                    IsConnHum = SysTempAndHum.IsConnHum;
                }
            });
           
        
        /*    while (!_manualResetEvent.WaitOne(500))
            {
               
            }*/
			/*Execute.OnUIThread(() => 
			{
               
            });*/
        }

    }
}
