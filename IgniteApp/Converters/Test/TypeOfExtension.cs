using IgniteApp.Views;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xaml;

namespace IgniteApp.Converters.Test
{
    public class TypeOfExtension : MarkupExtension
    {
        public string TypeName { get; set; }

        public override object ProvideValue(IServiceProvider sp)
        {
            //var UriContext = sp.GetService(typeof(IUriContext)) as IUriContext;
            //var BaseUri = UriContext.BaseUri;

            //string fileName = System.IO.Path.GetFileName(BaseUri.AbsolutePath); // "loginview.xaml"
            //string viewName = System.IO.Path.GetFileNameWithoutExtension(BaseUri.AbsolutePath); // "loginview"
            //string directory = System.IO.Path.GetDirectoryName(BaseUri.AbsolutePath); // "/views"
            //var rootProvider = sp.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            //string partName = PackUriHelper.GetPartUri(BaseUri).OriginalString;
            //var resolver = sp.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
            ////pack://application:,,,/IgniteApp;component/views/loginview.xaml

            // PackUriProvider.UriDict.TryAdd(viewName, BaseUri);
            return typeof(object);
        }
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
    }
}