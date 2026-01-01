using IgniteApp.Shell.Maintion.Args;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Mvvm;
using System;

namespace IgniteApp.Shell.Maintion.Models
{
    /// <summary>
    /// 电流表数据
    /// </summary>
    public class ElectricityModel : DaoViewModelBase
    {
        public IElectService IElectService { get; set; }
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

        private double _currentValue;

        public double CurrentValue
        {
            get => _currentValue;
            set => SetProperty(ref _currentValue, value);
        }

        private double _range;

        public double Range
        {
            get => _range;
            set => SetProperty(ref _range, value);
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private TangdaoActive _status;

        public TangdaoActive Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }

    public class ElectricityMotion
    {
        public event EventHandler<ElectricityArgs> StateChanged;   //电流超载或者电流过低，（电流状态改变）

        public void CheckState(ElectricityArgs electricityArgs)
        {
            StateChanged.Invoke(this, electricityArgs);
        }
    }
}