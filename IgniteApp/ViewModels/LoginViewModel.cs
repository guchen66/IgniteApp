using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteApp.Events;
using IgniteApp.Interfaces;
using IgniteShared.Dtos;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Paths;
using IT.Tangdao.Framework.Threading;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using MessageBox = HandyControl.Controls.MessageBox;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace IgniteApp.ViewModels
{
    public class LoginViewModel : ViewModelBase, IHandle<CloseRegisterEvent>, ITangdaoMessage
    {
        #region--字段--

        // [Inject]
        private MainViewModel _mainViewModel;

        private INavigationService _navigationService;
        private readonly IContentAccess _contentAccess;
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

        private IActionTable _actionTable;

        public LoginViewModel(INavigationService navigationService, IEventAggregator eventAggregator, MainViewModel mainViewModel, IActionTable actionTable)
        {
            _mainViewModel = mainViewModel;
            _contentAccess = ServiceLocator.GetService<IContentAccess>();
            _windowManager = ServiceLocator.GetService<IWindowManager>();

            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            RegisterCommand = MinidaoCommand.Create(ExecuteRegister);
            var defaultFilePath = TangdaoPath.GetDateFilePath("E://Datas");
            _actionTable = actionTable;
            _actionTable.Executing += _actionTable_Executing;
        }

        private void _actionTable_Executing(object sender, IT.Tangdao.Framework.Events.ActionTableEventArgs e)
        {
        }

        #endregion

        #region--方法--

        /// <summary>
        /// 登录
        /// </summary>
        public void ExecuteLogin()
        {
            // 1、Cache目录有缓存，使用缓存登录，2、Cache无缓存，使用新的账号登录3、缓存的账号一定是我登录过的，4、UserInfo一定包含我登录的信息
            var cacheData = UserManager.SearchCache(LoginDto);
            UIAmbientContext.SetObject(LoginDto);          //线程上下文传输数据
            if (cacheData)
            {
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");
                _windowManager.ShowWindow(_mainViewModel);
                //_contentAccess.Default.WriteObject(foldPath, LoginDto);
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
        public async void ExecuteRememberPwd()
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
                var s1 = TangdaoPath.GetThisFilePath();
                var foldPath = Path.Combine(IgniteInfoLocation.Cache, "LoginInfo.xml");

                string isRememberValue = _contentAccess.Default.Read(foldPath).AsXml().SelectNode("IsRemember").Value;
                _ = isRememberValue.TryToBool(out var value);
                if (value)
                {
                    LoginDto = _contentAccess.Cache.DeserializeCache<LoginDto>(foldPath, DaoFileType.Xml);
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