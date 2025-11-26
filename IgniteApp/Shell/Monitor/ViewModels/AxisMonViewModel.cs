using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Shell.Monitor.Models;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Contracts;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class AxisMonViewModel : ViewModelBase
    {
        private string _selectItem;

        public string SelectItem
        {
            get => _selectItem;
            set => SetAndNotify(ref _selectItem, value);
        }

        private BindableCollection<AxisMonItem> _axisMonItems;

        public BindableCollection<AxisMonItem> AxisMonItems
        {
            get => _axisMonItems;
            set => SetAndNotify(ref _axisMonItems, value);
        }

        public AxisMonViewModel()
        {
            AxisMonItems = new BindableCollection<AxisMonItem>()
            {
                new AxisMonItem(){Id=1,Name="123",Status=string.Empty,CreateTime=DateTime.Now,Remark="备注"},
            };
            this.Bind(viewModel => viewModel.SelectItem, (obj, sender) => DoExecute());

            UpdateUserCommand = new TangdaoCommand(ExecuteUpdate);
            DeleteUserCommand = new TangdaoCommand(ExecuteDelete);
        }

        private void ExecuteDelete()
        {
            MessageBox.Show("删除");
        }

        private void ExecuteUpdate()
        {
            MessageBox.Show("更新");
        }

        private void DoExecute()
        {
            var s1 = SelectItem;
        }

        public ICommand UpdateUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
    }
}