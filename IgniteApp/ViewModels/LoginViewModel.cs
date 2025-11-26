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
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System.IO;
using IT.Tangdao.Framework.Commands;
using IgniteApp.Views;
using IgniteApp.Interfaces;
using System.Windows.Navigation;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Events;
using System.Threading;
using IgniteApp.Extensions;

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
using IT.Tangdao.Framework.DaoException;
using IT.Tangdao.Framework.Extensions;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using IT.Tangdao.Framework.Threading;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Paths;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Abstractions.Results;
using IgniteShared.Enums;
using IT.Tangdao.Framework.Markup;

namespace IgniteApp.ViewModels
{
    public class LoginViewModel : ViewModelBase, IHandle<CloseRegisterEvent>, ITangdaoMessage
    {
        #region--字段--

        // [Inject]
        private MainViewModel _mainViewModel;

        private INavigationService _navigationService;
        private readonly IContentWriter _writeService;
        private readonly IContentReader _readService;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(LoginViewModel));
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
        public IDaoEventAggregator _daoEventAggregator;

        public LoginViewModel(IContentWriter writeService, INavigationService navigationService, IEventAggregator eventAggregator, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _writeService = writeService;
            _readService = ServiceLocator.GetService<IContentReader>();
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
            //1、Cache目录有缓存，使用缓存登录，2、Cache无缓存，使用新的账号登录3、缓存的账号一定是我登录过的，4、UserInfo一定包含我登录的信息
            var cacheData = UserManager.SearchCache(LoginDto);
            AmbientContext.SetObject(LoginDto);          //线程上下文传输数据
            if (cacheData)
            {
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");
                _windowManager.ShowWindow(_mainViewModel);
                _writeService.Default.WriteObject(foldPath, LoginDto);
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
            //bool isAdmin = RoleSelectors.DetermineIfAdmin(LoginDto.UserName);
            //var roleSelector = RoleSelectors.GetRoleSelector(isAdmin).Invoke(RoleType.管理员, RoleType.普通用户);
            //LoginDto.Role = roleSelector;
            //LoginDto.IsAdmin = isAdmin;
        }

        /// <summary>
        /// 初始化的时候检查本地是否有保存的密码
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            try
            {
                var s1 = TangdaoPath.Instance.GetThisFilePath();
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");
                string isRememberValue = _readService.Default.Read(foldPath).AsXml().SelectNode("IsRemember").Value;
                _ = isRememberValue.TryToBool(out var value);
                if (value)
                {
                    LoginDto = _readService.Cache.DeserializeCache<LoginDto>(foldPath, DaoFileType.Xml);
                }
                else
                    LoginDto = null;
            }
            catch (Exception ex)
            {
                Logger.WriteLocal(ex.ToString());
            }
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

        public void DragMove()
        {
            Application.Current.Windows.OfType<Window>()
                       .SingleOrDefault(w => w.IsActive)?.DragMove();
        }

        public void Response(ITangdaoRequest request)
        {
        }

        #endregion
    }

    public class TestStudent
    {
        public int Id { get; set; }

        internal List<TestStudent> ConvertList()
        {
            List<TestStudent> testStudents = new List<TestStudent>();

            testStudents.Add(this);
            return testStudents;
        }

        public static Func<TSource, TTarget> CreateMap<TSource, TTarget>()
        {
            var p = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "instance");
            var body = System.Linq.Expressions.Expression.MemberInit(System.Linq.Expressions.Expression.New(typeof(TTarget)), typeof(TTarget).GetProperties().Select(d => System.Linq.Expressions.Expression.Bind(d, System.Linq.Expressions.Expression.Property(p, d.Name))));
            return System.Linq.Expressions.Expression.Lambda<Func<TSource, TTarget>>(body, p).Compile();
        }
    }
}