using IgniteApp.Shell.Calibration.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using IgniteApp.Shell.Maintion.ViewModels;
using IgniteApp.Shell.Monitor.ViewModels;
using IgniteApp.Shell.Recipe.ViewModels;
using IgniteApp.Shell.Set.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public interface IViewFactory
    {
        #region--主页--

        DefaultViewModel DefaultViewModel();
        #endregion

        #region--用户--

        UserInfoViewModel UserInfoViewModel();
        #endregion

        #region--设置--

        SetViewModel SetViewModel();
        ProcessViewModel ProcessViewModel();
        AxisArgsViewModel AxisArgsViewModel();
        SystemSetViewModel SystemSetViewModel();
        #endregion

        #region--维护--

        MaintainViewModel MaintionViewModel();
        ResistiveViewModel ResistiveViewModel();
        PressureViewModel PressureViewModel();
        LightViewModel LightViewModel();
        ElectViewModel ElectViewModel();
        TempAndHumViewModel TempAndHumViewModel();
        #endregion

        #region--监控--

        MonitorViewModel MonitorViewModel();
        IOMonViewModel IOMonViewModel();
        AxisMonViewModel AxisMonViewModel();
        PlcMonViewModel PlcMonViewModel();
        ReportViewModel ReportViewModel();
        #endregion

        #region--配方--

        RecipeViewModel RecipeViewModel();
        #endregion

        #region--标定--

        CalibrationViewModel CalibrationViewModel();
        LoadCalibrationViewModel LoadCalibrationViewModel();
        UnLoadCalibrationViewModel UnLoadCalibrationViewModel();
        #endregion
        _404ViewModel _404ViewModel();

       // TSource ViewModel<TSource>()where TSource:INavigateEntry;

    }

    public static class ViewFactoryExtension
    {
        [Obsolete("此方法弃用，改为CreateViewModel")]
        public static IScreen CreateViewModel(this IViewFactory viewFactory, string title,bool f=false)
        {
            if (title == "首页")
            {
                return viewFactory.DefaultViewModel();
            }
            if (title == "用户信息")
            {
                return viewFactory.UserInfoViewModel();
            }
            else if (title == "监控")
            {
                return viewFactory.MonitorViewModel();
            }
            else if (title == "设置")
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
            else if (title == "标定")
            {
                return viewFactory.CalibrationViewModel();
            }
            return viewFactory._404ViewModel();
        }

        public static IScreen CreateViewModel(this IViewFactory viewFactory, string viewModelName)
        {
            var methodName = $"{viewModelName}";
            var methodInfo = typeof(IViewFactory).GetMethod(methodName);

            if (methodInfo != null)
            {
                return (IScreen)methodInfo.Invoke(viewFactory, null);
            }

            return viewFactory._404ViewModel();
        }

        public static IScreen CreateViewModelDelegateInvoke(this IViewFactory viewFactory,string viewModelName)
        {
            MethodInfo methodInfo = typeof(IViewFactory).GetMethod(viewModelName);
            Func<IViewFactory, object> func = MagicMethod<IViewFactory>(methodInfo);
            return (IScreen)func.Invoke(viewFactory);
        }

        public static Func<T,object> MagicMethod<T>(MethodInfo methodInfo) where T : class
        {
            MethodInfo genericHelper = typeof(ViewFactoryExtension).GetMethod("MagicMethodHelper",BindingFlags.Static|BindingFlags.Public);
            MethodInfo constructHelper = genericHelper.MakeGenericMethod(typeof(T),  methodInfo.ReturnType);

            object ret = constructHelper.Invoke(null, new object[] { methodInfo });
            return (Func<T, object>)ret;
        }

        public static Func<TTarget, object> MagicMethodHelper<TTarget, TReturn>(MethodInfo methodInfo) where TTarget : class
        {
            //创建方法转为委托
            Func<TTarget, TReturn> func = (Func<TTarget,TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), methodInfo);

            //创建更弱的委托调用上面的委托
            Func<TTarget,object> ret=(TTarget target)=>func(target);
            return ret;
        }
    }
}
