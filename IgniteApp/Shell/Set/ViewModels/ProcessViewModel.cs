using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System.Collections.Generic;
using System.IO;
using MessageBox = HandyControl.Controls.MessageBox;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        #region--字段--
        private readonly IContentAccess _contentAccess;
        #endregion

        #region--属性--
        private BindableCollection<ProcessItem> _processItems;

        public BindableCollection<ProcessItem> ProcessItems
        {
            get => _processItems;
            set => SetAndNotify(ref _processItems, value);
        }

        #endregion

        #region--.ctor--

        public ProcessViewModel(IContentAccess contentAccess)
        {
            _contentAccess = contentAccess;
            InitializalData();
        }

        #endregion

        #region--方法--

        public void InitializalData()
        {
            ProcessItems = GetLocalData();
        }

        public void RefreshProcessData()
        {
            ProcessItems = GetLocalData();
            ProcessItems.Refresh();
            MessageBox.Success("流程刷新成功");
        }

        public void SaveProcessData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessItem.xml");
            //将数据写成XML格式保存在本地
            var content = _contentAccess.Cache.Read(foldPath).Content;
            _contentAccess.Default.Write(foldPath, content, DaoFileType.Xml).AsXml().ToXml(ProcessItems);
            MessageBox.Success("流程保存成功");
        }

        private BindableCollection<ProcessItem> GetLocalData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessItem.xml");
            var result = _contentAccess.Default.Read(foldPath).AsXml().SelectNodes<ProcessItem>();
            if (result.IsSuccess)
            {
                ProcessItems = new BindableCollection<ProcessItem>(result.Data);
                return ProcessItems;
            }
            else
            {
                var faker = new TangdaoDataFaker<ProcessItem>();
                ProcessItems = new BindableCollection<ProcessItem>(faker.Build(20));
                return ProcessItems;
            }
        }

        #endregion
    }
}