using IgniteApp.Bases;
using IgniteApp.Shell.Monitor.Models;
using IgniteDevices.PLC;
using IgniteShared.Enums;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class PlcMonViewModel : ViewModelBase
    {
        private SelectModes _selectedMode;

        public SelectModes SelectedMode
        {
            get => _selectedMode = SelectModes.Load;
            set => SetAndNotify(ref _selectedMode, value);
        }

        private string _selectItem;

        public string SelectItem
        {
            get => _selectItem;
            set => SetAndNotify(ref _selectItem, value);
        }

        private BindableCollection<PlcMonItem> _plcMonItems;

        public BindableCollection<PlcMonItem> PlcMonItems
        {
            get => _plcMonItems;
            set => SetAndNotify(ref _plcMonItems, value);
        }

        public PlcMonViewModel()
        {
            /*  SelectedMode = new string[]
              {
                  "全部","Load","UpLoad"
              };*/

            this.Bind(viewModel => viewModel.SelectItem, (obj, sender) => DoExecute());
        }

        private void DoExecute()
        {
            var s1 = SelectItem;
        }

        public void AutoTriggerAlarm()
        {
            OmronManager.AlarmChenged?.Invoke("模拟报警_温度过高");
        }

        public void AutoTriggerAlarm2()
        {
            OmronManager.AlarmChenged?.Invoke("模拟报警_采集数据过大");
        }

        public void AutoTriggerAlarm3()
        {
            OmronManager.AlarmChenged?.Invoke("模拟报警_集尘器报警");
        }
    }
}