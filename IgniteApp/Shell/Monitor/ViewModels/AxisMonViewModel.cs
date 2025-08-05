using IgniteApp.Bases;
using IgniteApp.Shell.Monitor.Models;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Bind(viewModel => viewModel.SelectItem, (obj, sender) => DoExecute());
        }

        private void DoExecute()
        {
            var s1 = SelectItem;
        }
    }
}