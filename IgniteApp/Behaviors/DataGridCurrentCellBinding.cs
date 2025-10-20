using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace IgniteApp.Behaviors
{
    public static class DataGridCurrentCellBinding
    {
        public static readonly DependencyProperty CurrentCellProperty =
            DependencyProperty.RegisterAttached(
                "CurrentCell",
                typeof(DataGridCellInfo),
                typeof(DataGridCurrentCellBinding),
                new PropertyMetadata(default(DataGridCellInfo), OnCurrentCellChanged));

        public static DataGridCellInfo GetCurrentCell(DependencyObject obj)
            => (DataGridCellInfo)obj.GetValue(CurrentCellProperty);

        public static void SetCurrentCell(DependencyObject obj, DataGridCellInfo value)
            => obj.SetValue(CurrentCellProperty, value);

        /* 关键：挂/卸事件钩子 */

        private static void OnCurrentCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is DataGrid dg))
            {
                if (e.OldValue == default && e.NewValue != default)
                {
                    // 第一次绑定时，挂事件
                    dg.CurrentCellChanged += OnGridCurrentCellChanged;
                    dg.Unloaded += OnGridUnloaded;
                }
                else if (e.NewValue == default && e.OldValue != default)
                {
                    // 解除绑定
                    dg.CurrentCellChanged -= OnGridCurrentCellChanged;
                    dg.Unloaded -= OnGridUnloaded;
                }

                // VM→Grid 方向
                if (!dg.CurrentCell.Equals(e.NewValue))
                    dg.CurrentCell = (DataGridCellInfo)e.NewValue;
            }
            else
            {
                return;
            }
        }

        /* Grid→VM 方向 */

        private static void OnGridCurrentCellChanged(object sender, EventArgs e)
        {
            var dg = (DataGrid)sender;
            SetCurrentCell(dg, dg.CurrentCell);
        }

        private static void OnGridUnloaded(object sender, RoutedEventArgs e)
        {
            var dg = (DataGrid)sender;
            dg.CurrentCellChanged -= OnGridCurrentCellChanged;
            dg.Unloaded -= OnGridUnloaded;
        }
    }
}