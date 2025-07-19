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
    public class AxisItem : DaoViewModelBase
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

    /// <summary>
    /// 模拟数据，后期从数据库获取或者从本地获取
    /// </summary>
    public class AxisProvider : ObservableCollection<AxisItem>
    {
        private List<AxisItem> AxisItems = new List<AxisItem>();

        public AxisProvider()
        {
            _readService = ServiceLocator.GetService<IReadService>();
            _writeService = ServiceLocator.GetService<IWriteService>();
            InitializalData();
        }

        private readonly IReadService _readService;
        private readonly IWriteService _writeService;

        public void InitializalData()
        {
            var xmlData = _readService.Read(IgniteInfoLocation.AxisInfoPath);

            if (xmlData == null)
            {
                return;
            }
            _readService.Load(xmlData);

            List<AxisItem> AxisItems = XmlFolderHelper.Deserialize<List<AxisItem>>(xmlData);
            foreach (var item in AxisItems)
            {
                Add(item);
            }
        }

        protected void Load()
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
                var s = this as ObservableCollection<AxisItem>;
                AxisItems = new List<AxisItem>(s);

                var info = XmlFolderHelper.SerializeXML<List<AxisItem>>(AxisItems);
                _writeService.WriteString(IgniteInfoLocation.AxisInfoPath, info);
                var xmlData = _readService.Read(IgniteInfoLocation.AxisInfoPath);

                if (xmlData == null)
                {
                    return;
                }
                var doc = XDocument.Parse(xmlData);
            }
            catch (Exception ex)
            {
            }
        }
    }
}