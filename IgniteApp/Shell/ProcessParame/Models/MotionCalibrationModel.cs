using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    /// <summary>
    /// 运动系标定
    /// </summary>
    public class MotionCalibrationModel : DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _caliType;

        public string CaliType
        {
            get => _caliType;
            set => SetProperty(ref _caliType, value);
        }

        private double _startValue;

        public double StartValue
        {
            get => _startValue;
            set => SetProperty(ref _startValue, value);
        }

        private double _endValue;

        public double EndValue
        {
            get => _endValue;
            set => SetProperty(ref _endValue, value);
        }
    }
}