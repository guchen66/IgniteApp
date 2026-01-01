using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace IgniteApp.Converters
{
    public class StringArrayTypeConverter : TypeConverter
    {
        private readonly char[] _chars = new[] { ',' };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                // 按逗号分割并去除空白
                return str.Split(_chars, StringSplitOptions.RemoveEmptyEntries)
                         .Select(item => item.Trim())
                         .ToArray();
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}