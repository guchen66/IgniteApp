using IgniteApp.Dialogs.Manage;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Tests
{
    /// <summary>
    /// 主要设计路由，或者注册视图
    /// </summary>
    //public interface ISingleRouter : INotifyPropertyChanged
    //{
    //    ISingleNavigateView CurrentView { get; }
    //    bool CanPrevious { get; }
    //    bool CanNext { get; }
    //    bool IsAutoRotating { get; set; }

    //    event EventHandler NavigationChanged;

    //    void Previous();

    //    void Next();

    //    void ToggleAutoCarousel();

    //    void NavigateTo(ISingleNavigateView view);

    //    void NavigateToIndex(int index);
    //}

    //public class SingleRouter : ISingleRouter, INotifyPropertyChanged
    //{
    //    private readonly BindableCollection<ISingleNavigateView> _views;
    //    private ISingleNavigateView _currentView;
    //    private int _currentIndex;
    //    private readonly DispatcherTimer _autoCarouselTimer;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public event EventHandler NavigationChanged;

    //    public ISingleNavigateView CurrentView
    //    {
    //        get => _currentView;
    //        private set
    //        {
    //            if (_currentView != value)
    //            {
    //                _currentView = value;
    //                OnPropertyChanged();
    //                OnNavigationChanged();
    //            }
    //        }
    //    }

    //    public bool CanPrevious => _currentIndex > 0;
    //    public bool CanNext => _currentIndex < _views.Count - 1;

    //    public bool IsAutoRotating
    //    {
    //        get => _autoCarouselTimer.IsEnabled;
    //        set
    //        {
    //            if (_autoCarouselTimer.IsEnabled != value)
    //            {
    //                if (value)
    //                    _autoCarouselTimer.Start();
    //                else
    //                    _autoCarouselTimer.Stop();

    //                OnPropertyChanged();
    //                OnPropertyChanged(nameof(AutoRotateStatusText));
    //            }
    //        }
    //    }

    //    public string AutoRotateStatusText => IsAutoRotating ? "自动轮播开启中" : "自动轮播已禁用";

    //    public IReadOnlyList<ISingleNavigateView> Views => _views;

    //    public SingleRouter(IEnumerable<ISingleNavigateView> views)
    //    {
    //        _views = new BindableCollection<ISingleNavigateView>(
    //            views.OrderBy(v => v.DisplayOrder).ToList());

    //        CurrentView = _views.FirstOrDefault();

    //        _autoCarouselTimer = new DispatcherTimer
    //        {
    //            Interval = TimeSpan.FromSeconds(3)
    //        };
    //        _autoCarouselTimer.Tick += OnAutoCarouselTick;
    //    }

    //    public void Previous()
    //    {
    //        if (!CanPrevious) return;

    //        CurrentView = _views[--_currentIndex];
    //        RefreshNavigationState();
    //    }

    //    public void Next()
    //    {
    //        if (!CanNext) return;

    //        CurrentView = _views[++_currentIndex];
    //        RefreshNavigationState();
    //    }

    //    public void ToggleAutoCarousel()
    //    {
    //        IsAutoRotating = !IsAutoRotating;
    //    }

    //    public void NavigateTo(ISingleNavigateView view)
    //    {
    //        var index = _views.IndexOf(view);
    //        if (index >= 0)
    //        {
    //            NavigateToIndex(index);
    //        }
    //    }

    //    public void NavigateToIndex(int index)
    //    {
    //        if (index >= 0 && index < _views.Count)
    //        {
    //            _currentIndex = index;
    //            CurrentView = _views[index];
    //            RefreshNavigationState();
    //        }
    //    }

    //    public void AddView(ISingleNavigateView view)
    //    {
    //        _views.Add(view);
    //        _views.OrderBy(v => v.DisplayOrder);
    //        RefreshNavigationState();
    //    }

    //    public void RemoveView(ISingleNavigateView view)
    //    {
    //        if (_views.Remove(view))
    //        {
    //            if (_currentView == view)
    //            {
    //                CurrentView = _views.FirstOrDefault();
    //                _currentIndex = 0;
    //            }
    //            RefreshNavigationState();
    //        }
    //    }

    //    private void OnAutoCarouselTick(object sender, EventArgs e)
    //    {
    //        // 循环导航
    //        _currentIndex = (_currentIndex + 1) % _views.Count;
    //        CurrentView = _views[_currentIndex];
    //        RefreshNavigationState();
    //    }

    //    private void RefreshNavigationState()
    //    {
    //        OnPropertyChanged(nameof(CanPrevious));
    //        OnPropertyChanged(nameof(CanNext));
    //    }

    //    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    protected void OnNavigationChanged()
    //    {
    //        NavigationChanged?.Invoke(this, EventArgs.Empty);
    //    }

    //    public void Dispose()
    //    {
    //        _autoCarouselTimer.Stop();
    //        _autoCarouselTimer.Tick -= OnAutoCarouselTick;
    //    }
    //}

    /// <summary>
    /// 导航控制器，用于应对各种IOC容器
    /// </summary>
    public interface INagateController
    {
    }

    /// <summary>
    /// 真正用于导航的服务
    /// </summary>
    public interface INagateService
    {
        object Container { get; set; }
    }

    /// <summary>
    /// 导航系统
    /// </summary>
    public class NagateSystem
    {
    }

    public class StyletNagateController : INagateController
    {
        public object Container;

        public StyletNagateController(StyletIoC.IContainer container)
        {
            Container = container;
        }
    }

    public static class ViewModelFirst
    {
        public static IEnumerable<ISingleNavigateView> InitSingleView(StyletIoC.IContainer Container)
        {
            var allViews = Container.GetAll<ISingleNavigateView>();
            return allViews.ToList();
        }

        public static ISingleNavigateView InitSingleViewByTangdao(ITangdaoProvider tangdaoProvider)
        {
            var s1 = tangdaoProvider.Resolve<ISingleNavigateView>();
            // var allViews = Container.GetAll<ISingleNavigateView>();
            return s1;
        }
    }

    public delegate object Factory(Type type);   // 一行就够
}