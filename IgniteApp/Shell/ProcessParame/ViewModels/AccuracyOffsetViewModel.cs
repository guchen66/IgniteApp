using IgniteApp.Shell.ProcessParame.Models;
using IT.Tangdao.Framework.Mvvm;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class AccuracyOffsetViewModel : Screen
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        //IsChecked
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        private ObservableCollection<OffsetModel> _accuracyOffsetList;

        public ObservableCollection<OffsetModel> AccuracyOffsetList
        {
            get => _accuracyOffsetList;
            set => SetAndNotify(ref _accuracyOffsetList, value);
        }

        private DataGridCellInfo _currentCell;

        public DataGridCellInfo CurrentCell
        {
            get => _currentCell;
            set
            {
                if (SetAndNotify(ref _currentCell, value) && value.IsValid)
                {
                    // 行号
                    var row = AccuracyOffsetList.IndexOf(value.Item as OffsetModel);

                    // 列号
                    var column = value.Column.DisplayIndex;

                    // 列名
                    var header = value.Column.Header.ToString();
                    var val = GetCellDisplayText(value);

                    StatusText = $"第 {row + 1} 行，第 {column + 1} 列（{header}）的值：{val}";
                }
            }
        }

        public DataGrid OwnerGrid { get; set; }   // 由行为注入

        /// <summary>
        /// 统一拿“单元格显示文本”
        /// </summary>
        private string GetCellDisplayText(DataGridCellInfo cell)
        {
            if (!cell.IsValid || OwnerGrid == null) return string.Empty;

            // 1. 拿到行控件
            var row = OwnerGrid.ItemContainerGenerator.ContainerFromItem(cell.Item) as DataGridRow;
            if (row == null) return string.Empty;

            // 2. 让列自己把可视化内容吐出来
            var content = cell.Column.GetCellContent(row);   // 返回 FrameworkElement
            switch (cell.Column.Header.ToString())
            {
                case "End":
                    return (cell.Item as OffsetModel)?.EndValue.ToString() ?? string.Empty;

                case "Start":
                    return (cell.Item as OffsetModel)?.StartValue.ToString() ?? string.Empty;

                case "切割类型":
                    return (cell.Item as OffsetModel)?.CutType.ToString() ?? string.Empty;

                case "Id":
                    return (cell.Item as OffsetModel)?.Id.ToString() ?? string.Empty;

                default:
                    return string.Empty;
            }
            //switch (content)
            //{
            //    case (cell.Item as OffsetModel).EndValue: return;
            //    case TextBlock tb:
            //        return tb.Text;

            //    case TextBox tb:
            //        return tb.Text;

            //    case CheckBox cb:
            //        return cb.IsChecked?.ToString() ?? string.Empty;
            //    // 可以继续加 ComboBox、ToggleButton 等
            //    default:
            //        return string.Empty;
            //}
        }

        private string _statusText;
        public string StatusText { get => _statusText; set => SetAndNotify(ref _statusText, value); }

        private readonly RecipeManager _recipeManager = new RecipeManager();
        private readonly OffsetCaretaker _caretaker = new OffsetCaretaker();

        public AccuracyOffsetViewModel()
        {
            AccuracyOffsetList = new ObservableCollection<OffsetModel>()
            {
                new OffsetModel() {Id=1,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false, CutType="X1"},
                new OffsetModel() {Id=2,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X1"},
                new OffsetModel() {Id=3,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X1"},
                new OffsetModel() {Id=4,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X1"},
                new OffsetModel() {Id=5,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X2"},
                new OffsetModel() {Id=6,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="X2"},
                new OffsetModel() {Id=7,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X2"},
                new OffsetModel() {Id=8,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "X2"},
                new OffsetModel() {Id=9,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y1"},
                new OffsetModel() {Id=10,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y1"},
                new OffsetModel() {Id=11,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y1"},
                new OffsetModel() {Id=12,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y1"},
                new OffsetModel() {Id=13,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y2"},
                new OffsetModel() {Id=14,StartValue=1,EndValue=10,IsXDirty=false,IsYDirty=false,CutType="Y2"},
                new OffsetModel() {Id=15,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false,CutType = "Y2"},
                new OffsetModel() {Id=16,StartValue=1,EndValue=10, IsXDirty=false,IsYDirty=false, CutType = "Y2"},
            };
            listCaretaker = new Caretaker<List<OffsetModel>>(listOriginator);
            _recipeManager.CompensationData = AccuracyOffsetList;
        }

        private double _offset;

        public double Offset
        {
            get => _offset;
            set => SetAndNotify(ref _offset, value);
        }

        public void OneKeyAdd(string name)
        {
            // 1. 使用 ToLookup 分组
            var lookup = AccuracyOffsetList.ToLookup(x => x.CutType);

            // 2. 找到对应 CutType 的所有项
            var items = lookup[name];

            // 3. 遍历并修改 X1 的值（+10）
            foreach (var item in items)
            {
                item.StartValue += Offset;
            }
        }

        public void OneKeyDecrease(string name)
        { // 1. 使用 ToLookup 分组
            var lookup = AccuracyOffsetList.ToLookup(x => x.CutType);

            // 2. 找到对应 CutType 的所有项
            var items = lookup[name];

            // 3. 遍历并修改 X1 的值（+10）
            foreach (var item in items)
            {
                item.StartValue -= Offset;
            }
        }

        public void SaveData()
        {
            // _caretaker.Backup(_recipeManager);
            // _recipeManager.SaveRecipe();
            listOriginator.State = new List<OffsetModel>(listOriginator.State); // 创建新实例
            listOriginator.State.AddRange(AccuracyOffsetList);
            //
            listCaretaker.Save();
        }

        public void Undo()
        {
            listCaretaker.Undo(); // 回到 [1, 2, 3]
                                  // _caretaker.Undo(_recipeManager);
                                  // RaiseErrorsChanged(nameof(AccuracyOffsetList)); // 通知UI更新
        }

        public void DeleteData()
        {
            AccuracyOffsetList.Clear();
        }

        // 列表操作
        private Originator<List<OffsetModel>> listOriginator = new Originator<List<OffsetModel>>();

        private Caretaker<List<OffsetModel>> listCaretaker = null;
    }
}