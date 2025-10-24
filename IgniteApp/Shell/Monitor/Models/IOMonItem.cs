using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Monitor.Models
{
    public class IOMonItem:DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string remark;

        public string Remark
        {
            get => remark;
            set => SetProperty(ref remark, value);
        }

    }
}
