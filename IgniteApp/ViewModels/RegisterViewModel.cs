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
using IT.Tangdao.Framework.Events;
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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ScannerSubscribeAttribute : Attribute
    {
        public string Key { get; }

        public ScannerSubscribeAttribute(string key) => Key = key;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ScannerPublishAttribute : Attribute
    {
        public string Key { get; }

        public ScannerPublishAttribute(string key) => Key = key;
    }

    public static class ScannerBus
    {
        // key -> 所有订阅的方法委托
        private static readonly ConcurrentDictionary<string, List<Delegate>> _map = new ConcurrentDictionary<string, List<Delegate>>();

        /// <summary>
        /// 程序入口或 App.xaml.cs 里调用一次即可
        /// </summary>
        public static void Scan()
        {
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    from m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    let attr = m.GetCustomAttribute<ScannerSubscribeAttribute>()
                    where attr != null
                    select new { Method = m, Key = attr.Key, Type = t };

            foreach (var item in q)
            {
                var param = item.Method.GetParameters();
                var actionType = param.Length == 0
                    ? typeof(Action)
                    : System.Linq.Expressions.Expression.GetActionType(item.Method.GetParameters().Select(p => p.ParameterType).ToArray());

                var del = item.Method.IsStatic
                    ? Delegate.CreateDelegate(actionType, item.Method)
                    : Delegate.CreateDelegate(actionType, Activator.CreateInstance(item.Type), item.Method);

                _map.AddOrUpdate(item.Key, _ => new List<Delegate> { del }, (_, list) => { list.Add(del); return list; });
            }
        }

        /// <summary>
        /// 发布（在 [ScannerPublish] 的 AOP 里调用）
        /// </summary>
        public static void Publish(string key, params object[] args)
        {
            if (_map.TryGetValue(key, out var list))
            {
                foreach (var del in list.ToArray()) // ToArray 防止订阅中删订阅
                {
                    try { del.DynamicInvoke(args); }
                    catch (Exception ex) { /* 写日志 */ Debug.WriteLine(ex); }
                }
            }
        }
    }

    /// <summary>
    /// 给 MainViewModel 做一层代理，方法执行前检查特性并发布
    /// </summary>
    //public class PublisherProxy<T> : DispatchProxy where T : class
    //{
    //    private T _target;
    //    protected override object Invoke(MethodInfo targetMethod, object[] args)
    //    {
    //        var attr = targetMethod.GetCustomAttribute<ScannerPublishAttribute>();
    //        if (attr != null) ScannerBus.Publish(attr.Key, args);
    //        return targetMethod.Invoke(_target, args);
    //    }

    //    public static T Create(T target)
    //    {
    //        var proxy = Create<T, PublisherProxy<T>>();
    //        ((PublisherProxy<T>)proxy!)._target = target;
    //        return (T)proxy;
    //    }
    //}
}