using IgniteShared.Dtos;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoMvvm;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        List<AxisItem> AxisItems = new List<AxisItem>();
        public AxisProvider()
        {
            _readService = ServiceLocator.GetService<IReadService>();
            _writeService= ServiceLocator.GetService<IWriteService>();
            Load();
        }
      
     

        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        protected  void Load()
        {
            try
            {
               
                for (int i = 0; i < 5; i++)
                {
                    Add(new AxisItem() 
                    {
                        Title = "标题" + i,
                        AxisName = "轴" + i,
                        AxisMaxValue = i,
                        AxisMinValue = i
                    });
                }
                var info = XmlFolderHelper.SerializeXML<List<AxisItem>>(AxisItems);
                _writeService.WriteString(DeviceInfoLocation.AxisInfoPath, info);
                var xmlData = _readService.Read(DeviceInfoLocation.AxisInfoPath);

                if (xmlData == null)
                {
                    return;
                }
                var doc = XDocument.Parse(xmlData);
                //  var name=doc.Elements("LoginDto").Select(node=>node.Element("UserName").Value).ToList().FirstOrDefault();
              /*  List<string> result = doc.Root.Elements().Select(node => node.Value).ToList();
                var isRememberValue = doc.Element("LoginDto").Element("IsRemember").Value; // 获取元素的值

                // 将字符串转换为bool类型
                if (bool.TryParse(isRememberValue, out bool isRemember))
                {
                    if (isRemember)
                    {
                        LoginDto = XmlFolderHelper.Deserialize<LoginDto>(xmlData);
                    }
                    else
                    {
                        LoginDto = null;
                    }
                }*/
            }
            catch (Exception ex)
            {
               
            }
        }

    }

   
}
