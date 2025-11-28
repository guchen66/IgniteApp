using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace IgniteApp.Behaviors
{
    public static class CellDirtyStateBehavior
    {
        // 用于存储每个单元格的原始值
        private static readonly Dictionary<DependencyObject, object> OriginalValues = new Dictionary<DependencyObject, object>();

        // 定义附加属性来存储原始值
        public static readonly DependencyProperty OriginalValueProperty =
            DependencyProperty.RegisterAttached(
                "OriginalValue",
                typeof(object),
                typeof(CellDirtyStateBehavior),
                new PropertyMetadata(null, OnOriginalValueChanged));

        public static void SetOriginalValue(DependencyObject obj, object value)
        {
            obj.SetValue(OriginalValueProperty, value);
        }

        public static object GetOriginalValue(DependencyObject obj)
        {
            return obj.GetValue(OriginalValueProperty);
        }

        // 当OriginalValue属性设置时，存储原始值
        private static void OnOriginalValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                // 存储原始值
                OriginalValues[d] = e.NewValue;

                // 监听Text属性变化
                DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock))
                    .AddValueChanged(textBlock, OnTextChanged);
            }
        }

        // 当Text属性变化时，检查是否与原始值不同
        private static void OnTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                // 获取当前值和原始值
                string currentValue = textBlock.Text;
                object originalValue = OriginalValues.TryGetValue(textBlock, out var value) ? value : null;
                
                // 比较当前值和原始值
                bool isDirty = !Equals(currentValue, originalValue?.ToString());
                
                // 更新前景色 - 只有当值变化时才变为红色，不会自动恢复
                if (isDirty)
                {
                    textBlock.Foreground = Brushes.Red;
                }
            }
        }

        // 重置所有单元格的修改状态
        public static void ResetDirtyState(DataGrid dataGrid)
        {
            // 遍历所有可见行
            foreach (var item in dataGrid.Items)
            {
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    // 遍历所有单元格
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        var cell = dataGrid.Columns[i].GetCellContent(row)?.Parent as DataGridCell;
                        if (cell != null)
                        {
                            // 查找TextBlock
                            if (cell.Content is TextBlock textBlock)
                            {
                                // 重置前景色
                                textBlock.Foreground = SystemColors.WindowTextBrush;

                                // 更新原始值为当前值
                                if (OriginalValues.ContainsKey(textBlock))
                                {
                                    OriginalValues[textBlock] = textBlock.Text;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}