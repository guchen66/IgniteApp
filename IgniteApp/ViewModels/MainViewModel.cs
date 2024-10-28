using IgniteApp.Bases;
using IgniteApp.Common;
using IgniteApp.Shell.Aside.ViewModels;
using IgniteApp.Shell.Footer.ViewModels;
using IgniteApp.Shell.Header.ViewModels;
using IgniteApp.Shell.Home.ViewModels;
using Stylet;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.ViewModels
{
    public class MainViewModel: WindowViewModelBase// Conductor<Screen>.Collection.OneActive
    {
        #region--属性--

        [Inject]
		public HeaderViewModel HeaderViewModel { get; set; }
        [Inject]
        public AsideViewModel AsideViewModel { get; set; }
        [Inject]
        public HomeViewModel HomeViewModel { get; set; }
        [Inject]
        public FooterViewModel FooterViewModel { get; set; }

        #endregion

        #region--ctor--
        public MainViewModel()
        {
            //  ActivateItem(HeaderViewModel);
            InitException();
        }

        #endregion

        #region--方法--
        private void InitException()
        {



        }

        public void ShowLoginScreen()
        {
            // ActivateItem(new LoginViewModel(this));
        }

        public void ShowMainScreen()
        {
            // ActivateItem(new MainViewModel());
            // DeactivateItem(this.ActiveItem); // 关闭当前激活的ViewModel
        }
        #endregion

        #region--view--
        protected override void OnActivate()
        {
            base.OnActivate();
        }
        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

        }

        #endregion
          
    }
}
