using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public class TeachItem : DaoViewModelBase
    {
        private double _x1;

        public double X1
        {
            get => _x1;
            set => SetProperty(ref _x1, value);
        }

        private double _x2;

        public double X2
        {
            get => _x2;
            set => SetProperty(ref _x2, value);
        }

        private double _y1;

        public double Y1
        {
            get => _y1;
            set => SetProperty(ref _y1, value);
        }

        private double _y2;

        public double Y2
        {
            get => _y2;
            set => SetProperty(ref _y2, value);
        }

        public TeachItem(double x1, double x2, double y1, double y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }
    }
}