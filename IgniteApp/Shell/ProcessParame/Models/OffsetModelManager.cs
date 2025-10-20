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

    public class MementoManager
    {
        public MementoType Memento { get; set; }
    }

    /// <summary>
    /// 备忘录接口
    /// </summary>
    public interface IMemento<T>
    {
        T State { get; }
        DateTime CreateTime { get; }
    }

    /// <summary>
    /// 具体的备忘录实现
    /// </summary>
    public class Memento<T> : IMemento<T>
    {
        public T State { get; }
        public DateTime CreateTime { get; }

        public Memento(T state)
        {
            State = state;
            CreateTime = DateTime.Now;
        }
    }

    public enum MementoType
    {
        Last, Current, Next,
    }

    /// <summary>
    /// 负责创建备忘录和从备忘录恢复状态
    /// </summary>
    public class Originator<T>
    {
        private T _state;

        public T State
        {
            get => _state;
            set
            {
                _state = value;
                Console.WriteLine($"State set to: {value}");
            }
        }

        /// <summary>
        /// 创建备忘录
        /// </summary>
        public IMemento<T> CreateMemento()
        {
            return new Memento<T>(_state);
        }

        /// <summary>
        /// 从备忘录恢复状态
        /// </summary>
        public void RestoreMemento(IMemento<T> memento)
        {
            _state = memento.State;
            Console.WriteLine($"State restored to: {_state} (from {memento.CreateTime})");
        }
    }

    /// <summary>
    /// 负责管理备忘录历史
    /// </summary>
    public class Caretaker<T>
    {
        private readonly Stack<IMemento<T>> _history = new Stack<IMemento<T>>();
        private readonly Originator<T> _originator;

        public Caretaker(Originator<T> originator)
        {
            _originator = originator;
        }

        /// <summary>
        /// 保存当前状态
        /// </summary>
        public void Save()
        {
            var memento = _originator.CreateMemento();
            _history.Push(memento);
            Console.WriteLine($"State saved. History count: {_history.Count}");
        }

        /// <summary>
        /// 撤销到上一个状态
        /// </summary>
        public void Undo()
        {
            if (_history.Count == 0)
            {
                Console.WriteLine("No history to undo");
                return;
            }

            var memento = _history.Pop();
            _originator.RestoreMemento(memento);
        }

        /// <summary>
        /// 获取历史记录数量
        /// </summary>
        public int HistoryCount => _history.Count;

        /// <summary>
        /// 清空历史记录
        /// </summary>
        public void ClearHistory()
        {
            _history.Clear();
            Console.WriteLine("History cleared");
        }
    }
}