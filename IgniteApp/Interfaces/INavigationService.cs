using IgniteApp.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StyletIoC;

namespace IgniteApp.Interfaces
{
    public interface INavigationService<T>
    {
        T Current { get; }

        void NavigateToLogin();

        void NavigateToRegister();
    }

    public interface INavigationService
    {
        void NavigateToLogin();

        void NavigateToRegister();
    }

    public class NavigationService<T> : INavigationService<T>, INavigationService
    {
        public T Current { get; }
        private readonly IWindowManager _windowManager;
        private readonly IContainer _container;

        public NavigationService(IWindowManager windowManager, IContainer container)
        {
            _windowManager = windowManager;
            _container = container;
        }

        public void NavigateToLogin()
        {
            var _loginViewModel = _container.Get<LoginViewModel>();
            _windowManager.ShowWindow(_loginViewModel);
        }

        public void NavigateToRegister()
        {
            var _registerViewModel = _container.Get<RegisterViewModel>();
            _windowManager.ShowWindow(_registerViewModel);
        }
    }
}