using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    /// <summary>
    /// 精度补偿数据管理类
    /// </summary>
    public class OffsetModelManager
    {
        public List<OffsetModel> CompensationData { get; }

        public OffsetModelManager(List<OffsetModel> data)
        {
            // 深拷贝防止外部修改
            CompensationData = data.Select(item => new OffsetModel()
            {
                Id = item.Id,
                StartValue = item.StartValue,
                EndValue = item.EndValue,
                CutType = item.CutType,
                IsXDirty = item.IsXDirty,
                IsYDirty = item.IsYDirty,
            }).ToList();
        }
    }

    public class RecipeManager
    {
        private List<OffsetModel> _currentCompensation = new List<OffsetModel>();

        // 当前补偿数据（绑定到DataGrid）
        public ObservableCollection<OffsetModel> CompensationData { get; set; }

        public RecipeManager()
        {
        }

        // 保存状态到备忘录
        public OffsetModelManager SaveToMemento()
        {
            return new OffsetModelManager(CompensationData.ToList());
        }

        // 从备忘录恢复
        public void RestoreFromMemento(OffsetModelManager memento)
        {
            CompensationData.Clear();
            foreach (var point in memento.CompensationData)
            {
                CompensationData.Add(point);
            }
        }

        // 保存配方（实际业务逻辑）
        public void SaveRecipe()
        {
            _currentCompensation = CompensationData.ToList();
            // 其他保存逻辑...
        }
    }

    /// <summary>
    /// 管理者类（负责备忘录生命周期）
    /// </summary>
    public class OffsetCaretaker
    {
        private Stack<OffsetModelManager> _history = new Stack<OffsetModelManager>();

        public void Backup(RecipeManager manager)
        {
            _history.Push(manager.SaveToMemento());
        }

        public void Undo(RecipeManager manager)
        {
            if (_history.Count > 0)
            {
                manager.RestoreFromMemento(_history.Pop());
            }
        }
    }
}