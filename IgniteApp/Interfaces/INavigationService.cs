using IgniteApp.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public interface INavigationService
    {
        void NavigateToLogin();

        void NavigateToRegister();
    }

    public class NavigationService : INavigationService
    {
        private readonly IWindowManager _windowManager;

        public NavigationService(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void NavigateToLogin()
        {
            var _loginViewModel = ServiceLocator.GetService<LoginViewModel>();
            _windowManager.ShowWindow(_loginViewModel);
        }

        public void NavigateToRegister()
        {
            var _registerViewModel = ServiceLocator.GetService<RegisterViewModel>();
            _windowManager.ShowWindow(_registerViewModel);
        }
    }
}