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

namespace IgniteApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region--字段--

        [Inject]
        private MainViewModel _mainViewModel;

        private INavigationService _navigationService;
        private readonly IWriteService _writeService;
        private readonly IReadService _readService;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigateService _navigateService;
        private readonly IContainer _container;
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

        public LoginViewModel(IWriteService writeService, IContainer container, INavigationService navigationService, IEventAggregator eventAggregator, INavigateService navigateService)
        {
            _writeService = writeService;
            _readService = ServiceLocator.GetService<IReadService>();
            _windowManager = ServiceLocator.GetService<IWindowManager>();
            _container = container;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            RegisterCommand = MinidaoCommand.Create(ExecuteRegister);
            _navigateService = navigateService;
        }

        #endregion

        #region--方法--

        /// <summary>
        /// 登录
        /// </summary>
        public void ExecuteLogin()
        {
            if (LoginDto.UserName == "Admin")
            {
                ExecuteAdminLogin();
            }
            else
            {
                ExecuteCommonLogin();
            }
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        public void ExecuteAdminLogin()
        {
            LoginDto.IsAdmin = true;
            LoginDto.IP = IPHelper.GetLocalIPByLinq();
            SysLoginInfo.Role = LoginDto.Role = RoleType.管理员;
            SysLoginInfo.UserName = LoginDto.UserName;
            SysLoginInfo.Password = LoginDto.Password;
            UserManager.SaveXml(LoginDto);
            // 显示主窗口
            _windowManager.ShowWindow(_mainViewModel);
            //关闭登录窗口
            RequestClose();
        }

        /// <summary>
        /// 普通用户登录
        /// </summary>
        public void ExecuteCommonLogin()
        {
            LoginDto.IsAdmin = false;
            LoginDto.IP = IPHelper.GetLocalIPByLinq();
            SysLoginInfo.Role = LoginDto.Role = RoleType.普通用户;
            SysLoginInfo.UserName = LoginDto.UserName;
            SysLoginInfo.Password = LoginDto.Password;
            UserManager.SaveXml(LoginDto);
            // 显示主窗口
            _windowManager.ShowWindow(_mainViewModel);
            RequestClose();
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
            if (LoginDto.UserName == "Admin")
            {
                LoginDto.IsAdmin = true;
            }
            var info = XmlFolderHelper.SerializeXML<LoginDto>(LoginDto);
        }

        /// <summary>
        /// 初始化的时候检查本地是否有保存的密码
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
                _readService.Load(xmlData);

                var isRememberValue = _readService.Current.SelectNode("IsRemember").Value;// 获取元素的值
                //  var name=doc.Elements("LoginDto").Select(node=>node.Element("UserName").Value).ToList().FirstOrDefault();
                //  List<string> result = doc.Root.Elements().Select(node => node.Value).ToList();

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
                MessageBox.Show(ex.Message);
            }
        }

        public void DragMove()
        {
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).DragMove();
        }

        [Inject]
        private RegisterViewModel _registerViewModel;

        private void ExecuteRegister()
        {
            //RegisterViewModel创建的迟了，所以这里我必须先进行RegisterViewModel的构造，然后进行数据的发送
            //我使用我自己写的事件聚合器和Stylet自带的都测试一遍，效果相同
            ITangdaoParameter parameter = new TangdaoParameter();
            parameter.Add("userName", LoginDto.UserName);
            parameter.Add("password", LoginDto.Password);
            parameter.AddCommand<bool?>("close", RequestClose);
            _navigationService.NavigateToRegister();
            _eventAggregator.Publish(parameter);

            //DialogParameters dialogParameters = new DialogParameters();
            //dialogParameters.Add("userName", LoginDto.UserName);
            //dialogParameters.Add("password", LoginDto.Password);

            //var dialog = _windowManager.ShowDialogEx(_registerViewModel, dialogParameters);
            //if (dialog.Result.HasValue)
            //{
            //    RequestClose();
            //}
            //
            //
        }

        #endregion
    }
}