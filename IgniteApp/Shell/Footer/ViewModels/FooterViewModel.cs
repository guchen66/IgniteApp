using HandyControl.Controls;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Modules;
using IgniteApp.Shell.Home.Models;
using IgniteApp.ViewModels;
using IgniteDb;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoCommands;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Unity;

namespace IgniteApp.Shell.Footer.ViewModels
{
    public class FooterViewModel : ControlViewModelBase
    {
        #region--属性--

      
        private BindableCollection<ProductDto> _productList;

        public BindableCollection<ProductDto> ProductList
        {
            get => _productList??(_productList=new BindableCollection<ProductDto>());
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

        private PlcDto _plcDto;

        public PlcDto PlcDto
        {
            get => _plcDto;
            set => SetAndNotify(ref _plcDto, value);
        }

        private bool _isConn;

        public bool IsConn
        {
            get => _isConn;
            set => SetAndNotify(ref _isConn, value);
        }
        private readonly IPlcProvider _plcProvider;
        private readonly IMaterialRepository _materialRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReadService _readService;
        #endregion

        #region--ctor--
        public FooterViewModel(IMaterialRepository materialRepository, IReadService readService, IProductRepository productRepository, IPlcProvider plcProvider)
        {
            _materialRepository = materialRepository;
            _readService = readService;
            _productRepository = productRepository;
            _plcProvider = plcProvider;
            InitData();
            QueryPlcStatus();
            UpdateCommand = MinidaoCommand.Create<int?>(ExecuteUpdate);
          
        }
        #endregion

        public void InitData()
        {
            var materialModel = _materialRepository.GetAllMaterialInfo();//.MaterialInfos.ToList();
            MaterialInfoList.AddRange(materialModel);
            var productModel=_productRepository.GetAllProductInfo();
            ProductList.AddRange(productModel);
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
           // IsConn = false;
            Task.Run(() => 
            {
                IsConn=_plcProvider.ConnectionSiglePLC().IsSuccess;
                InitPlc();
               // IsConn = true;
            });
          
        }
        public void ExecuteReset()
        {
           
            StaticticInfo.SetReset();
        }
        /// <summary>
        /// 初始化的时候检查本地是否有保存的Plc信息
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            try
            {
                var xmlData = _readService.Read(LoginInfoLocation.LoginPath);

                if (xmlData == null)
                {
                    return;
                }
                var doc = XDocument.Parse(xmlData);
                //  var name=doc.Elements("LoginDto").Select(node=>node.Element("UserName").Value).ToList().FirstOrDefault();
                List<string> result = doc.Root.Elements().Select(node => node.Value).ToList();
                var isRememberValue = doc.Element("LoginDto").Element("IsConn").Value; // 获取元素的值

                // 将字符串转换为bool类型
                if (bool.TryParse(isRememberValue, out bool isRemember))
                {
                    if (isRemember)
                    {
                        PlcDto = XmlFolderHelper.Deserialize<PlcDto>(xmlData);
                    }
                    else
                    {
                        PlcDto = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 是否初始化数据表
        /// </summary>
        /// <returns></returns>
        private void InitPlc()
        {
            var path = DirectoryHelper.SelectDirectoryByName("appsetting.json");
            string jsonContent = File.ReadAllText(path);
            var plcConfig = JsonConvert.DeserializeObject<PlcConfig>(JObject.Parse(jsonContent)["PlcConfig"].ToString());
        }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set;}
    }
  
}
