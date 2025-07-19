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
    public class FooterViewModel : ViewModelBase
    {
        #region--属性--

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

        #endregion

        #region--ctor--
        private IWindowManager _WindowManager;

        public FooterViewModel(IPlcProvider plcProvider, IWindowManager windowManager)
        {
            _plcProvider = plcProvider;
            _WindowManager = windowManager;

            QueryPlcStatus();
        }

        #endregion

        public void QueryPlcStatus()
        {
            Task.Run(() =>
            {
                IsConn = _plcProvider.ConnectionSiglePLC().IsSuccess;

                // IsConn = true;
            });
        }

        public void OpenEQP()
        {
        }

        [Inject]
        public TTForgeViewModel tTForgeViewModel { get; set; }

        public void OpenTTView()
        {
            _WindowManager.ShowWindow(tTForgeViewModel);
        }
    }
}