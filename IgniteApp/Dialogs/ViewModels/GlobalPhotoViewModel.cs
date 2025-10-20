using IgniteApp.Dialogs.Manage;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Attributes;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace IgniteApp.Dialogs.ViewModels
{
    public class GlobalPhotoViewModel : Screen
    {
        private readonly ISingleRouterDemo _router;

        public ISingleNavigateView CurrentView => _router.CurrentView;
        public bool CanPrevious => _router.CanPrevious;
        public bool CanNext => _router.CanNext;
        public bool IsAutoRotating => _router.IsAutoRotating;
        public string AutoRotateStatusText => _router.IsAutoRotating ? "自动轮播开启中" : "自动轮播已禁用";

        public GlobalPhotoViewModel(ISingleRouterDemo router)
        {
            _router = router;

            _router.PropertyChanged += OnRouterPropertyChanged;
            _router.NavigationChanged += OnRouterNavigationChanged;
        }

        public void Previous()
        {
            _router.Previous();
        }

        public void Next()
        {
            _router.Next();
        }

        public void ToggleAutoCarousel() => _router.ToggleAutoCarousel();

        private void OnRouterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 将路由器的属性变化转发到视图模型
            NotifyOfPropertyChange(e.PropertyName);

            if (e.PropertyName == nameof(ISingleRouter.IsAutoRotating))
            {
                NotifyOfPropertyChange(nameof(AutoRotateStatusText));
            }
        }

        private void OnRouterNavigationChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(nameof(CurrentView));
            NotifyOfPropertyChange(nameof(CanPrevious));
            NotifyOfPropertyChange(nameof(CanNext));
        }

        protected override void OnDeactivate()
        {
            _router.IsAutoRotating = false;
            base.OnDeactivate();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            // NavigationContext.CurrentGroup = "相机";
        }
    }

    public interface ISingleRouterDemo : INotifyPropertyChanged
    {
        ISingleNavigateView CurrentView { get; }

        bool CanPrevious { get; }

        bool CanNext { get; }

        bool IsAutoRotating { get; set; }

        string CurrentGroupKey { get; set; }

        event EventHandler NavigationChanged;

        event EventHandler<string> GroupChanged;

        void Previous();

        void Next();

        void ToggleAutoCarousel();

        void NavigateTo(ISingleNavigateView view);

        void NavigateToIndex(int index);
    }

    public sealed class SingleRouterDemo : ISingleRouterDemo, INotifyPropertyChanged
    {
        private readonly ObservableCollection<ISingleNavigateView> _views;
        private readonly IReadOnlyDictionary<string, IReadOnlyList<ISingleNavigateView>> _viewGroups;
        private CircularBuffer<ISingleNavigateView> _buffer;
        private ISingleNavigateView _currentView;
        private readonly DispatcherTimer _autoCarouselTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler NavigationChanged;

        public event EventHandler<string> GroupChanged;

        public ISingleNavigateView CurrentView
        {
            get => _currentView;
            private set
            {
                if (_currentView == value) return;
                _currentView = value;
                OnPropertyChanged();
                OnNavigationChanged();
            }
        }

        // 循环缓冲里永远有元素，因此导航能力一直可用（也可删掉这两个属性）
        private int _slot = 0;   // 当前页槽位（0 表示第一页）

        public bool CanPrevious => _slot > 0;
        public bool CanNext => _slot < _buffer.Count - 1;

        public bool IsAutoRotating
        {
            get => _autoCarouselTimer.IsEnabled;
            set
            {
                if (_autoCarouselTimer.IsEnabled == value) return;
                if (value) _autoCarouselTimer.Start();
                else _autoCarouselTimer.Stop();
                OnPropertyChanged();
                OnPropertyChanged(nameof(AutoRotateStatusText));
            }
        }

        public string AutoRotateStatusText => IsAutoRotating ? "自动轮播开启中" : "自动轮播已禁用";

        private string _currentGroupKey;

        public string CurrentGroupKey
        {
            get => _currentGroupKey;
            set
            {
                if (_currentGroupKey == value) return;
                _currentGroupKey = value;
                OnPropertyChanged();
                GroupChanged?.Invoke(this, value);
            }
        }

        public IReadOnlyList<ISingleNavigateView> Views => _views;

        public SingleRouterDemo(IEnumerable<ISingleNavigateView> views)
        {
            if (views == null) throw new ArgumentNullException(nameof(views));
            var viewList = views.ToList();
            if (viewList.Count == 0) throw new ArgumentException("必须提供至少一个视图实现", nameof(views));

            // 1. 分组
            _viewGroups = viewList
                .GroupBy(v =>
                {
                    var attr = v.GetType().GetCustomAttribute<SingleNavigateScanAttribute>();
                    return attr?.ViewScanName ?? "Default";
                })
                .ToDictionary(g => g.Key,
                              g => (IReadOnlyList<ISingleNavigateView>)g
                                      .OrderBy(v => v.DisplayOrder)
                                      .ToList());

            // 2. 默认组
            var firstKey = _viewGroups.Keys.First();
            CurrentGroupKey = firstKey;

            // 3. 建立循环缓冲
            _buffer = new CircularBuffer<ISingleNavigateView>(_viewGroups[firstKey]);
            _views = new ObservableCollection<ISingleNavigateView>(_buffer.Count == 0 ? Array.Empty<ISingleNavigateView>() : _viewGroups[firstKey]);

            // 4. 初始页
            CurrentView = _buffer.Next;

            // 5. 自动轮播
            _autoCarouselTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _autoCarouselTimer.Tick += (_, __) => CurrentView = _buffer.Next;
        }

        // 6. 导航接口——全部一行
        public void Next()
        {
            //if (!CanNext) return;

            CurrentView = _buffer.GetNext();
            _slot = (_slot + 1) % _buffer.Count;   // 循环但记录槽位
            RefreshNavigationState();
        }

        public void Previous()
        {
            // if (!CanPrevious) return;

            CurrentView = _buffer.GetPrevious();
            _slot = (_slot - 1 + _buffer.Count) % _buffer.Count;
            RefreshNavigationState();
        }

        public void ToggleAutoCarousel() => IsAutoRotating = !IsAutoRotating;

        public void NavigateTo(ISingleNavigateView view)
        {
            var idx = _views.IndexOf(view);
            if (idx >= 0) NavigateToIndex(idx);
        }

        public void NavigateToIndex(int index)
        {
            if (index < 0 || index >= _buffer.Count) return;
            // 把缓冲器指针一次性拨到指定位置（原子操作，线程安全）
            // 简单实现：连续 Next/Previous 到目标，或扩展 CircularBuffer 加 SetIndex
            // 这里用最快粗暴方式：连续 Next
            for (int i = 0; i < index; i++) _ = _buffer.Next;
            CurrentView = _buffer.Next;
        }

        public void AddView(ISingleNavigateView view)
        {
            // 实际项目里应同步更新 _viewGroups 并重建缓冲，Demo 简化为直接加
            _views.Add(view);
            // 重建缓冲
            var list = _views.OrderBy(v => v.DisplayOrder).ToList();
            _buffer = new CircularBuffer<ISingleNavigateView>(list);
            CurrentView = _buffer.Next;
        }

        public void RemoveView(ISingleNavigateView view)
        {
            if (!_views.Remove(view)) return;
            // 重建缓冲
            var list = _views.OrderBy(v => v.DisplayOrder).ToList();
            _buffer = new CircularBuffer<ISingleNavigateView>(list);
            CurrentView = list.Count == 0 ? null : _buffer.Next;
        }

        public void Dispose()
        {
            _autoCarouselTimer.Stop();
        }

        private void RefreshNavigationState()
        {
            OnPropertyChanged(nameof(CanPrevious));
            OnPropertyChanged(nameof(CanNext));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnNavigationChanged()
            => NavigationChanged?.Invoke(this, EventArgs.Empty);
    }
}