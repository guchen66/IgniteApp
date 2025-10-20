using HandyControl.Controls;
using IgniteAdmin.Providers;
using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Set.Models;
using IgniteShared.Dtos;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using MessageBox = HandyControl.Controls.MessageBox;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        #region--字段--
        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        #endregion

        #region--属性--
        private BindableCollection<ProcessItem> _processItems;

        public BindableCollection<ProcessItem> ProcessItems
        {
            get => _processItems;
            set => SetAndNotify(ref _processItems, value);
        }

        private IReadProvider<ProcessItem> _readProvider;
        #endregion

        #region--.ctor--

        public ProcessViewModel(IReadService readService, IWriteService writeService, IReadProvider<ProcessItem> readProvider)
        {
            _readService = readService;
            _writeService = writeService;
            _readProvider = readProvider;
            InitializalData();
        }

        #endregion

        #region--方法--

        public void InitializalData()
        {
            var operations = _readProvider.SelectList(IgniteInfoLocation.Recipe);
            if (operations.IsSuccess)
            {
                ProcessItems = new BindableCollection<ProcessItem>(operations.Data);
            }
            //NotifyOfPropertyChange(nameof(ProcessItems));
        }

        public void RefreshProcessData()
        {
            // InitializalData();
            var operations = _readProvider.SelectList(IgniteInfoLocation.Recipe);
            if (operations.IsSuccess)
            {
                ProcessItems.Clear();
                foreach (var item in operations.Data)
                    ProcessItems.Add(item);
            }
            MessageBox.Success("流程刷新成功");
            ProcessItems.Refresh();
            // MessageBox.Success("流程刷新成功");
            //Application.Current.Dispatcher.Invoke(() => { });
            //Dispatcher.CurrentDispatcher.Invoke(() => { });

            //Dispatcher.CurrentDispatcher.InvokeAsync(() => { });
            //Dispatcher.CurrentDispatcher.BeginInvoke();
            //Dispatcher dispatcher;
            //dispatcher = dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void SaveProcessData()
        {
            var foldPath = Path.Combine(IgniteInfoLocation.Recipe, "ProcessItem.xml");
            //将数据写成XML格式保存在本地
            _writeService.WriteEntityToXml(ProcessItems, foldPath);
            MessageBox.Success("流程保存成功");
        }

        #endregion
    }
}