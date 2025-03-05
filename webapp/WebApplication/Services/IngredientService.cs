using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.Services
{
    public class IngredientService : BaseService, IIngredientService
    {
        private readonly ILogger _logger;
        private readonly IRepository<Ingredient> _ingredientsRepository;

        public IngredientService(
            IRepository<Ingredient> ingredientsRepository,
            IRepository<Product> productsRepository,
            IRepository<Protocol> protocolsRepository,
            IRepository<ProductIngredient> productIngredientsRepository,
            IRepository<Activity> activitiesRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationsRepository,
            IRepository<FoodItem> foodItemsRepository,
            IServiceBasePackage serviceBasePackage)
            : base(serviceBasePackage)
        {
            _ingredientsRepository = ingredientsRepository;
        }

        public Ingredient Find(int id)
        {
            return _ingredientsRepository.Find(id);
        }

        public Ingredient FindNext(int id)
        {
            return _ingredientsRepository.Find(e => e.Id > id).OrderBy(e => e.Id).FirstOrDefault() ?? _ingredientsRepository.GetQuery("SELECT TOP 1 * FROM [Ingredient] ORDER BY [Id]").FirstOrDefault();
        }

        public Ingredient FindPrevious(int id)
        {
            return _ingredientsRepository.Find(e => e.Id < id).OrderByDescending(e => e.Id).FirstOrDefault() ?? _ingredientsRepository.GetQuery("SELECT TOP 1 * FROM [Ingredient] ORDER BY [Id] DESC").FirstOrDefault();
        }

        public Ingredient Find(string seoFriendlyId)
        {
            return _ingredientsRepository.Find(e => e.SeoFriendlyId == seoFriendlyId).FirstOrDefault();
        }

        public Ingredient DuplicateIngredient(int id)
        {
            var ingredient = Find(id);
            if (ingredient == null)
            {
                return null;
            }

            var newIngredient = new Ingredient();
            ingredient.MapTo(newIngredient);
            newIngredient.Id = 0;
            var newIngredientName = $"{ingredient.Name} Copy";
            newIngredient.Name = newIngredientName;

            _ingredientsRepository.Create(newIngredient);
            newIngredient = Find(newIngredientName);
            if (newIngredient == null)
            {
                throw new Exception("Error duplicating ingredient");
            }

            return ingredient;
        }

        public List<Ingredient> List()
        {
            return _ingredientsRepository.Find(e => !e.IsDeleted).OrderBy(e => e.Name).ToList();
        }
        
        public void UpdateIngredientCategories()
        {
            var ingredients = _ingredientsRepository.List();
            var ingredientCategories = Constants.Constants.IngredientCategories;

            if (ingredients.Any(e => !ingredientCategories.Contains(e.Category)))
            {
                throw new Exception("Cannot update ingredients. Not all items have a valid ItemCode");
            }

            var vitamins = ingredients.Where(e => e.Category == ECategory.Vitamin).ToList();
            var minerals = ingredients.Where(e => e.Category == ECategory.Mineral).ToList();
            var phytoNutrients = ingredients.Where(e => e.Category == ECategory.Phytonutrient).ToList();
            var herbs = ingredients.Where(e => e.Category == ECategory.Herb).ToList();
            var superfoods = ingredients.Where(e => e.Category == ECategory.Superfood).ToList();
            var others = ingredients.Where(e => e.Category == ECategory.Other).ToList();
            var aminoAcids = ingredients.Where(e => e.Category == ECategory.AminoAcid).ToList();
            var itemCode = 0;

            if (vitamins.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Vitamin + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in vitamins.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (minerals.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Mineral + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in minerals.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (phytoNutrients.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Phytonutrient + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in phytoNutrients.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (herbs.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Herb + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in herbs.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (superfoods.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Superfood + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in superfoods.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (others.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.Other + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in others.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }

            if (aminoAcids.All(e => e.ItemCode == 0))
            {
                itemCode = (int)ECategory.AminoAcid + Constants.Constants.ItemCodeGap;
                foreach (var ingredient in aminoAcids.OrderBy(e => e.Name).ToList())
                {
                    ingredient.ItemCode = itemCode;
                    _ingredientsRepository.Update(ingredient);
                    itemCode += Constants.Constants.ItemCodeGap;
                }
            }
        }

        public List<IngredientItem> ListIngredientItems()
        {
            var ingredients = List();
            var ingredientsItems = new List<IngredientItem>();

            foreach (var ingredient in ingredients)
            {
                var ingredientItem = ingredient.MapTo<IngredientItem>();
                ingredientItem.IngredientTypeText = ingredient.GetIngredientTypeText();
                ingredientsItems.Add(ingredientItem);
            }

            return ingredientsItems;
        }
    }
}

