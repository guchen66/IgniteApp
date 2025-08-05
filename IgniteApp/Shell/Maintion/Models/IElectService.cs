using IgniteApp.Shell.Maintion.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Models
{
    public interface IElectService
    {
        string Name { get; set; }
        double Range { get; set; }
        double CurrentValue { get; set; }
        Action StatusChanged { get; set; }

        event EventHandler<ElectricityArgs> AlarmStateChanged;
    }

    public class ElectService : IElectService
    {
        public string Name { get; set; }
        public double Range { get; set; }
        private double _currentValue;

        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                StatusChanged?.Invoke();
            }
        }

        public Action StatusChanged { get; set; }

        public event EventHandler<ElectricityArgs> AlarmStateChanged;
    }
}