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

        /// <summary>
        /// 标定时间
        /// </summary>
        private string _caliTime;

        public string CaliTime
        {
            get => _caliTime;
            set => SetProperty(ref _caliTime, value);
        }

        /// <summary>
        /// 标定结果
        /// </summary>
        private bool _result;

        public bool Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
    }
}