using IgniteApp.Dialogs.Manage;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Abstractions.Navigates;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace IgniteApp.Dialogs.ViewModels
{
    public class GlobalPhotoViewModel : Screen
    {
        private readonly ISingleRouter _router;

        public ISingleNavigateView CurrentView => _router.CurrentView;
        public bool CanPrevious => _router.CanPrevious;
        public bool CanNext => _router.CanNext;
        public bool IsAutoRotating => _router.IsAutoRotating;
        public string AutoRotateStatusText => _router.IsAutoRotating ? "自动轮播开启中" : "自动轮播已禁用";

        public GlobalPhotoViewModel(ISingleRouter router)
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
            NavigationContext.CurrentGroup = "相机";
        }
    }
}