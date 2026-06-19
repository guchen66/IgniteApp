using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Mvvm;
using IT.Tangdao.Framework.Serializers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;

namespace IgniteApp.Shell.Set.Models
{
    public class AxisItem : ViewModelBase
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
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(AxisProvider));

        public AxisProvider()
        {
            _contentAccess = ServiceLocator.GetService<IContentAccess>();

            InitializalData();
        }

        private readonly IContentAccess _contentAccess;

        public void InitializalData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "AxisInfo.xml");
            var xmlData = _contentAccess.Default.Read(foldPath).Content;
            if (xmlData == null)
            {
                return;
            }
            // _contentAccess.Current.Load(xmlData);
            List<AxisItem> AxisItems = _contentAccess.Cache.DeserializeCache<List<AxisItem>>(foldPath, DaoFileType.Xml);
            //  List<AxisItem> AxisItems = XmlFolderHelper.Deserialize<List<AxisItem>>(xmlData);
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

                var info = TangdaoXmlConvert.SerializeXML<List<AxisItem>>(AxisItems);
                var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "AxisInfo.xml");
                _contentAccess.Default.Write(info, foldPath);
                var xmlData = _contentAccess.Default.Read(foldPath).Content;

                if (xmlData == null)
                {
                    return;
                }
                var doc = XDocument.Parse(xmlData);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}