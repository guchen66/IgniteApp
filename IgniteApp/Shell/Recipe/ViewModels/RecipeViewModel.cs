using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Dialogs.ViewModels;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Home.ViewModels;
using IgniteApp.Shell.Recipe.Models;
using IgniteApp.Shell.Set.ViewModels;
using IgniteApp.ViewModels;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Recipe.ViewModels
{
    public class RecipeViewModel : SectionViewModel
    {
        #region--字段--

        private readonly IRecipeRepository db;
        public IViewFactory _viewFactory;
        public IWindowManager _windowManager;
        #endregion

        #region--属性--
        private ObservableCollection<RecipeDto> _recipeMenuList;

        public ObservableCollection<RecipeDto> RecipeMenuList
        {
            get => _recipeMenuList ?? (_recipeMenuList = new ObservableCollection<RecipeDto>());
            set => SetAndNotify(ref _recipeMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }
        private string _selectedValue;

        public string SelectedValue
        {
            get => _selectedValue;
            set => SetAndNotify(ref _selectedValue, value);
        }

        #endregion

        #region--.ctor--
        public RecipeViewModel(IRecipeRepository db, IViewFactory viewFactory, IWindowManager windowManager)
        {
            this.db = db;
            _viewFactory = viewFactory;
            _windowManager = windowManager;
            //字典转列表
            var lists = db.GetRecipes().ToObservableCollection();
            RecipeMenuList = lists;
           // this.Bind(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());

        }
        #endregion

        #region--方法--
        private void DoNavigateToView()
        {
            switch (SelectedIndex)
            {
                case 0: ActivateItem(ProcessViewModel ?? (ProcessViewModel = _viewFactory.ProcessViewModel())); break;
                case 1: ActivateItem(AxisArgsViewModel ?? (AxisArgsViewModel = _viewFactory.AxisArgsViewModel())); break;
                case 2: ActivateItem(SystemSetViewModel ?? (SystemSetViewModel = _viewFactory.SystemSetViewModel())); break;
                default:
                    break;
            }
        }

        public void ExecuteAdd()
        {
            var result = _windowManager.ShowDialog(AddRecipeViewModel);
            ///关闭子窗体的返回结果
            if (result.GetValueOrDefault(true))
            {
                this.RefreshData();
            }
            else
            {

            }
        }
        public void ExecuteEdit()
        {
            var result = _windowManager.ShowDialog(SafePopupViewModel);
         //   IsEnabled = result.HasValue;
            ///关闭子窗体的返回结果
            if (result.GetValueOrDefault(true))
            {
                var dto = db.GetRecipeById(SelectedValue.ToInt());
                this.RefreshData();
            }
            else
            {
                MessageBox.Error("密码输入错误");
            }
          //  var dto = db.GetRecipeById(SelectedValue.ToInt());
           // dto.RecipeName = "配方4";
           // db.EditRecipe(dto);
           // this.RefreshData();
        }

        public void ExecuteDelete()
        {
            var dto = db.GetRecipeById(SelectedIndex);
            db.DeleteRecipe(dto);
            this.RefreshData();
        }
        public void RefreshData()
        {
            RecipeMenuList = new ObservableCollection<RecipeDto>();
            db.GetRecipes().ForEach(x => RecipeMenuList.Add(x));
        }

        public void ExecuteApply()
        {
            //TODO 保存配方并发送给PLC
        }

        public ProcessViewModel ProcessViewModel;
        public AxisArgsViewModel AxisArgsViewModel;
        public SystemSetViewModel SystemSetViewModel;

        [Inject]
        public AddRecipeViewModel AddRecipeViewModel;
        [Inject]
        public SafePopupViewModel SafePopupViewModel;
        #endregion

    }
}
