using IgniteApp.Dialogs.Manage;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace IgniteApp.Dialogs.ViewModels
{
    public class GlobalPhotoViewModel : Screen//, IHaveDisplayName
    {
        private readonly BindableCollection<IPhotoView> _photoViews = new BindableCollection<IPhotoView>();
        private IPhotoView _currentView;
        private int _currentIndex;
        private readonly DispatcherTimer _autoCarouselTimer;

        public IReadOnlyList<IPhotoView> PhotoViews => _photoViews;

        public IPhotoView CurrentView
        {
            get => _currentView;
            private set => SetAndNotify(ref _currentView, value);
        }

        public bool CanPrevious => _currentIndex > 0;
        public bool CanNext => _currentIndex < _photoViews.Count - 1;

        public bool IsAutoRotating
        {
            get => _autoCarouselTimer.IsEnabled;
            set
            {
                if (value) _autoCarouselTimer.Start();
                else _autoCarouselTimer.Stop();
                NotifyOfPropertyChange();
            }
        }

        public string AutoRotateStatusText => IsAutoRotating ? "自动轮播开启中" : "自动轮播已禁用";

        public GlobalPhotoViewModel(
            LoadPrePhotoViewModel loadPreVm,
            TakePhotoViewModel takePhotoVm,
            CheckProcessPhotoViewModel checkProcessVm)
        {
            // 初始化视图集合（按显示顺序排序）
            _photoViews.AddRange(new IPhotoView[]
            {
            loadPreVm,
            takePhotoVm,
            checkProcessVm
            }.OrderBy(v => v.DisplayOrder));

            CurrentView = _photoViews.FirstOrDefault();

            // 配置自动轮播定时器（可选功能）
            _autoCarouselTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _autoCarouselTimer.Tick += (s, e) => AutoRotateNext();
        }

        public void Previous()
        {
            if (!CanPrevious) return;

            CurrentView = _photoViews[--_currentIndex];
            RefreshNavigationState();
        }

        public void Next()
        {
            if (!CanNext) return;

            CurrentView = _photoViews[++_currentIndex];
            RefreshNavigationState();
        }

        public void ToggleAutoCarousel()
        {
            IsAutoRotating = !IsAutoRotating;
            NotifyOfPropertyChange(nameof(IsAutoRotating));
            NotifyOfPropertyChange(nameof(AutoRotateStatusText));
        }

        private void RefreshNavigationState()
        {
            NotifyOfPropertyChange(() => CanPrevious);
            NotifyOfPropertyChange(() => CanNext);
        }

        private void AutoRotateNext()
        {
            // 计算下一个索引（循环模式）
            _currentIndex = (_currentIndex + 1) % _photoViews.Count;
            CurrentView = _photoViews[_currentIndex];
            RefreshNavigationState();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            var win = View;
            DisplayName = "全局图片显示";
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            var win = View;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            var win = View;
        }

        protected override void OnDeactivate()
        {
            _autoCarouselTimer.Stop(); // 确保窗口关闭时停止定时器
            base.OnDeactivate();
        }
    }
}