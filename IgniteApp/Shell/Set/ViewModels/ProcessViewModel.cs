using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Dtos;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoEnums;
using IT.Tangdao.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        #region--字段--
        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        #endregion

        #region--属性--
        private ObservableCollection<ProcessItem> _processItems;

        public ObservableCollection<ProcessItem> ProcessItems
        {
            get => _processItems;
            set => SetAndNotify(ref _processItems, value);
        }

        #endregion

        #region--.ctor--

        public ProcessViewModel(IReadService readService, IWriteService writeService)
        {
            _readService = readService;
            _writeService = writeService;
            InitializalData();
        }

        #endregion

        #region--方法--

        public void InitializalData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessInfo.xml");
            var xmlData = _readService.Read(foldPath);

            if (xmlData == null)
            {
                ProcessItems = new ObservableCollection<ProcessItem>()
                {
                     new ProcessItem(){Name="生产模式",IsFeeding=false,IsBoardMade=false,IsBoardCheck=false,IsSafe=false,IsCharge=true}
                };
                return;
            }
            _readService.Current.Load(xmlData);
            var readResult = _readService.Current.SelectNodes<ProcessItem>();
            if (readResult.Status)
            {
                ProcessItems = new ObservableCollection<ProcessItem>(readResult.Result);
            }
        }

        public void RefreshProcessData()
        {
            MessageBox.Success("流程刷新成功");
        }

        public void SaveProcessData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessInfo.xml");
            //将数据写成XML格式保存在本地
            _writeService.WriteEntityToXml(ProcessItems, foldPath);
            MessageBox.Success("流程保存成功");
        }

        #endregion
    }
}