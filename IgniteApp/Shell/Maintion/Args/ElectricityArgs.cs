using IgniteApp.Shell.Maintion.Models;
using System;

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