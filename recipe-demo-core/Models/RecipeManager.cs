using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;

namespace Recipe.Monolith.Models
{
    public class RecipeManager
    {

        private static Dictionary<long?, Recipe> recipes = null;
        private static IEnumerator<long?> keysEnumerator;
        private static string resolvedPath;

        public Recipe NextRecipe
        {
            get
            {
                if (!keysEnumerator.MoveNext())
                {
                    keysEnumerator.Reset();
                }

                return recipes[keysEnumerator.Current];
            }
            set { }
        }

        static RecipeManager()
        {
            recipes = new Dictionary<long?, Recipe>();
            resolvedPath =  Path.Combine(Global.Singleton.DataPath, "Recipes"); 

            string[] filenames = Directory.GetFiles(resolvedPath);
            foreach(string filename in filenames)
            {
                // DEMO: Step Into Specific
                string json = File.ReadAllText(filename);
                Recipe recipe = LoadRecipeFromJson(json);

                if (recipe.Hits == null)
                {
                    recipe.Hits = 0;
                }
                recipes.Add(recipe.Id, recipe);
            }

            // Demo: Data breakpoints (set bp here)
            keysEnumerator = recipes.Keys.GetEnumerator();
        }

        public static List<Recipe> GetAll()
        {
            return recipes.Values.ToList();
        }

        public static List<Recipe> GetAll(int Count)
        {
            return recipes.Values.Take<Recipe>(Count).ToList();
        }

        public static Recipe GetRecipeById(long id)
        {
            if(!recipes.ContainsKey(id))
            {
                return null;
            }
            return recipes[id];
        }

        public static Recipe GetRecipeRandom()
        {
            // This is a bit crazy but it's all for a demo! 
            int crazyMultiplier = 100000;
            int index = 0;
            Recipe randomRecipe = null;

            double randomNumber = Global.Singleton.Random.NextDouble() / crazyMultiplier;

            // DEMO: Double visualizer
            index = (int)(randomNumber * recipes.Count * crazyMultiplier);

            randomRecipe = recipes.Values.ElementAt(index);

            return randomRecipe;
        }

        // GetAllByName
        public static List<Recipe> GetAllByName(string Name)
        {
            return Search(Name, 0, 100);
        }

        public static List<Recipe> Search(string term, int start, int limit)
        {
            // Note: This is obvioussly insane and done for the sake of a demo
            List<Recipe> recipesArray = new List<Recipe>();

            foreach(Recipe r in recipes.Values.ToList())
            {
                if(!string.IsNullOrWhiteSpace(term))
                {
                    if(r.Title.IndexOf(term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        recipesArray.Add(r);
                    }
                }
                else
                {
                    recipesArray.Add(r);
                }
            }

            for (int i = 0; i < recipesArray.Count; i++)
            {
                for (int j = 0; j < recipesArray.Count - i - 1; j++)
                {
                    if (recipesArray[j].SpoonacularScore > recipesArray[j + 1].SpoonacularScore)
                    {
                        Recipe tempRecipe = recipesArray[j];
                        recipesArray[j] = recipesArray[j + 1];
                        recipesArray[j + 1] = tempRecipe;
                    }
                }
            }
            
            IEnumerable<Recipe> returnRecipe = recipesArray.ToList();
            returnRecipe = returnRecipe.Skip(start).Take(limit);

            return (List<Recipe>)returnRecipe.ToList();
        }

        public static void UpdateRecipe(long id, Models.Recipe recipe)
        {
            // TODO: when there's time, write the updated stuff to the actual file
            // recipes[id] = recipe;

            // Write recipe updates to original JSON file
            string recipeFile = resolvedPath + "/" + recipe.Id + ".json";
            using (StreamWriter file = File.CreateText(recipeFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, recipe);
            }
        }
        
        private static Recipe LoadRecipeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Recipe>(json, Converter.Settings);
        }

        internal class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
            };
        }

    }
}