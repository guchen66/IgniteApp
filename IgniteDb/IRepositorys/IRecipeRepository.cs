using IgniteShared.Dtos;
using System.Collections.Generic;

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
