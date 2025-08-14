using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using IgniteAdmin.Managers.Login;
using IgniteAdmin.Managers.Transmit;
using IgniteApp.Bases;
using IgniteDb;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoDtos.Globals;
using IT.Tangdao.Framework.DaoEnums;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System.IO;
using IT.Tangdao.Framework.DaoCommands;
using IgniteApp.Views;
using IgniteApp.Interfaces;
using System.Windows.Navigation;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoEvents;
using System.Threading;
using IgniteApp.Extensions;
using IgniteShared.Extensions;
using System.Security.Principal;
using IgniteApp.Common;
using IgniteApp.Events;
using System.Windows.Shapes;
using Path = System.IO.Path;
using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;
using IgniteShared.Delegates;
using System.Windows.Markup;

namespace IgniteApp.ViewModels
{
    public class LoginViewModel : ViewModelBase, IHandle<CloseRegisterEvent>
    {
        #region--字段--

        // [Inject]
        private MainViewModel _mainViewModel;

        private INavigationService _navigationService;
        private readonly IWriteService _writeService;
        private readonly IReadService _readService;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(LoginViewModel));
        public ICommand RegisterCommand { get; set; }
        #endregion

        #region--属性--

        private LoginDto _loginDto;

        public LoginDto LoginDto
        {
            get => _loginDto ?? (_loginDto = new LoginDto());
            set => SetAndNotify(ref _loginDto, value);
        }

        #endregion

        #region--ctor--

        public LoginViewModel(IWriteService writeService, INavigationService navigationService, IEventAggregator eventAggregator, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _writeService = writeService;
            _readService = ServiceLocator.GetService<IReadService>();
            _windowManager = ServiceLocator.GetService<IWindowManager>();

            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            RegisterCommand = MinidaoCommand.Create(ExecuteRegister);
        }

        #endregion

        #region--方法--

        /// <summary>
        /// 登录
        /// </summary>
        public void ExecuteLogin()
        {
            //查找本地是否有登录过的账号
            var cacheData = UserManager.SearchCache(LoginDto);
            if (cacheData)
            {
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");
                _windowManager.ShowWindow(_mainViewModel);
                _writeService.WriteEntityToXml(LoginDto, foldPath);
                RequestClose();
            }
            else
            {
                MessageBox.Error("账号未注册");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void ExecuteCancel()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 记住密码
        /// </summary>
        public void ExecuteRememberPwd()
        {
            bool isAdmin = RoleSelectors.DetermineIfAdmin(LoginDto.UserName);
            var roleSelector = RoleSelectors.GetRoleSelector(isAdmin).Invoke(RoleType.管理员, RoleType.普通用户);
            LoginDto.Role = roleSelector;
            LoginDto.IsAdmin = isAdmin;
        }

        /// <summary>
        /// 初始化的时候检查本地是否有保存的密码
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            try
            {
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");
                var xmlData = _readService.Read(foldPath);
                if (xmlData == null)
                {
                    File.Create(foldPath);
                    return;
                }
                _readService.Current.Load(xmlData);
                var isRememberValue = _readService.Current.SelectNode("IsRemember").Value;// 获取元素的值
                // 将字符串转换为bool类型
                if (bool.TryParse(isRememberValue, out bool isRemember))
                {
                    if (isRemember)
                    {
                        LoginDto = XmlFolderHelper.Deserialize<LoginDto>(xmlData);
                    }
                    else
                    {
                        LoginDto = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLocal(ex.ToString());
            }
        }

        public void DragMove()
        {
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).DragMove();
        }

        private void ExecuteRegister()
        {
            _navigationService.NavigateToRegister();
            RequestClose();
        }

        public void Handle(CloseRegisterEvent closeRegister)
        {
            LoginDto.UserName = closeRegister.Name;
            LoginDto.Password = closeRegister.Pwd;
            //在这里关闭注册窗口
            // tangdaoParameter.ExecuteCommand<bool?>("Close", null);
        }

        #endregion
    }
}