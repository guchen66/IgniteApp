using IgniteApp.Bases;
using IgniteApp.Shell.Maintion.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    /// <summary>
    /// 导航的数据上下文
    /// </summary>
    public class NavigatitionContext
    {
        public Uri Uri { get; set; }
        public INavigationService NavigationService { get; set; }

        public NavigatitionContext(INavigationService navigationService, Uri uri)
        {
            NavigationService = navigationService;
            Uri = uri;
        }
    }

    public class NavigateParameters
    {
    }

    public interface INavigateRoute
    {
        void NavigateTo(NavigatitionContext context);
    }

    public class NavigateRoute : INavigateRoute
    {
        public void NavigateTo(NavigatitionContext context)
        {
        }
    }

    public class HeaderAttribute : Attribute
    {
    }

    public class HeaderMethodAttribute : Attribute
    {
    }
}