using HandyControl.Controls;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Converters;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Models;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Stylet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.IO;
using IgniteApp.Common;
using IgniteApp.Dialogs.ViewModels;
using StyletIoC;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class DefaultViewModel : ViewModelBase// IScreenState
    {
        #region--属性--
        private BindableCollection<ProductDto> _productList;

        public BindableCollection<ProductDto> ProductList
        {
            get => _productList ?? (_productList = new BindableCollection<ProductDto>());
            set => SetAndNotify(ref _productList, value);
        }

        private BindableCollection<MaterialInfo> _materialInfoList;

        public BindableCollection<MaterialInfo> MaterialInfoList
        {
            get => _materialInfoList ?? (_materialInfoList = new BindableCollection<MaterialInfo>());
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
            get => _staticticInfo ?? (_staticticInfo = new StaticticDto());
            set => SetAndNotify(ref _staticticInfo, value);
        }

        private string _imageURI;

        public string ImageURI
        {
            get => _imageURI;
            set => SetAndNotify(ref _imageURI, value);
        }

        [Inject]
        public ImageContextMenuViewModel ImageContextMenuViewModel { get; set; }

        private readonly IMaterialRepository _materialRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReadService _readService;
        #endregion

        #region--ctor--

        public DefaultViewModel(IMaterialRepository materialRepository, IReadService readService, IProductRepository productRepository)
        {
            _materialRepository = materialRepository;
            _readService = readService;
            _productRepository = productRepository;
            InitData();

            UpdateCommand = MinidaoCommand.Create<int?>(ExecuteUpdate);
            string path = "../../../Assets/Images/404.png";
            ImageURI = path;
            IgniteEventHandler.StatisticUpdated -= OnStatisticUpdated;
            IgniteEventHandler.StatisticUpdated += OnStatisticUpdated;
        }

        #endregion

        #region--方法--

        private void Init()
        {
            //ImageContextMenuViewModel imageContextMenuViewModel = new ImageContextMenuViewModel();
            //imageContextMenuViewModel.CreateGrayImage(ImageURI);
        }

        public void InitData()
        {
            var materialModel = _materialRepository.GetAllMaterialInfo();//.MaterialInfos.ToList();
            MaterialInfoList.AddRange(materialModel);
            var productModel = _productRepository.GetAllProductInfo();
            ProductList.AddRange(productModel);
        }

        private void OnStatisticUpdated(object sender, StatisticUpdateEventArgs e)
        {
            if (e.StatisticDto != null)
            {
                StaticticInfo = e.StatisticDto;
            }
        }

        public void ExecuteUpdate(int? i)
        {
        }

        protected override void OnActivate()
        {
        }

        public void ExecuteReset()
        {
            StaticticInfo.SetReset();
        }

        #endregion

        #region--命令--

        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion
    }
}