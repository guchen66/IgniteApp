using IgniteApp.Shell.Set.Models;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Messaging;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
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

        private ITangdaoLogger _logger = TangdaoLogger.Get(typeof(HardawreSetViewModel));

        public HardawreSetViewModel()
        {
            ConnectionCommand = new TangdaoCommand(ExecuteAllDeviceConn);
        }

        private void ExecuteAllDeviceConn()
        {
            // 通过中介者通知所有设备
            var newState = IsChecked = !IsChecked;
            MessageContext noticeContext = new MessageContext();
            noticeContext.CurrentState = newState;
            noticeContext.CurrentTime = DateTime.Now;
            TangdaoMessenger.Instance.NotifyObservers(noticeContext);
            _logger.WriteLocal($"全部状态已通知更改,通知时间{noticeContext.CurrentTime}");
        }

        public ICommand ConnectionCommand { get; set; }

        protected override void OnClose()
        {
            base.OnClose();
        }
    }
}