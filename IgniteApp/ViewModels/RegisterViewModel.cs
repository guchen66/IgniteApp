using HandyControl.Tools.Extension;
using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Events;
using IgniteApp.Extensions;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Maintion.Views;
using IgniteShared.Delegates;
using IgniteShared.Dtos;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.DaoEvents;
using IT.Tangdao.Framework.DaoMvvm;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using Stylet.Xaml;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IContainer = StyletIoC.IContainer;

namespace IgniteApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        private string _account;

        public string Account
        {
            get => _account;
            set => SetAndNotify(ref _account, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetAndNotify(ref _password, value);
        }

        public ICommand BackLoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public ITangdaoHandler _tangdaoHandler;
        private IEventAggregator _eventAggregator;

        public RegisterViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            BackLoginCommand = MinidaoCommand.Create(ExecuteBackLogin);
            CloseCommand = MinidaoCommand.Create<Window>(ExecuteClose);
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="win"></param>
        private void ExecuteClose(Window win)
        {
            win.Close();
        }

        /// <summary>
        /// 返回登录界面
        /// </summary>
        public void ExecuteBackLogin()
        {
            _navigationService.NavigateToLogin();
            RequestClose();
        }

        public void Confirm()
        {
            bool isAdmin = RoleSelectors.DetermineIfAdmin(Account);

            // 柯里化：动态生成 Role 选择器
            var roleSelector = RoleSelectors.GetRoleSelector(isAdmin).Invoke(RoleType.管理员, RoleType.普通用户);

            LoginDto loginDto = new LoginDto
            {
                UserName = Account,
                Password = Password,
                IsAdmin = isAdmin,
                Role = roleSelector,
                IP = IPHelper.GetLocalIPByLinq()
            };

            UserManager.SaveUserInfo(loginDto);
            _eventAggregator.Publish(new CloseRegisterEvent(loginDto.UserName, loginDto.Password));
            _navigationService.NavigateToLogin();
            RequestClose();
        }
    }

    public class TangdaoEventDispatcher
    {
        // 使用线程安全的集合（根据需要选择）
        private readonly List<ITangdaoHandler> _handlers = new List<ITangdaoHandler>();

        // 订阅事件处理器
        public void Subscribe(ITangdaoHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handlers.Add(handler);
        }

        // 取消订阅事件处理器
        public void Unsubscribe(ITangdaoHandler handler)
        {
            _handlers.Remove(handler);
        }

        // 触发事件，所有订阅者都会收到通知
        public void Dispatch(ITangdaoParameter parameter)
        {
            foreach (var handler in _handlers)
            {
                handler.Response();
            }
        }
    }

    public interface ITangdaoHandler
    {
        void Response();
    }

    public class TangdaoHandler : ITangdaoHandler
    {
        public void Response()
        {
            MaintainView maintainView = new MaintainView();
            maintainView.Show();
        }
    }
}