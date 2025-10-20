using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.Services;
using IgniteApp.Shell.Set.Models;
using IT.Tangdao.Framework.Commands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class HardawreSetViewModel : Screen
    {
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        private NetSetItem _netSetItem;

        public NetSetItem NetSetItem
        {
            get => _netSetItem;
            set => SetAndNotify(ref _netSetItem, value);
        }

        public HardawreSetViewModel()
        {
            ConnectionCommand = new TangdaoCommand(ExecuteAllDeviceConn);
        }

        private void ExecuteAllDeviceConn()
        {
            // 通过中介者通知所有设备
            var newState = IsChecked = !IsChecked;

            DeviceMediator.Instance.NotifyAllDevices(newState);
        }

        public ICommand ConnectionCommand { get; set; }
    }
}