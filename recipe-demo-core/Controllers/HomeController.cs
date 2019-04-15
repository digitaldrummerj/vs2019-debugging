using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Recipe.Monolith.Models;

namespace Recipe.Monolith.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            RecipesViewModel recipes = RecipesViewModel.GetRecipeViewModel(12);

            ViewData["Title"] = recipes.Title;

            return View(recipes);
        }

        public IActionResult Recipe(string id)
        {
            long idAsLong = 0;
            if (!long.TryParse(id, out idAsLong))
            {
                throw new Exception("Invalid id");
            }

            Models.Recipe recipe = RecipeManager.GetRecipeById(idAsLong);
            if (recipe == null)
            {
                throw new Exception($"Recipe not found for id {id}");
            }

            // Demo: Data breakpoints 
            if (recipe.Hits == null)
            {
                recipe.Hits = 1;
            }
            else
            {
                recipe.Hits++;
            }
            RecipeManager.UpdateRecipe(idAsLong, recipe);

            return View(recipe);
        }

        public IActionResult Random()
        {
            Models.Recipe recipe = RecipeManager.GetRecipeRandom();
            return View("Recipe", recipe);
        }

        public IActionResult SearchResults(string searchString)
        {
            List<Models.Recipe> recipes = new List<Models.Recipe>();

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = RecipeManager.GetAllByName(searchString);
            }
            ViewData["Recipes"] = recipes;
            ViewData["Title"] = "SearchResults";

            return View(recipes);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
