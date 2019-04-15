using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe.Monolith.Models
{
    public class RecipeViewModel
    {
        public static async Task<RecipeViewModel> GetRecipeViewModelById(long id)
        {
            Recipe recipe = RecipeManager.GetRecipeById(id);
            RecipeViewModel viewModel = new RecipeViewModel(recipe);

            return viewModel;
        }

        public Recipe Recipe;

        public RecipeViewModel(Recipe Recipe)
        {
            this.Recipe = Recipe;
        }
    }
}
