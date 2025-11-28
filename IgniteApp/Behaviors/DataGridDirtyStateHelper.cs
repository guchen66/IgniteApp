using System.Windows;
using System.Windows.Controls;

namespace IgniteApp.Behaviors
{
    public static class DataGridDirtyStateHelper
    {
        // 定义一个附加属性，用于将DataGrid实例传递给ViewModel
        public static readonly DependencyProperty DataGridInstanceProperty = 
            DependencyProperty.RegisterAttached(
                "DataGridInstance",
                typeof(DataGrid),
                typeof(DataGridDirtyStateHelper),
                new PropertyMetadata(null));

        public static void SetDataGridInstance(DependencyObject obj, DataGrid value)
        {
            obj.SetValue(DataGridInstanceProperty, value);
        }

        public static DataGrid GetDataGridInstance(DependencyObject obj)
        {
            return (DataGrid)obj.GetValue(DataGridInstanceProperty);
        }
    }
}