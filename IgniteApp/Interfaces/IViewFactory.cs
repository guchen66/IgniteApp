﻿using IgniteApp.Shell.Home.ViewModels;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Shell.Recipe.ViewModels;
using IgniteApp.Shell.Set.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public interface IViewFactory
    {
        DefaultViewModel DefaultViewModel();
        SetViewModel SetViewModel();
        UserInfoViewModel UserInfoViewModel();
        MonitorViewModel MonitorViewModel();
        MaintainViewModel MaintionViewModel();
        RecipeViewModel RecipeViewModel();
        _404ViewModel _404ViewModel();

        ProcessViewModel ProcessViewModel();
        AxisArgsViewModel AxisArgsViewModel();
        SystemSetViewModel SystemSetViewModel();
    }

    public static class ViewFactoryExtension
    {
        public static IScreen CreateViewModel(this IViewFactory viewFactory,string title)
        {
            if (title == "首页")
            {
                return viewFactory.DefaultViewModel();
            }
            if (title== "用户信息")
            {
               return viewFactory.UserInfoViewModel();
            }
            else if (title == "监控")
            {
               return viewFactory.MonitorViewModel();
            }
            else if(title=="设置")
            {
               return viewFactory.SetViewModel();
            }
            else if (title == "维护")
            {
                return viewFactory.MaintionViewModel();
            }
            else if (title == "配方")
            {
                return viewFactory.RecipeViewModel();
            }

            return viewFactory._404ViewModel();
        }
    }
}
