using IgniteApp.Bases;
using IgniteApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class LightViewModel: ControlViewModelBase, INavigateEntry
    {
		private double _progress;

		public double Progress
        {
			get => _progress;
			set => SetAndNotify(ref _progress, value);
		}

        public LightViewModel()
        {
           
        }

        public async Task ExecuteGetProcess()
        {
            for (int i = 0; i <= 100; i++)
            {
               await Task.Delay(100);
                Progress = i;
            }
        }
    }
}
