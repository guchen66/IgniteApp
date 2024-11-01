using IgniteShared.Dtos;
using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.IRepositorys
{
    public interface IRecipeRepository
    {
        void AddRecipe(int id);

        RecipeDto AddRecipe(RecipeDto dto);

        RecipeDto GetRecipeById(int id);

        List<RecipeDto> GetRecipes();

        void EditRecipe(RecipeDto recipeDto);

        void DeleteRecipe(RecipeDto dto);
    }
}
