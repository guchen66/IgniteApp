using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IgniteApp.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path, UriKind.Relative)) { CacheOption = BitmapCacheOption.OnLoad };
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public static class ImageConverter2
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr intPtr);

        public static ImageSource BitmapToImageSourceConvert(Bitmap bitmap) 
        {
            IntPtr intPtr=bitmap.GetHbitmap();
            ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            //释放内存
            if (!DeleteObject(intPtr))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            return imageSource;
        }
    }
}
