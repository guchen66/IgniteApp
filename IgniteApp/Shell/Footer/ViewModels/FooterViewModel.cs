using IgniteApp.Bases;
using IgniteApp.Modules;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoCommands;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace IgniteApp.Shell.Footer.ViewModels
{
    public class FooterViewModel : ControlViewModelBase
    {
        #region--属性--

        private readonly IMaterialRepository db;
        private BindableCollection<ProductDto> _productList;

        public BindableCollection<ProductDto> ProductList
        {
            get => _productList;
            set => SetAndNotify(ref _productList, value);
        }
        private BindableCollection<MaterialInfo> _materialInfoList;

        public BindableCollection<MaterialInfo> MaterialInfoList
        {
            get => _materialInfoList??(_materialInfoList=new BindableCollection<MaterialInfo>());
            set => SetAndNotify(ref _materialInfoList, value);
        }
        private BindableCollection<StaticticDto> _staticticList;

        public BindableCollection<StaticticDto> StaticticList
        {
            get => _staticticList;
            set => SetAndNotify(ref _staticticList, value);
        }
        private StaticticDto _staticticInfo;

        public StaticticDto StaticticInfo
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
        public FooterViewModel(IMaterialRepository db)
        {
            this.db = db;
            InitData();
            QueryPlcStatus();
            UpdateCommand = MinidaoCommand.Create<int?>(ExecuteUpdate);
           
        }
        #endregion

        public void InitData()
        {
            ProductList = new BindableCollection<ProductDto>() 
            {
                 new ProductDto(){ProductName="123",Remark="2222"},
                  new ProductDto(){ProductName="234",Remark = "2222"},
            };
            AccessDbContext db=new AccessDbContext();
            
            var model=db.MaterialInfos.ToList();
            MaterialInfoList = new BindableCollection<MaterialInfo>(model);

            StaticticInfo = new StaticticDto
            {
                OkCount = 1,
                NgCount = 100,
                OutputCount = 121,
            };

        }

        public void ExecuteUpdate(int? i)
        {

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

        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set;}
    }
}
