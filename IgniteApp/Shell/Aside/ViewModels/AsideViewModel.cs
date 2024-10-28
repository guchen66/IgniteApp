using IgniteAdmin.Managers.Main;
using IgniteApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Aside.ViewModels
{
    public class AsideViewModel:ControlViewModelBase
    {

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
