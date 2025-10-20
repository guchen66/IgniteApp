using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IgniteApp.Behaviors
{
    public class DataGridCurrentCellBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty CurrentCellProperty =
            DependencyProperty.Register(
                nameof(CurrentCell),
                typeof(DataGridCellInfo),
                typeof(DataGridCurrentCellBehavior),
                new FrameworkPropertyMetadata(default(DataGridCellInfo), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DataGridCellInfo CurrentCell
        {
            get => (DataGridCellInfo)GetValue(CurrentCellProperty);
            set => SetValue(CurrentCellProperty, value);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.CurrentCellChanged -= OnCurrentCellChanged;
            base.OnDetaching();
        }

        private void OnCurrentCellChanged(object sender, EventArgs e)
        {
            // 直接丢给VM，不管结构体引用是否相同
            CurrentCell = AssociatedObject.CurrentCell;
        }

        public static readonly DependencyProperty OwnerGridProperty =
       DependencyProperty.Register(
           nameof(OwnerGrid),
           typeof(DataGrid),
           typeof(DataGridCurrentCellBehavior));

        public DataGrid OwnerGrid
        {
            get => (DataGrid)GetValue(OwnerGridProperty);
            set => SetValue(OwnerGridProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            OwnerGrid = AssociatedObject;          // 把 grid 自己抛出去
            AssociatedObject.CurrentCellChanged += OnCurrentCellChanged;
        }
    }
}