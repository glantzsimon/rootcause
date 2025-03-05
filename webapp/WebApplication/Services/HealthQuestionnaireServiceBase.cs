using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.Services
{
    public class HealthQuestionnaireServiceBase : BaseService
    {
        private readonly IRepository<HealthQuestionnaire> _healthQuestionnaireRepository;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Ingredient> _ingredientsRepository;
        private readonly IRepository<ProductIngredient> _productIingredientsRepository;
        private readonly IRepository<Protocol> _protocolsRepository;
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<DietaryRecommendation> _dietaryRecommendationsRepository;
        private readonly IRepository<FoodItem> _foodItemsRepository;
        private readonly IRepository<ProtocolActivity> _protocolActivitiesRepository;
        private readonly IRepository<ProtocolDietaryRecommendation> _protocolDietaryRecommendationRepository;
        private readonly IRepository<ProtocolFoodItem> _protocolFoodItemsRepository;
        private readonly IRepository<ProtocolProduct> _protocolProductsRepository;
        private readonly IProtocolService _protocolService;

        public HealthQuestionnaireServiceBase(
            IRepository<Product> productsRepository,
            IRepository<HealthQuestionnaire> healthQuestionnaireRepository,
            IRepository<Protocol> protocolsRepository, IRepository<Activity> activitiesRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationsRepository,
            IRepository<FoodItem> foodItemsRepository,
            IRepository<Ingredient> ingredientsRepository,
            IRepository<ProductIngredient> productIngredientsRepository,
            IRepository<IngredientSubstitute> ingredientSubstitutesRepository,
            IRepository<ProtocolActivity> protocolActivitiesRepository,
            IRepository<ProductIngredientSubstitute> productIngredientSubstitutesRepository,
            IRepository<ProtocolDietaryRecommendation> protocolDietaryRecommendationRepository,
            IRepository<ProtocolFoodItem> protocolFoodItemsRepository,
            IRepository<ProtocolProduct> protocolProductsRepository,
            IProtocolService protocolService, 
            IServiceBasePackage servicePackage) :
            base(servicePackage)
        {
            _healthQuestionnaireRepository = healthQuestionnaireRepository;
            _productsRepository = productsRepository;
            _protocolsRepository = protocolsRepository;
            _ingredientsRepository = ingredientsRepository;
            _productIingredientsRepository = productIngredientsRepository;
            _activitiesRepository = activitiesRepository;
            _dietaryRecommendationsRepository = dietaryRecommendationsRepository;
            _foodItemsRepository = foodItemsRepository;
            _protocolActivitiesRepository = protocolActivitiesRepository;
            _protocolDietaryRecommendationRepository = protocolDietaryRecommendationRepository;
            _protocolFoodItemsRepository = protocolFoodItemsRepository;
            _protocolProductsRepository = protocolProductsRepository;
            _protocolService = protocolService;
        }

        internal GeneticProfileMatchedItemsViewModel GetGeneticProfileMatchedItems(int id)
        {
            var hq = _healthQuestionnaireRepository.Find(id);
            if (hq == null || !hq.IsComplete())
            {
                return null;
            }

            var genoType = hq.CalculateGenotype();
            if (genoType == null)
            {
                return null;
            }

            var result = new GeneticProfileMatchedItemsViewModel
            {
                Products = GetGenoTypeFilteredItems(hq, genoType.GenoType,
                    new List<Product>(_productsRepository.List()), 9),
                Ingredients = GetGenoTypeFilteredItems(hq, genoType.GenoType,
                    new List<Ingredient>(_ingredientsRepository.List())),
                DietaryRecommendations = GetGenoTypeFilteredItems(hq, genoType.GenoType,
                    new List<DietaryRecommendation>(_dietaryRecommendationsRepository.List())),
                Activities =
                    GetGenoTypeFilteredItems(hq, genoType.GenoType, new List<Activity>(_activitiesRepository.List())),
                Foods = GetGenoTypeFilteredFoodItems(hq, genoType.GenoType)
            };

            result.UpdateRelativeScores();

            return result;
        }

        internal List<T> GetGenoTypeFilteredItems<T>(HealthQuestionnaire hq, EGenoType genoType, List<T> items, int take = -1) where T : GenoTypeBase
        {
            foreach (var item in items)
            {
                item.Score = GetGenoTypeItemScore(hq, genoType, item);
            }

            var results = items
                    .Where(e => e.Score > 0)
                    .OrderByDescending(e => e.Score).ToList();

            if (take > 0)
            {
                results = results.Take(take).ToList();
            }

            return results;
        }

        internal List<FoodItem> GetGenoTypeFilteredFoodItems(HealthQuestionnaire hq, EGenoType genoType)
        {
            var foodItems = _foodItemsRepository.List();

            foreach (var foodItem in foodItems)
            {
                var foodScore = GetGenoTypeItemScore(hq, genoType, foodItem);
                foodScore += new SeasonScore().GetScore(hq, hq.CurrentSeason, foodItem);
                foodScore += new FiveElementScore().GetScore(hq, foodItem);
                foodScore += new DoshasScore().GetScore(hq, foodItem);
                foodItem.Score = foodScore;
            }

            // Warn about A1 lectins
            if (hq.IsLowLectin)
            {
                foodItems.Where(e => e.Category == EFoodCategory.Dairy).ToList().ForEach(e => e.Name = $"{e.Name} ({Globalisation.Dictionary.A2MilkOnly})");
            }

            var results = foodItems
                .OrderByDescending(e => e.Score).ToList();

            return results;
        }

        internal int GetGenoTypeItemScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            var score = 0;

            var scoredProperties = hq.GetPropertiesWithAttribute(typeof(ScoreAttribute)).ToList();
            foreach (var scoredProperty in scoredProperties)
            {
                var scoredAttribute = scoredProperty.GetAttribute<ScoreAttribute>();
                var attributeProperties = scoredAttribute.GetProperties();
                foreach (var attributeProperty in attributeProperties.Where(e => item.HasProperty(e.Name)))
                {
                    var attributeValue = scoredAttribute.GetProperty(attributeProperty);
                    var itemValue = item.GetProperty(attributeProperty.Name);

                    if (attributeValue == itemValue)
                    {
                        score++;
                    }
                }
            }

            // Custom scores
            var scores = GetType().Assembly.GetTypes()
                .Where(p => p.IsClass && typeof(IScore).IsAssignableFrom(p)).ToList();

            foreach (var scoreImplementation in scores)
            {
                var instance = Activator.CreateInstance(scoreImplementation) as IScore;
                score += instance.GetScore(hq, genoType, item);
            }

            return score;
        }
        
    }
}