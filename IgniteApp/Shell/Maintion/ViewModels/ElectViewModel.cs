using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.Models;
using IT.Tangdao.Framework.DaoCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class ElectViewModel : ViewModelBase
    {
        private ElectricityModel _electricity;

        public ElectricityModel Electricity
        {
            get => _electricity;
            set => SetAndNotify(ref _electricity, value);
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

        public ElectricityMotion _electricityMotion;
        public ElectPriceTrace _electricityPriceTrace;
        public ICommand CheckCommand { get; set; }
        public ICommand CheckPriceCommand { get; set; }

        public ElectViewModel()
        {
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
            _electricityMotion.CheckElectricity(Electricity);
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
    }
}