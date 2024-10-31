using IT.Tangdao.Framework.DaoMvvm;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.Models
{
    public class AxisItem:DaoViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _axisName;

        public string AxisName
        {
            get => _axisName;
            set => SetProperty(ref _axisName, value);
        }

        private double _axisMinValue;

        public double AxisMinValue
        {
            get => _axisMinValue;
            set => SetProperty(ref _axisMinValue, value);
        }

        private double _axisMaxValue;

        public double AxisMaxValue
        {
            get => _axisMaxValue;
            set => SetProperty(ref _axisMaxValue, value);
        }

    }

    public class AxisProvider: ObservableCollection<AxisItem>
    {
        public AxisProvider()
        {
            Add(
                new AxisItem()
                {
                    Title = "标题1",
                    AxisName = "1轴",
                   // Password = "123"
                }
            );
            Add(
                new AxisItem()
                {
                    Title = "标题2",
                    AxisName = "2轴",
                    //Password = "456"
                }
            );
            Add(
                new AxisItem()
                {
                    Title = "标题3",
                    AxisName = "3轴",
                  //  Password = "789"
                }
            );
            Add(
               new AxisItem()
               {
                   Title = "标题1",
                   AxisName = "4轴",
                   // Password = "123"
               }
           );
            Add(
                new AxisItem()
                {
                    Title = "标题2",
                    AxisName = "5轴",
                    //Password = "456"
                }
            );
            Add(
                new AxisItem()
                {
                    Title = "标题3",
                    AxisName = "6轴",
                    //  Password = "789"
                }
            );
        }
        public void GetAxisItems()
        {
            
        }

    }

   
}
