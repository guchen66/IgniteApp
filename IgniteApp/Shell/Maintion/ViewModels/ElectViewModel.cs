using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.Args;
using IgniteApp.Shell.Maintion.Models;
using IT.Tangdao.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class ElectViewModel : BaseDeviceViewModel
    {
        private UsualArgs _usualArgs;

        public UsualArgs UsualArgs
        {
            get => _usualArgs ?? (_usualArgs = new UsualArgs());
            set => SetAndNotify(ref _usualArgs, value);
        }

        private ElectricityModel _selectedItem;

        public ElectricityModel SelectedItem
        {
            get => _selectedItem;
            set => SetAndNotify(ref _selectedItem, value);
        }

        private ElectStatus _electStatus;

        public ElectStatus ElectStatus
        {
            get => _electStatus ?? (_electStatus = new ElectStatus());
            set => SetAndNotify(ref _electStatus, value);
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetAndNotify(ref _statusMessage, value);
        }

        private bool _isStatusPopupVisible;

        public bool IsStatusPopupVisible
        {
            get => _isStatusPopupVisible;
            set => SetAndNotify(ref _isStatusPopupVisible, value);
        }

        private ObservableCollection<ElectricityModel> _eLectModelList;

        public ObservableCollection<ElectricityModel> ELectModelList
        {
            get => _eLectModelList ?? (_eLectModelList = new ObservableCollection<ElectricityModel>());
            set => SetAndNotify(ref _eLectModelList, value);
        }

        public ElectricityMotion _electricityMotion;
        public ElectPriceTrace _electricityPriceTrace;
        public ICommand CheckCommand { get; set; }
        public ICommand CheckPriceCommand { get; set; }

        public ElectViewModel() : base("Elect")
        {
            ELectModelList = new ObservableCollection<ElectricityModel>()
            {
                new ElectricityModel(){ Id=1,Name="电流表1" ,CurrentValue=10,Range=25,Status=SingleState.Disabled,IElectService=new ElectService()},
                new ElectricityModel(){ Id=2,Name="电流表2" ,CurrentValue=10,Range=25,Status=SingleState.Disabled,IElectService=new ElectService()}
            };
            CheckCommand = MinidaoCommand.Create(ExecuteCheck);
            CheckPriceCommand = MinidaoCommand.Create(ExecuteCheckPrice);
            _electricityMotion = new ElectricityMotion();
            _electricityPriceTrace = new ElectPriceTrace();
            _electricityMotion.OverLoad += OnOverLoadChanged;
            _electricityMotion.LowLoad += OnLowLoadChanged;
            //_electricityPriceTrace.PriceChanged += OnPriceChanged;
        }

        private void OnPriceChanged(object sender, string e)
        {
            if (ElectStatus.ElectPrice > 100)
            {
                e = "大了";
            }
        }

        private void ExecuteCheck()
        {
            IsStatusPopupVisible = false;
            //StatusMessage = string.Empty; // 先清除旧状态
            _electricityMotion.CheckElectricity(SelectedItem);
        }

        private void ExecuteCheckPrice()
        {
        }

        private void OnOverLoadChanged(object sender, ElectricityArgs e)
        {
            StatusMessage = "温度超载";
            IsStatusPopupVisible = true;
        }

        private void OnLowLoadChanged(object sender, ElectricityArgs e)
        {
            StatusMessage = "温度过低";
            IsStatusPopupVisible = true;
        }

        private readonly Dictionary<int, ElectAdapter> _adapters = new Dictionary<int, ElectAdapter>();

        public void StartMonitor(ElectricityModel model)
        {
            if (_adapters.ContainsKey(model.Id))
                return;

            var adapter = new ElectAdapter(model.IElectService, model);
            adapter.StatusChanged += OnStateChanged;
            _adapters.Add(model.Id, adapter);
        }

        private void OnStateChanged(ElectricityModel model)
        {
            model.CurrentValue = model.IElectService.CurrentValue;
            if (model.CurrentValue > 30)
            {
                StatusMessage = "温度超载";
                IsStatusPopupVisible = true;
            }
            else if (model.CurrentValue < 0)
            {
                StatusMessage = "温度过低";
                IsStatusPopupVisible = true;
            }
        }

        public void StopMonitor(ElectricityModel model)
        {
            if (_adapters.TryGetValue(model.Id, out var adapter))
            {
                adapter.StatusChanged -= OnStateChanged;
                adapter.Dispose();
                _adapters.Remove(model.Id);
            }
        }

        public void EditValue(ElectricityModel model)
        {
            model.IElectService.CurrentValue++;
            //  model.CurrentValue = 11;
        }

        public void EditValue2(ElectricityModel model)
        {
            model.IElectService.CurrentValue = -1;
            //  model.CurrentValue = 11;
        }
    }
}