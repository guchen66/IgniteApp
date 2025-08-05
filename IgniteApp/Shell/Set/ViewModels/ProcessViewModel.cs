using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoEnums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var xmlData = _readService.Current.XMLData = _readService.Read(IgniteInfoLocation.ProcessInfoPath);

            if (xmlData == null)
            {
                return;
            }
            _readService.Current.Load();

            var readResult = _readService.Current.SelectNodes("ProcessItem", x => new ProcessItem
            {
                Name = x.Element("Name")?.Value,
                IsFeeding = (bool)x.Element("IsFeeding"),
                IsBoardMade = (bool)x.Element("IsBoardMade"),
                IsBoardCheck = (bool)x.Element("IsBoardCheck"),
                IsSeal = (bool)x.Element("IsSeal"),
                IsSafe = (bool)x.Element("IsSafe"),
                IsCharge = (bool)x.Element("IsCharge"),
                IsBlanking = (bool)x.Element("IsBlanking"),
            });

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
            //将数据写成XML格式保存在本地
            _writeService.WriteEntityToXml(ProcessItems, IgniteInfoLocation.ProcessInfoPath, DaoFileType.Xml);
            MessageBox.Success("流程保存成功");
        }

        #endregion
    }
}