using IgniteApp.Bases;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IgniteShared.Entitys;
using Stylet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Footer.ViewModels
{
    public class FooterViewModel : ControlViewModelBase
    {
        #region--产品信息--
        private BindableCollection<ProductInfo> _productList;

        public BindableCollection<ProductInfo> ProductList
        {
            get => _productList;
            set => SetAndNotify(ref _productList, value);
        }
        private BindableCollection<MaterialInfo> _materialInfoList;

        public BindableCollection<MaterialInfo> MaterialInfoList
        {
            get => _materialInfoList;
            set => SetAndNotify(ref _materialInfoList, value);
        }
        private BindableCollection<StaticticInfo> _staticticList;

        public BindableCollection<StaticticInfo> StaticticList
        {
            get => _staticticList;
            set => SetAndNotify(ref _staticticList, value);
        }
        private StaticticInfo _staticticInfo;

        public StaticticInfo StaticticInfo
        {
            get => _staticticInfo;
            set => SetAndNotify(ref _staticticInfo, value);
        }
        private string _name;

        public string PlcColor
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }
        private bool _isConn;

        public bool IsConn
        {
            get => _isConn;
            set => SetAndNotify(ref _isConn, value);
        }

        #endregion

        #region--ctor--
        public FooterViewModel()
        {
            InitData();
            QueryPlcStatus();
           
        }
        #endregion

        public void InitData()
        {
            ProductList = new BindableCollection<ProductInfo>() 
            {
                 new ProductInfo(){ProductName="123",Remark="2222"},
                  new ProductInfo(){ProductName="234",Remark = "2222"},
            };
            MaterialInfoList = new BindableCollection<MaterialInfo>() 
            {
                new MaterialInfo(){Station="第一工位",Status="已处理"},
                new MaterialInfo(){Station="第二工位",Status="已处理"},
                new MaterialInfo(){Station="第三工位",Status="未处理"},
                new MaterialInfo(){Station="第一工位",Status="已处理"},
                new MaterialInfo(){Station="第二工位",Status="已处理"},
                new MaterialInfo(){Station="第三工位",Status="未处理"},
                new MaterialInfo(){Station="第一工位",Status="已处理"},
                new MaterialInfo(){Station="第二工位",Status="已处理"},
                new MaterialInfo(){Station="第三工位",Status="未处理"},
                new MaterialInfo(){Station="第一工位",Status="已处理"},
                new MaterialInfo(){Station="第二工位",Status="已处理"},
                new MaterialInfo(){Station="第三工位",Status="未处理"},
            };
            StaticticInfo = new StaticticInfo
            {
                OkCount = 1,
                NgCount = 100,
                OutputCount = 121,
            };

        }

        public void QueryPlcStatus()
        {
            Task.Run(() => 
            {
                PlcColor = "Red";
                IsConn = true;
            });
          
        }
        public void ExecuteReset()
        {
            PlcColor = "Green";
            StaticticInfo.SetReset();
        }
    }
}
