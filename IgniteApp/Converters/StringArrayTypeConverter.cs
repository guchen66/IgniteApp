using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Converters
{
    public class StringArrayTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                // 按逗号分割并去除空白
                return str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(item => item.Trim())
                         .ToArray();
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}