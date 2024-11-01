using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
