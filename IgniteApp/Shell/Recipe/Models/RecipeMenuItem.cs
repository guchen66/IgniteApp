using IT.Tangdao.Framework.Mvvm;

namespace IgniteApp.Shell.Recipe.Models
{
    public class RecipeMenuItem : DaoViewModelBase//, IMenuItem
    {
        private string _recipeName;

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }

    }
}
