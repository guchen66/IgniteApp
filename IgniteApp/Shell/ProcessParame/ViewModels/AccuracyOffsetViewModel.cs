using IgniteApp.Shell.ProcessParame.Models;
using IT.Tangdao.Framework.DaoMvvm;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class AccuracyOffsetViewModel : Screen
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        private ObservableCollection<OffsetModel> _accuracyOffsetList;

        public ObservableCollection<OffsetModel> AccuracyOffsetList
        {
            get => _accuracyOffsetList;
            set => SetAndNotify(ref _accuracyOffsetList, value);
        }

        public AccuracyOffsetViewModel()
        {
            Name = "1111";
            AccuracyOffsetList = new ObservableCollection<OffsetModel>()
            {
                new OffsetModel() {Id=1,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false, CutType="X1"},
                new OffsetModel() {Id=2,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X1"},
                new OffsetModel() {Id=3,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X1"},
                new OffsetModel() {Id=4,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X1"},
                new OffsetModel() {Id=5,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X2"},
                new OffsetModel() {Id=6,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X2"},
                new OffsetModel() {Id=7,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X2"},
                new OffsetModel() {Id=8,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X2"},
                new OffsetModel() {Id=9,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y1"},
                new OffsetModel() {Id=10,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y1"},
                new OffsetModel() {Id=11,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y1"},
                new OffsetModel() {Id=12,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y1"},
                new OffsetModel() {Id=13,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y2"},
                new OffsetModel() {Id=14,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y2"},
                new OffsetModel() {Id=15,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false,CutType = "Y2"},
                new OffsetModel() {Id=16,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y2"},
            };
        }

        public string[] OffsetList { get; set; } = new string[]
        {
            "X1","X2","Y1","Y2"
        };

        private double _offset;

        public double Offset
        {
            get => _offset;
            set => SetAndNotify(ref _offset, value);
        }

        public void OneKeyAdd(string name)
        {
            // 1. 使用 ToLookup 分组
            var lookup = AccuracyOffsetList.ToLookup(x => x.CutType);

            // 2. 找到对应 CutType 的所有项
            var items = lookup[name];

            // 3. 遍历并修改 X1 的值（+10）
            foreach (var item in items)
            {
                item.StartValue += Offset;
            }
        }

        public void OneKeyDecrease(string name)
        { // 1. 使用 ToLookup 分组
            var lookup = AccuracyOffsetList.ToLookup(x => x.CutType);

            // 2. 找到对应 CutType 的所有项
            var items = lookup[name];

            // 3. 遍历并修改 X1 的值（+10）
            foreach (var item in items)
            {
                item.StartValue -= Offset;
            }
        }
    }

    [TypeConverter(typeof(OffsetModel))]
    public class OffsetModelList : DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private bool _isXDirty;

        public bool IsXDirty
        {
            get => _isXDirty;
            set => SetProperty(ref _isXDirty, value);
        }

        private bool _isYDirty;

        public bool IsYDirty
        {
            get => _isYDirty;
            set => SetProperty(ref _isYDirty, value);
        }

        private double _startValue;

        public double StartValue
        {
            get => _startValue;
            set
            {
                SetProperty(ref _startValue, value);
                IsXDirty = true;
            }
        }

        private double _endValue;

        public double EndValue
        {
            get => _endValue;
            set
            {
                SetProperty(ref _endValue, value);
                IsYDirty = true;
            }
        }

        private string _cutTYpe;

        public string CutType
        {
            get => _cutTYpe;
            set => SetProperty(ref _cutTYpe, value);
        }
    }
}