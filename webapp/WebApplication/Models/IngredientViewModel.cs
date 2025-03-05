using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Models
{
    public class IngredientViewModel
    {
        public Ingredient Ingredient { get; set; }
        public string IngredientName => Ingredient?.Name;
        public float TotalAmountUsed { get; set; }
        public int TotalProductsUsedIn { get; set; }
        public int TotalOrdersUsedIn { get; set; }

    }
}