using IgniteApp.Shell.Maintion.Args;
using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Models
{
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

        private SingleState _status;

        public SingleState Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }

    public enum SingleState
    {
        Disabled = 0,
        Enabled = 1
    }

    public class ElectricityMotion
    {
        public event EventHandler<ElectricityArgs> OverLoad;   //电流超载

        public event EventHandler<ElectricityArgs> LowLoad;    //电流过低

        public void CheckElectricity(ElectricityModel model)
        {
            ElectricityArgs args = new ElectricityArgs(model);
            //if (model.Value > 39)
            //{
            //    OverLoad?.Invoke(this, args);
            //}
            //else if (model.Value < 0)
            //{
            //    LowLoad?.Invoke(this, args);
            //}
        }
    }
}