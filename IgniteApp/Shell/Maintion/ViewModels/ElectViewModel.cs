using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.Args;
using IgniteApp.Shell.Maintion.Models;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Notices;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.DaoTasks;
using IT.Tangdao.Framework.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MessageBox = HandyControl.Controls.MessageBox;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class ElectViewModel : BaseDeviceViewModel
    {
        private UsualArgs _usualArgs;

        public UsualArgs UsualArgs
        {
            get => _usualArgs ?? (_usualArgs = new UsualArgs());
            set => SetAndNotify(ref _usualArgs, value);
        }

        /// <summary>
        /// 单事件
        /// </summary>
        private ElectricityModel _singleEventElect;

        public ElectricityModel SingleEventElect
        {
            get => _singleEventElect ?? (_singleEventElect = new ElectricityModel());
            set => SetAndNotify(ref _singleEventElect, value);
        }

        /// <summary>
        /// 多事件
        /// </summary>
        private ElectricityModel _manyEventElect;

        public ElectricityModel ManyEventElect
        {
            get => _manyEventElect ?? (_manyEventElect = new ElectricityModel());
            set => SetAndNotify(ref _manyEventElect, value);
        }

        private ElectStatus _electStatus;

        public ElectStatus ElectStatus
        {
            get => _electStatus ?? (_electStatus = new ElectStatus());
            set => SetAndNotify(ref _electStatus, value);
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetAndNotify(ref _statusMessage, value);
        }

        private bool _isStatusPopupVisible;

        public bool IsStatusPopupVisible
        {
            get => _isStatusPopupVisible;
            set => SetAndNotify(ref _isStatusPopupVisible, value);
        }

        private ObservableCollection<ElectricityModel> _eLectModelList;

        public ObservableCollection<ElectricityModel> ELectModelList
        {
            get => _eLectModelList ?? (_eLectModelList = new ObservableCollection<ElectricityModel>());
            set => SetAndNotify(ref _eLectModelList, value);
        }

        private string _tag;

        public string Tag
        {
            get => _tag;
            set => SetAndNotify(ref _tag, value);
        }

        public ElectricityMotion _electricityMotion;
        public ElectPriceTrace _electricityPriceTrace;
        public ICommand CheckCommand { get; set; }
        public ICommand CheckPriceCommand { get; set; }

        public ElectViewModel(IContentAccess contentAccess) : base("Elect")
        {
            ELectModelList = new ObservableCollection<ElectricityModel>()
            {
                new ElectricityModel(){ Id=1,Name="电流表1" ,CurrentValue=10,Range=25,Status=TangdaoActive.Disabled,IElectService=new ElectService()},
                new ElectricityModel(){ Id=2,Name="电流表2" ,CurrentValue=10,Range=25,Status=TangdaoActive.Disabled,IElectService=new ElectService()}
            };
            CheckCommand = MinidaoCommand.Create(ExecuteCheck);
            CheckPriceCommand = MinidaoCommand.Create(ExecuteCheckPrice);
            _electricityMotion = new ElectricityMotion();
            _electricityMotion.StateChanged += OnStateChanged;
        }

        private void ExecuteCheck()
        {
            //StatusMessage = string.Empty; // 先清除旧状态

            Application.Current.Dispatcher.Invoke(() =>
            {
                ElectricityArgs electricityArgs = new ElectricityArgs(ManyEventElect);
                _electricityMotion.CheckState(electricityArgs);
            });
        }

        private void ExecuteCheckPrice()
        {
        }

        private void OnStateChanged(object sender, ElectricityArgs e)
        {
            if (e.ElectricityModel.CurrentValue > 20)
            {
                StatusMessage = "温度超载";
                IsStatusPopupVisible = true;
                //  MessageBox.Show("温度超载");
            }
            if (e.ElectricityModel.CurrentValue < 0)
            {
                MessageBox.Show("温度过低");
                StatusMessage = "温度过低";
                //  IsStatusPopupVisible = true;
            }
        }

        private readonly Dictionary<int, ElectAdapter> _adapters = new Dictionary<int, ElectAdapter>();

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <param name="model"></param>
        public void StartMonitor(ElectricityModel model)
        {
            if (_adapters.ContainsKey(model.Id))
                return;

            var adapter = new ElectAdapter(model.IElectService, model);
            adapter.StatusChanged += OnStateChanged;
            _adapters.Add(model.Id, adapter);
        }

        private void OnStateChanged(ElectricityModel model)
        {
            model.CurrentValue = model.IElectService.CurrentValue;
            if (model.CurrentValue > 30)
            {
                StatusMessage = "温度超载";
                IsStatusPopupVisible = true;
            }
            else if (model.CurrentValue < 0)
            {
                StatusMessage = "温度过低";
                IsStatusPopupVisible = true;
            }
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        /// <param name="model"></param>
        public void StopMonitor(ElectricityModel model)
        {
            if (_adapters.TryGetValue(model.Id, out var adapter))
            {
                adapter.StatusChanged -= OnStateChanged;
                adapter.Dispose();
                _adapters.Remove(model.Id);
            }
        }

        /// <summary>
        /// 测试Add
        /// </summary>
        /// <param name="model"></param>
        public void EditValue(ElectricityModel model)
        {
            model.IElectService.CurrentValue++;
        }

        /// <summary>
        /// 测试Delete
        /// </summary>
        /// <param name="model"></param>
        public void EditValue2(ElectricityModel model)
        {
            model.IElectService.CurrentValue = -1;
        }
    }
}