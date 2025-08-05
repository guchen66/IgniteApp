using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Args
{
    public class UsualArgs : DaoViewModelBase
    {
        private double[] _doubleValue;

        public double[] DoubleValue
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }
    }
}