using IgniteApp.Bases;
using IgniteApp.Interfaces;
using IgniteApp.Shell.Home.Models;
using IgniteApp.Shell.Recipe.Models;
using IgniteApp.Shell.Set.ViewModels;
using IgniteApp.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Recipe.ViewModels
{
    public class RecipeViewModel : SectionViewModel
    {
        private BindableCollection<RecipeMenuItem> _recipeMenuList;

        public BindableCollection<RecipeMenuItem> RecipeMenuList
        {
            get => _recipeMenuList;
            set => SetAndNotify(ref _recipeMenuList, value);
        }
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        public IViewFactory _viewFactory;
        public RecipeViewModel(IViewFactory viewFactory)
        {
            this._viewFactory = viewFactory;
            RecipeMenuItem Item = new RecipeMenuItem();
            //字典转列表
            var lists = Item.ReadAppConfigToDic("RecipeMenu").Select(kvp => new RecipeMenuItem
            {
                MenuName = kvp.Value,
               
            }).ToList();
            RecipeMenuList = new BindableCollection<RecipeMenuItem>(lists);
            this.Bind(viewModel => viewModel.SelectedIndex, (obj, args) => DoNavigateToView());
        }

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
        public ProcessViewModel ProcessViewModel;
        public AxisArgsViewModel AxisArgsViewModel;
        public SystemSetViewModel SystemSetViewModel;

    }
}
