using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class ProcessViewModel:ControlViewModelBase
    {
        private ObservableCollection<ProcessItem> _processItems;

        public ObservableCollection<ProcessItem> ProcessItems
        {
            get => _processItems;
            set => SetAndNotify(ref _processItems, value);
        }

        public ProcessViewModel()
        {
            ProcessItems = new ObservableCollection<ProcessItem>()
            {
                 new ProcessItem() {Name="生产模式", IsFeeding=true,IsBoardMade=true,IsBoardCheck=true,IsSeal=true,IsSafe=true,IsCharge=true,IsBlanking=true},
                 new ProcessItem() {Name="空跑模式", IsFeeding=true,IsBoardMade=false,IsBoardCheck=false,IsSeal=false,IsSafe=false,IsCharge=false,IsBlanking=false},
            };
        }
    }
}
