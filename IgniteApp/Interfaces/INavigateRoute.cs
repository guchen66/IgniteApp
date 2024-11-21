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
    public interface INavigateRoute
    {
        IScreen GetRoute<T>(T t);
        void GetRoute(int id,string screenName);
       // void GetName();
    }

    public class NavigateRoute: INavigateRoute
    {
        private readonly IViewFactory _viewFactory;
        public NavigateRoute()
        {
            _viewFactory=ServiceLocator.GetService<IViewFactory>();
        }
        public IScreen GetRoute<T>(T t)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            string[] viewModelNames = Array.Empty<string>(); 
            List<object> strings = new List<object>();
            foreach (var item in types)
            {
                if (item.Namespace != null && item.GetInterfaces().Contains(typeof(INavigateEntry)))
                {
                    var obj=Activator.CreateInstance(item) as LightViewModel;
                    //_viewFactory.ViewModel<LightViewModel>();
                   /* Array.Resize(ref viewModelNames, viewModelNames.Length+1);
                    viewModelNames[viewModelNames.Length - 1] = item.Name;
                    var propertyInfo = item.GetProperty("Name");
                    if (propertyInfo != null)
                    {
                        
                        var obj = propertyInfo.GetValue(t);
                        strings.Add(obj);
                    }*/
                }
            }
            // var ssss = strings;
            // return viewModelNames.ToString();
            return new Screen();
        }

        public void GetName()
        {
            Dictionary<string, Type> dicts = new Dictionary<string, Type>();
            Type baseType = typeof(INavigateRoute);
            var classTypes=Assembly.GetEntryAssembly().GetTypes().Where(type=>type.IsSubclassOf(baseType)).ToList();
            foreach (var classType in classTypes) 
            {
               var s= Activator.CreateInstance(classType) as NavigatViewModel;
              //  s.Name = classType.Name;
            }
           
        }
        private List<Type> HeaderTypes;
        private object Methods;
        public void Get()
        {
            HeaderTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(HeaderAttribute), false).Any()).ToList();
            List<string> lists=HeaderTypes.Select(t => t.Name).ToList();

        }

        public void GetRoute(int id, string screenName)
        {
            NavigatViewModel  viewModel = ServiceLocator.GetService<NavigatViewModel>();
            MethodInfo methodInfo = viewModel.GetType().GetMethod(screenName);
            Screen screen=(Screen)methodInfo.Invoke(viewModel, new object[] { });
            viewModel.ActivateItem(screen);
        }
    }

    public class HeaderAttribute : Attribute
    {

    }

    public class HeaderMethodAttribute : Attribute
    {

    }
}
