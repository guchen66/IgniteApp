using HandyControl.Controls;
using IgniteApp.Shell.ProcessParame.Services;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using IT.Tangdao.Framework.DaoAdmin.Sockets;
using IT.Tangdao.Framework.DaoCommands;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class CO2TeachViewModel : Screen, ITangdaoPage
    {
        private string _name = "未设置";

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        private bool _isStatusActive;
        private readonly ITangdaoChannel _channel;
        private readonly ITangdaoRequest _request;

        [DefaultValue(true)]
        public bool IsStatusActive
        {
            get => _isStatusActive;
            set => SetAndNotify(ref _isStatusActive, value);
        }

        public string PageTitle => "";
        private readonly IReadService _readService;

        public CO2TeachViewModel(IReadService readService, ITangdaoChannel channel, ITangdaoRequest request)
        {
            _readService = readService;
            _channel = channel;
            SetCommand = MinidaoCommand.Create(ExecuteSet);
            _request = request;
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
            //MessageBox.Ask("准备从CO2离开");
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
            if (_channel.IsConnected)
            {
                _isStatusActive = true;
            }
            Name = parameter.Get<string>("Name");
        }

        public async void ExecuteSet()
        {
            await _request.SendAsync("1");
        }

        public void ExecuteSet(object obj)
        {
            var s1 = IsStatusActive;
        }

        public ICommand SetCommand { get; set; }
    }
}