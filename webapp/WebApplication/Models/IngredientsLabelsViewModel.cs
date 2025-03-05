using K9.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.Models
{
    public class IngredientsLabelsViewModel
    {
        public List<Ingredient> Ingredients { get; set; }
        public List<Ingredient> SelectIngredients => Ingredients?.Where(e => e.IsSelected).ToList();

        public IngredientsLabelsViewModel()
        {
        }

        public IngredientsLabelsViewModel(List<Ingredient> ingredients)
        {
            Ingredients = ingredients.OrderBy(e => e.CategoryText).ThenBy(e => e.Name).ToList();
        }
    }
}