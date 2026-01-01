using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Events;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Events;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class DefaultViewModel : ViewModelBase, IHandle<ProductUpdateEvent>
    {
        #region--属性--

        private ProductDto _productDto;

        public ProductDto ProductDto
        {
            get => _productDto ?? (_productDto = new ProductDto());
            set => SetAndNotify(ref _productDto, value);
        }

        private BindableCollection<ProductDto> _productList;

        public BindableCollection<ProductDto> ProductList
        {
            get => _productList ?? (_productList = new BindableCollection<ProductDto>());
            set => SetAndNotify(ref _productList, value);
        }

        private ObservableCollection<MaterialInfo> _materialInfoList;

        public ObservableCollection<MaterialInfo> MaterialInfoList
        {
            get => _materialInfoList ?? (_materialInfoList = new ObservableCollection<MaterialInfo>());
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
        private readonly IContentAccess _contentAccess;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region--ctor--

        public DefaultViewModel(IMaterialRepository materialRepository, IContentAccess contentAccess, IProductRepository productRepository, IEventAggregator eventAggregator)
        {
            _materialRepository = materialRepository;
            _contentAccess = contentAccess;
            _productRepository = productRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            InitData();

            UpdateCommand = MinidaoCommand.Create<int?>(ExecuteUpdate);
            DeleteCommand = new TangdaoCommand(ExecuteDelete);
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

            if (MaterialInfoList.Count == 0)
            {
                var faker = new TangdaoDataFaker<MaterialInfo>();
                MaterialInfoList = faker.Build(20).ToObservableCollection();
            }
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

        public void ExecuteDelete()
        {
            MessageBox.Show("i.ToString()");
        }

        protected override void OnActivate()
        {
        }

        public void ExecuteReset()
        {
            StaticticInfo.SetReset();
        }

        public void Handle(ProductUpdateEvent message)
        {
            if (message == null)
            {
                string path = Path.Combine(IgniteInfoLocation.Cache, "ProductDto.xml");
                _contentAccess.Default.Read(path).AsXml().SelectNodes<ProductUpdateEvent>();
            }
            ProductDto.ProductId = message.ProductId;
            ProductDto.ProductName = message.ProductName;
            ProductDto.Remark = message.Remark;
            ProductDto.UpdateTime = message.UpdateTime;
        }

        #endregion

        #region--命令--

        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion
    }
}