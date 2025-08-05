using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Events;
using IgniteApp.Shell.Monitor.Models;
using IgniteDevices.PLC;
using IgniteDevices.PLC.Services;
using IgniteShared.Enums;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

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

        private string _receiveData;

        public string ReceiveData
        {
            get => _receiveData;
            set => SetAndNotify(ref _receiveData, value);
        }

        private BindableCollection<PlcMonItem> _plcMonItems;

        public BindableCollection<PlcMonItem> PlcMonItems
        {
            get => _plcMonItems;
            set => SetAndNotify(ref _plcMonItems, value);
        }

        private IPlcCommunicator _plcCommunicator;
        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;
        private AlarmPopupManager _alarmPopupManager;

        public PlcMonViewModel(IPlcCommunicator plcCommunicator, IEventAggregator eventAggregator, AlarmPopupManager alarmPopupManager)
        {
            _plcCommunicator = plcCommunicator;
            _eventAggregator = eventAggregator;
            _alarmPopupManager = alarmPopupManager;
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

        public void GetData()
        {
            if (_plcCommunicator == null)
            {
                ReceiveData = "读取失败";
            }
            else
            {
                var data = _plcCommunicator.ReadSingleRegister(1);

                ReceiveData = data.Data.ToString();
                string hex = data.Data.ToString("X4"); // "3039" (16进制)
                string padded = data.Data.ToString("D6"); // "012345" (补零)

                if (data.Data > 23)
                {
                    OmronManager.AlarmChenged?.Invoke("模拟报警_温度过高");
                }
            }
        }

        public void SolvedAlarm()
        {
            _alarmPopupManager.CloseAlarmPopup();
        }
    }
}