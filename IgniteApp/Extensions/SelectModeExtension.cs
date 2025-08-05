using IgniteApp.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;

namespace IgniteApp.Extensions
{
    //  [TypeConverter(typeof(StringArrayTypeConverter))]
    public class SelectModeExtension : MarkupExtension
    {
        // 可配置的选项（可通过XAML属性自定义）
        private string _customItems;

        public string CustomItems
        {
            get => _customItems;
            set => _customItems = value;
        }

        // 默认选项
        private static readonly string[] DefaultItems = { "全部", "Load", "UpLoad" };

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // 返回自定义项或默认项
            return string.IsNullOrEmpty(CustomItems) ? DefaultItems : CustomItems.Split(',');
        }
    }
}