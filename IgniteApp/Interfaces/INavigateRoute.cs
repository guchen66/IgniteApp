using System;

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