using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace IgniteApp.Extensions
{
    public static class BoolExtensions
    {
        public static string ToYAndN(this bool value)
        {
            return value ? "Y" : "N";
        }
    }

    public class CustomExtension : MarkupExtension
    {
        public Binding Binding { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (targetProvider?.TargetObject is FrameworkElement target)
            {
                // 等到运行时 DataContext 确定
                target.Loaded += (s, _) =>
                {
                    var vm = target.DataContext;   // 这里就能拿到 VM 实例
                                                   // TODO: 你自己的逻辑（但不要改 Text，否则破坏绑定）
                };
            }
            return Binding;   // 先让绑定通路跑起来
        }
    }
}