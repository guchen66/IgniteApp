using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Models
{
    public class ElectricityModel : DaoViewModelBase
    {
        public bool IsOverload { get; }
        public bool IsLow { get; }
        public double Value { get; }

        public ElectricityModel(bool isOverload, bool isLow, double value)
        {
            IsOverload = isOverload;
            IsLow = isLow;
            Value = value;
        }
    }

    public class ElectricityArgs : EventArgs
    {
        public ElectricityModel ElectricityModel { get; set; }

        public ElectricityArgs(ElectricityModel electricityModel)
        {
            ElectricityModel = electricityModel;
        }
    }

    public class ElectricityMotion
    {
        public event EventHandler<ElectricityArgs> OverLoad;   //电流超载

        public event EventHandler<ElectricityArgs> LowLoad;    //电流过低

        public void CheckElectricity(ElectricityModel model)
        {
            ElectricityArgs args = new ElectricityArgs(model);
            if (model.Value > 39)
            {
                OverLoad?.Invoke(this, args);
            }
            else if (model.Value < 0)
            {
                LowLoad?.Invoke(this, args);
            }
        }
    }
}