using IgniteApp.Shell.Maintion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Args
{
    public class ElectricityArgs : EventArgs
    {
        public ElectricityModel ElectricityModel { get; set; }

        public ElectricityArgs(ElectricityModel electricityModel)
        {
            ElectricityModel = electricityModel;
        }
    }
}