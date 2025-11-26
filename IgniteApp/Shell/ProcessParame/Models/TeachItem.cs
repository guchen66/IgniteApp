using IT.Tangdao.Framework.Attributes;
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
        private decimal _x1;

        [TangdaoFake(Min = 0, Max = 100, Point = 3)]
        public decimal X1
        {
            get => _x1;
            set => SetProperty(ref _x1, value);
        }

        private decimal _x2;

        [TangdaoFake(Min = 0, Max = 100, Point = 3)]
        public decimal X2
        {
            get => _x2;
            set => SetProperty(ref _x2, value);
        }

        private decimal _y1;

        public decimal Y1
        {
            get => _y1;
            set => SetProperty(ref _y1, value);
        }

        private decimal _y2;

        [TangdaoFake()]
        public decimal Y2
        {
            get => _y2;
            set => SetProperty(ref _y2, value);
        }
    }
}