using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Events;
using IgniteApp.Shell.Monitor.Models;
using IgniteDevices.PLC;
using IgniteDevices.PLC.Services;
using IgniteShared.Enums;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Paths;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Media.Imaging;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class PlcMonViewModel : ViewModelBase
    {
        private string _selectedItem;

        public string SelectedItem
        {
            get => _selectedItem;
            set => SetAndNotify(ref _selectedItem, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        private string _receiveData;

        public string ReceiveData
        {
            get => _receiveData;
            set => SetAndNotify(ref _receiveData, value);
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image;
            set => SetAndNotify(ref _image, value);
        }

        private BindableCollection<PlcMonItem> _plcMonItems;

        public BindableCollection<PlcMonItem> PlcMonItems
        {
            get => _plcMonItems ?? (_plcMonItems = new BindableCollection<PlcMonItem>());
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
            this.BindAndInvoke(viewModel => viewModel.SelectedIndex, (obj, sender) => SelectOpenPlcList());
            var imagePath = TangdaoPath.Instance.Solution().Combine("Assets", "Images", "Light.png").Build();
            var allImagePath = TangdaoPath.Instance.Solution().Combine("Assets", "Images").Build().EnumerateFiles("*.png");
            Image = new BitmapImage(new Uri(imagePath.Value));
        }

        public void Export()
        {
            try
            {
                var template = PathTemplate.Create("{Solution}/Exports/{Date}/report.txt");
                var exportPath = template.Resolve(new
                {
                    Solution = IgniteInfoLocation.Cache,
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                });

                Directory.CreateDirectory(exportPath.Parent().Value);
                File.WriteAllText(exportPath.Value, $"Hello Stylet! Exported at {DateTime.Now}");
                var backup = exportPath.Backup(".bak");
            }
            catch (Exception ex)
            {
            }
        }

        private void SelectOpenPlcList()
        {
            // GetPlcData(SelectedItem);
            GetPlcDataIndex(SelectedIndex);
        }

        private BindableCollection<PlcMonItem> GetPlcDataIndex(int plcUnit)
        {
            PlcMonItems.Clear();
            if (plcUnit == 1)
            {
                PlcMonItems = new BindableCollection<PlcMonItem>()
                {
                    new PlcMonItem(){ Id=1,Name="西门子",Status="连接",Remark="未备注"},
                    new PlcMonItem(){ Id=2,Name="西门子2",Status="连接",Remark="未备注"},
                };
            }
            else if (plcUnit == 2)
            {
                PlcMonItems = new BindableCollection<PlcMonItem>()
                {
                    new PlcMonItem(){ Id=1,Name="三菱1",Status="连接",Remark="未备注"},
                    new PlcMonItem(){ Id=2,Name="三菱2",Status="连接",Remark="未备注"},
                };
            }
            else if (plcUnit == 0)
            {
                PlcMonItems = new BindableCollection<PlcMonItem>()
                {
                    new PlcMonItem(){ Id=1,Name="未知",Status="未连接",Remark="未备注"},
                };
            }
            return PlcMonItems;
        }

        public void AutoTriggerAlarm()
        {
            OmronManager.AlarmChenged?.Invoke("模拟报警_温度过高");
        }

        public void AutoTriggerAlarm2()
        {
            OmronManager.AlarmErrorChenged?.Invoke(new AlarmMessage()
            {
                Id = 1,
                Name = "西门子",
                Solution = "关闭程序",
                AlarmLevel = AlarmLevel.Info,
            });
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