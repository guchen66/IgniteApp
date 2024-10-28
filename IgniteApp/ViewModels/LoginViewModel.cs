using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using StyletIoC;

namespace IgniteApp.ViewModels
{
    public class LoginViewModel : WindowViewModelBase
    {
        #region--字段--

        [Inject]
        private MainViewModel _mainViewModel;
        private readonly IWriteService _writeService;
        private readonly IReadService _readService;
        private readonly IWindowManager _windowManager;
        private readonly IContainer _container;
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(LoginViewModel));

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

        public LoginViewModel(IWriteService writeService, IContainer container)
        {
            _writeService = writeService;
            _readService = ServiceLocator.GetService<IReadService>();
            _windowManager = ServiceLocator.GetService<IWindowManager>();
            _container = container;
        }

        #endregion

        #region--方法--
        /// <summary>
        /// 登录
        /// </summary>
        public void ExecuteLogin() 
        {
            if (LoginDto.UserName=="Admin")
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
            UserManager.SaveXml(LoginDto);
            // 显示主窗口
            _windowManager.ShowWindow(_mainViewModel);
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
            if (LoginDto.UserName=="Admin")
            {
                LoginDto.IsAdmin = true;
            }
            var info = XmlFolderHelper.SerializeXML<LoginDto>(LoginDto);         
            _writeService.WriteString(LoginInfoLocation.LoginPath, info);
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

                if (xmlData==null)
                {
                    return;
                }
                var doc = XDocument.Parse(xmlData);
              //  var name=doc.Elements("LoginDto").Select(node=>node.Element("UserName").Value).ToList().FirstOrDefault();
                List<string> result = doc.Root.Elements().Select(node => node.Value).ToList();
                var isRememberValue = doc.Element("LoginDto").Element("IsRemember").Value; // 获取元素的值

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
        #endregion
    }
}
