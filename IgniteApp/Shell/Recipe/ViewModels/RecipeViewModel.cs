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
    public class RecipeViewModel : NavigatViewModel
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

        /// <summary>
        /// TODO只有SelectedIndex才能正常跳转，其他类似SelectedValue不起作用应该是HC框架的bug
        /// 但是下标从0开始，所以为了和数据库Id=1对应，然后数据+1
        /// </summary>
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

        private RecipeDto _selectItem;

        public RecipeDto SelectItem
        {
            get => _selectItem;
            set => SetAndNotify(ref _selectItem, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
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
        }

        #endregion

        #region--方法--

        public void ExecuteAdd()
        {
            var result = _windowManager.ShowDialog(AddRecipeViewModel);
            AddRecipeViewModel.Parent = this;
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
            IsEnabled = result.HasValue;

            ///关闭子窗体的返回结果
            if (result.GetValueOrDefault(true))
            {
                //var dto = db.GetRecipeById(SelectedValue.ToInt());
                //this.RefreshData();
                IsEnabled = true;
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