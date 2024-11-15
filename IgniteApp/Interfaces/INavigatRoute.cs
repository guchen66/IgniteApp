using IgniteApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public interface INavigatRoute
    {
        string GetRouteName();
    }

    public class NavigatRoute: INavigatRoute
    {
        public string GetRouteName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            string[] viewModelNames = Array.Empty<string>(); 
            foreach (var item in types)
            {
                if (item.Namespace != null && item.Namespace.Contains("ViewModels"))
                {
                    Array.Resize(ref viewModelNames, viewModelNames.Length+1);
                    viewModelNames[viewModelNames.Length - 1] = item.Name;
                }
            }
            return viewModelNames.ToString();
        }

        public void GetName()
        {
            Dictionary<string, Type> dicts = new Dictionary<string, Type>();
            Type baseType = typeof(NavigatViewModel);
            var classType=Assembly.GetEntryAssembly().GetTypes().Where(type=>type.IsSubclassOf(baseType));
        }
    }
}
