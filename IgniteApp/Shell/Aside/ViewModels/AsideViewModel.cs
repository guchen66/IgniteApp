using IgniteAdmin.Managers.Main;
using IgniteApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Shell.Aside.ViewModels
{
    public class AsideViewModel:ControlViewModelBase
    {
        private string _currentTime;

        public string CurrentTime
        {
            get => _currentTime;
            set => SetAndNotify(ref _currentTime, value);
        }

        DispatcherTimer timer;
        public AsideViewModel()
        {

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString();
        }

        #region--方法--

        public void ExecuteManual()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public void ExecuteAuto()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public void ExecuteStartRun()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public void ExecuteStop()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public void ExecuteStep()
        {
            PlcManager.GetPlcSignal<int>(1);
        }

        public void ExecuteInit()
        {
            PlcManager.GetPlcSignal<int>(1);
        }
        #endregion
    }
}
