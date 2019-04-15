using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipe.Monolith.Models
{
    public class RecipesViewModel
    {
        public static RecipesViewModel GetRecipeViewModelByName(string searchTerm)
        {
            RecipesViewModel search = null;

            List<Recipe> recipes =  GetRecipeByName(searchTerm);

            if(recipes != null && recipes.Count == 1 && recipes[0] != null && recipes[0].Id == null)
            {
                recipes = null;
            }

            search = new RecipesViewModel(searchTerm, recipes);

            return search;
        }
        public static RecipesViewModel GetRecipeViewModelByHighestRated()
        {
            RecipesViewModel search = null;

            List<Recipe> recipes = GetAllRecipesByRating();
            search = new RecipesViewModel("", recipes);

            return search;
        }

        public static RecipesViewModel GetRecipeViewModel(int NumberOfRecipes)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            RecipesViewModel search = null;

            List<Recipe> recipes =  GetAll(NumberOfRecipes);
            //DEMO: DebuggerDisplay View
            // DEMO: Tracepoints ($FUNCTION {response.Data.Count} matches), show parameter values in the Call Stack 
            search = new RecipesViewModel("", recipes);
            //DEMO: $ReturnValue, $ReturnValue1 etc...
            var temp = "   Hello world!   ".ToLower().ToUpper().Trim();

            GetRecipeByName(recipes.First().Title);
            return search;
        }

        private static List<Models.Recipe> GetAllRecipes()
        {
            // DEMO: 01 - Run To 
            List<Models.Recipe> recipes = new List<Recipe>();
            try
            {
                recipes = GetAll(int.MaxValue);
            }
            catch (Exception ex)
            {
                return null;
            }

            return recipes;
        }

        private static List<Models.Recipe> GetRecipeByName(string name)
        {
            List<Models.Recipe> recipes = new List<Recipe>();

            try
            {
                recipes = RecipeManager.GetAllByName(name);

                // DEMO: Step Into Specific
                string title = GetFancyName(GetRandom(recipes).Title.ToUpper());
            }
            catch (Exception ex)
            {
                return null;
            }
            return recipes;
        }
        
        private static List<Models.Recipe> GetAllRecipesByRating()
        {
            return RecipeManager.GetAll();
        }
        private static List<Models.Recipe> GetAll(int NumberOfRecipes)
        {
            return RecipeManager.GetAll(NumberOfRecipes);
        }

        private static Recipe GetRandom(List<Recipe> Recipes)
        {
            // DEMO: NSE
            int index = Global.Singleton.Random.Next(0, Recipes.Count);

            return Recipes[index];
        }

        private static string GetFancyName(string Name)
        {
            return Name.ToUpper();
        }

        public string SearchTerm
        {
            get;
            private set;
        }
        public List<Recipe> Recipes
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public RecipesViewModel(string searchTerm, List<Recipe> recipes)
        {
            this.Recipes = recipes;
            this.SearchTerm = searchTerm;
            this.Title = "Fabrikam Recipes";
        }
    }
}