using HandyControl.Controls;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using IT.Tangdao.Framework.DaoCommands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string PageTitle => "";
        private readonly IReadService _readService;

        public CO2TeachViewModel(IReadService readService)
        {
            _readService = readService;
            SetCommand = MinidaoCommand.Create<object>(ExecuteSet);
        }

        public bool CanNavigateAway()
        {
            return false;
        }

        public void OnNavigatedFrom()
        {
            MessageBox.Ask("准备从CO2离开");
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
            Name = parameter.Get<string>("Name");
        }

        public void ExecuteSet()
        {
        }

        public void ExecuteSet(object obj)
        {
        }

        public ICommand SetCommand { get; set; }
    }
}