using K9.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.ViewModels
{
    public class GeneticProfileMatchedItemsViewModel
    {
        public List<ProductPack> ProductPacks { get; set; }
        public List<Product> Products { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Activity> Activities { get; set; }
        public List<DietaryRecommendation> DietaryRecommendations { get; set; }
        public List<FoodItem> Foods { get; set; }

        /// <summary>
        /// Checks the score of all other items in the category, and sets the RelativeScore value from 1 to 100 (100 being the max score in the collection)
        /// </summary>
        public void UpdateRelativeScores()
        {
            UpdateRelativeScores(ProductPacks);
            UpdateRelativeScores(Products);
            UpdateRelativeScores(Ingredients);
            UpdateRelativeScores(Activities);
            UpdateRelativeScores(DietaryRecommendations);
            UpdateRelativeScores(Foods);
        }

        private void UpdateRelativeScores<T>(List<T> items)  where T : GenoTypeBase
        {
            if (items != null & items.Any())
            {
                var maxScore = items.Max(e => e.Score);
                foreach (var item in items)
                {
                    if (item.Score >= 0)
                    {
                        item.RelativeScore = (int)Math.Ceiling(((double)item.Score / maxScore) * 100);
                    }
                    else
                    {
                        item.RelativeScore = item.Score;
                    }
                }
            }
        }
    }
}